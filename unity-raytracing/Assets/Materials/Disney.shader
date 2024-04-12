Shader "Disney" 
{
    Properties 
    {
        mainTex("Albedo", 2D) = "white" {}
        basecolor("Basecolor", Vector) = (1,1,1,1)
        normalMap("NormalMap", 2D) = "white" {}
        metallic("Metallic", Range(0, 1)) = 0
        eta("Eta", Range(1, 4)) = 1.5
        roughness("Roughness", Range(0, 1)) = 0
        anisotropic("Anisotropic", Range(0, 1)) = 0
        specularTint("SpecularTint", Range(0, 1)) = 0
        specTrans("SpecTrans", Range(0, 1)) = 0
        sheen("Sheen", Range(0, 1)) = 0
        sheenTint("SheenTint", Range(0, 1)) = 0
        clearcoat("Clearcoat", Range(0, 1)) = 0
        clearcoatGloss("ClearcoatGloss", Range(0, 1)) = 0

        scatterDistance("ScatterDistance", Vector) = (0,0,0,0)
        
        diffTrans("DiffTrans", Color) = (0,0,0,0)
        flatness("Flatness", Color) = (0,0,0,0)
    }
    SubShader 
    {
	    Tags 
        {
            "RenderType"="Opaque"  "Queue"="Geometry"
        } 
        Pass 
        {
            Name "FORWARD"
            Tags 
            {
                "LightMode"="ForwardBase"
            }
       
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma target 3.0
            
            sampler2D mainTex;
            sampler2D normalMap;

            float4 basecolor;
            float metallic;
            float eta;
            float roughness;
            float anisotropic;
            float specularTint;
            float specTrans;
            float sheen;
            float sheenTint;
            float clearcoat;
            float clearcoatGloss;
            float4 scatterDistance;
            float4 diffTrans;
            float4 flatness;

            struct VertexInput 
            {
                float4 vertex : POSITION;       //local vertex position
                float3 normal : NORMAL;         //normal direction
                float4 tangent : TANGENT;       //tangent direction    
                float2 texcoord0 : TEXCOORD0;   //uv coordinates
                float2 texcoord1 : TEXCOORD1;   //lightmap uv coordinates
            };

            struct VertexOutput 
            {
                float4 pos : SV_POSITION;              //screen clip space position and depth
                float2 uv0 : TEXCOORD0;                //uv coordinates
                float2 uv1 : TEXCOORD1;                //lightmap uv coordinates

                float3 normalDir : TEXCOORD3;          //normal direction   
                float3 posWorld : TEXCOORD4;          //normal direction   
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)                   //this initializes the unity lighting and shadow
                UNITY_FOG_COORDS(9)                    //this initializes the unity fog
            };

            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;           
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }

            #define Pi (3.1415926)
            #define InvPi (1.0 / Pi)
            
            float Y(float3 c)
            {
                return 0.299 * c.r + 0.587 * c.g + 0.114 * c.b;
            }

            float sqr(float v)
            {
                return v * v;
            }

            bool IsBlack(float3 c)
            {
                return !any(c);
            }

            bool SameHemisphere(float3 w, float3 wp) 
            {
                return w.z * wp.z > 0;
            }

            float CosTheta(float3 w) 
            {
                return w.z;
            }
            
            float Cos2Theta(float3 w) 
            {
                return sqr(w.z);
            }
            
            float AbsCosTheta(float3 w) 
            {
                return abs(w.z);
            }

            float Sin2Theta(float3 w) 
            {
                return max(0.0, 1.0 - Cos2Theta(w));
            }
            
            float SinTheta(float3 w) 
            {
                return sqrt(Sin2Theta(w));
            }

            float TanTheta(float3 w) 
            {
                return SinTheta(w) / CosTheta(w);
            }

            float Tan2Theta(float3 w) 
            {
                return Sin2Theta(w) / Cos2Theta(w);
            }

            float CosPhi(float3 w) 
            {
                float sinTheta = SinTheta(w);
                return (sinTheta == 0) ? 1 : clamp(w.x / sinTheta, -1, 1);
            }

            float SinPhi(float3 w) 
            {
                float sinTheta = SinTheta(w);
                return (sinTheta == 0) ? 0 : clamp(w.y / sinTheta, -1, 1);
            }

            float CosDPhi(float3 wa, float3 wb) 
            {
                float waxy = sqr(wa.x) + sqr(wa.y), wbxy = sqr(wb.x) + sqr(wb.y);
                if (waxy == 0 || wbxy == 0)
                    return 1;
                return clamp((wa.x * wb.x + wa.y * wb.y) / sqrt(waxy * wbxy), -1, 1);
            }            

            float Cos2Phi(float3 w) 
            { 
                return CosPhi(w) * CosPhi(w); 
            }

            float Sin2Phi(float3 w) 
            {
                return SinPhi(w) * SinPhi(w); 
            }           

            float AbsDot(float3 v1, float3 v2) 
            {
                return abs(dot(v1, v2));
            }
 
       
            float SchlickWeight(float cosTheta) 
            {
                float m = clamp(1 - cosTheta, 0, 1);
                return (m * m) * (m * m) * m;
            }

            float SchlickR0FromEta(float eta) 
            { 
                return sqr(eta - 1) / sqr(eta + 1); 
            }

            float Lerp(float t, float v1, float v2) 
            { 
                return lerp(v1, v2, t);
            }

            float4 Lerp(float t, float4 v1, float4 v2) 
            { 
                return lerp(v1, v2, t);
            }            

            float3 Faceforward(float3 n, float3 v) 
            {
                return (dot(n, v) < 0.f) ? -n : n;
            }

            bool IsInf(float v)
            {
                return isinf(v);
            }
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Diffuse Reflection: Diffuse
            float4 DisneyDiffuse_f(float4 R, float3 wo, float3 wi)
            {
                float Fo = SchlickWeight(AbsCosTheta(wo));
                float Fi = SchlickWeight(AbsCosTheta(wi));

                return R * InvPi * (1 - Fo / 2) * (1 - Fi / 2);                
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Diffuse Reflection: FakeSS
            float4 DisneyFakeSS_f(float4 R, float roughness, float3 wo, float3 wi)
            {
                float3 wh = wi + wo;
                if (wh.x == 0 && wh.y == 0 && wh.z == 0) 
                    return float4(0., 0., 0., 1.);
                wh = normalize(wh);
                float cosThetaD = dot(wi, wh);

                // Fss90 used to "flatten" retroreflection based on roughness
                float Fss90 = cosThetaD * cosThetaD * roughness;
                float Fo = SchlickWeight(AbsCosTheta(wo));
                float Fi = SchlickWeight(AbsCosTheta(wi));
                float Fss = Lerp(Fo, 1.0, Fss90) * Lerp(Fi, 1.0, Fss90);
                // 1.25 scale is used to (roughly) preserve albedo
                float ss = 1.25f * (Fss * (1 / (AbsCosTheta(wo) + AbsCosTheta(wi)) - .5f) + .5f);

                return R * InvPi * ss;
            }            

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Diffuse Reflection: Subsurface
            float4 SpecularTransmission_f(float3 T, float etaA, float etaB, float3 wo, float3 wi)
            {
                return float4 (0., 0., 0., 1.);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Diffuse Reflection: Retro
            float4 DisneyRetro_f(float4 R, float roughness, float3 wo, float3 wi)
            {
                float3 wh = wi + wo;
                if (wh.x == 0 && wh.y == 0 && wh.z == 0) 
                    return float4 (0., 0., 0., 1.);
                wh = normalize(wh);
                float cosThetaD = dot(wi, wh);

                float Fo = SchlickWeight(AbsCosTheta(wo));
                float Fi = SchlickWeight(AbsCosTheta(wi));
                float Rr = 2 * roughness * cosThetaD * cosThetaD;

                // Burley 2015, eq (4).
                return R * InvPi * Rr * (Fo + Fi + Fo * Fi * (Rr - 1));
            }            

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Diffuse Reflection: Sheen
            float4 DisneySheen_f(float4 R, float3 wo, float3 wi)
            {
                float3 wh = wi + wo;
                if (wh.x == 0 && wh.y == 0 && wh.z == 0) 
                    return float4 (0., 0., 0., 1.);
                wh = normalize(wh);
                float cosThetaD = dot(wi, wh);

                return R * SchlickWeight(cosThetaD);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Glossy Reflection: Microfacet

            // Distribution and Geometric
            float TrowbridgeReitzDistribution_Lambda(float alphax, float alphay, float3 wh)
            {
                float absTanTheta = abs(TanTheta(wh));
                if (IsInf(absTanTheta)) 
                    return 0.;
                
                // Compute _alpha_ for direction _wh_
                float alpha = sqrt(Cos2Phi(wh) * alphax * alphax + Sin2Phi(wh) * alphay * alphay);
                float alpha2Tan2Theta = (alpha * absTanTheta) * (alpha * absTanTheta);
                return (-1 + sqrt(1.f + alpha2Tan2Theta)) / 2;
            }

            float TrowbridgeReitzDistribution_G(float alphax, float alphay, float3 wo, float3 wi)
            {
                return 1.0 / (1.0 + TrowbridgeReitzDistribution_Lambda(alphax, alphay, wo) + TrowbridgeReitzDistribution_Lambda(alphax, alphay, wi));
            }            

            float TrowbridgeReitzDistribution_D(float alphax, float alphay, float3 wh)
            {
                float tan2Theta = Tan2Theta(wh);
                if (IsInf(tan2Theta)) 
                    return 0.0;
                
                float cos4Theta = Cos2Theta(wh) * Cos2Theta(wh);
                float e = (Cos2Phi(wh) / (alphax * alphax) + Sin2Phi(wh) / (alphay * alphay)) * tan2Theta;

                return 1 / (Pi * alphax * alphay * cos4Theta * (1 + e) * (1 + e));
            }

            // Fresnel
            float FrDielectric(float cosThetaI, float etaI, float etaT) 
            {
                cosThetaI = clamp(cosThetaI, -1, 1);
                // Potentially swap indices of refraction
                bool entering = cosThetaI > 0.f;
                if (!entering) 
                {
                    float temp = etaT;
                    etaT = etaI;
                    etaI = temp;
                    cosThetaI = abs(cosThetaI);
                }

                // Compute _cosThetaT_ using Snell's law
                float sinThetaI = sqrt(max(0, 1 - cosThetaI * cosThetaI));
                float sinThetaT = etaI / etaT * sinThetaI;

                // Handle total internal reflection
                if (sinThetaT >= 1)
                    return 1;
                float cosThetaT = sqrt(max(0, 1 - sinThetaT * sinThetaT));
                float Rparl = ((etaT * cosThetaI) - (etaI * cosThetaT)) / ((etaT * cosThetaI) + (etaI * cosThetaT));
                float Rperp = ((etaI * cosThetaI) - (etaT * cosThetaT)) / ((etaI * cosThetaI) + (etaT * cosThetaT));
                
                return (Rparl * Rparl + Rperp * Rperp) / 2;
            }            

            float4 FrSchlick(float4 R0, float cosTheta) 
            {
                return Lerp(SchlickWeight(cosTheta), R0, float4(1.0, 1.0, 1.0, 1.0));
            }            

            float FrSchlick(float R0, float cosTheta) 
            {
                return Lerp(SchlickWeight(cosTheta), R0, 1);
            }

            float4 DisneyFresnel_Evaluate(float4 Cspec0, float metallicWeight, float eta, float cosI)
            {
                float d = FrDielectric(cosI, 1, eta);
                
                return Lerp(metallicWeight, float4(d, d, d, d), FrSchlick(Cspec0, cosI));
            }            

            float4 MicrofacetReflection_f(float4 R, float alphax, float alphay, float4 Cspec0, float metallicWeight, float eta, float3 wo, float3 wi)
            {
                float cosThetaO = AbsCosTheta(wo), cosThetaI = AbsCosTheta(wi);
                float3 wh = wi + wo;
                // Handle degenerate cases for microfacet reflection
                if (cosThetaI == 0 || cosThetaO == 0) 
                    return float4 (0., 0., 0., 1.);
                if (wh.x == 0 && wh.y == 0 && wh.z == 0) 
                    return float4 (0., 0., 0., 1.);
                wh = normalize(wh);
                
                // For the Fresnel call, make sure that wh is in the same hemisphere
                // as the surface normal, so that TIR is handled correctly.
                float4 F = DisneyFresnel_Evaluate(Cspec0, metallicWeight, eta, dot(wi, Faceforward(wh, float3(0, 0, 1))));
                
                return R * TrowbridgeReitzDistribution_D(alphax, alphay, wh) * TrowbridgeReitzDistribution_G(alphax, alphay, wo, wi) * F / (4 * cosThetaI * cosThetaO);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Glossy Reflection: ClearCoat
            float GTR1(float cosTheta, float alpha) 
            {
                float alpha2 = alpha * alpha;

                return (alpha2 - 1) / (Pi * log(alpha2) * (1 + (alpha2 - 1) * cosTheta * cosTheta));
            }            

            float smithG_GGX(Float cosTheta, Float alpha) 
            {
                float alpha2 = alpha * alpha;
                float cosTheta2 = cosTheta * cosTheta;
                return 1 / (cosTheta + sqrt(alpha2 + cosTheta2 - alpha2 * cosTheta2));
            }

            float4 DisneyClearcoat_f(float weight, float gloss, float3 wo, float3 wi)
            {
                float3 wh = wi + wo;
                if (wh.x == 0 && wh.y == 0 && wh.z == 0) 
                    return float4 (0., 0., 0., 1.);
                wh = normalize(wh);

                // Clearcoat has ior = 1.5 hardcoded -> F0 = 0.04. It then uses the
                // GTR1 distribution, which has even fatter tails than Trowbridge-Reitz
                // (which is GTR2).
                float Dr = GTR1(AbsCosTheta(wh), gloss);
                float Fr = FrSchlick(.04, dot(wo, wh));
                // The geometric term always based on alpha = 0.25.
                float Gr = smithG_GGX(AbsCosTheta(wo), .25) * smithG_GGX(AbsCosTheta(wi), .25);

                return weight * Gr * Fr * Dr / 4;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Glossy Transmission: Microfacet
            float4 FresnelDielectric_Evaluate(float etaI, float etaT, float cosThetaI)
            {
                return FrDielectric(cosThetaI, etaI, etaT);
            }            

            float4 MicrofacetTransmission_f(float4 T, float alphax, float alphay, float etaA, float etaB, float3 wo, float3 wi)
            {
                if (SameHemisphere(wo, wi)) 
                    return float4 (0., 0., 0., 1.);

                float cosThetaO = CosTheta(wo);
                float cosThetaI = CosTheta(wi);
                if (cosThetaI == 0 || cosThetaO == 0) 
                    return float4 (0., 0., 0., 1.);

                // Compute $\wh$ from $\wo$ and $\wi$ for microfacet transmission
                float eta = CosTheta(wo) > 0 ? (etaB / etaA) : (etaA / etaB);
                float3 wh = normalize(wo + wi * eta);
                if (wh.z < 0) 
                    wh = -wh;

                // Same side?
                if (dot(wo, wh) * dot(wi, wh) > 0) 
                    return float4 (0., 0., 0., 1.);

                float4 F = FresnelDielectric_Evaluate(etaA, etaB, dot(wo, wh));

                float sqrtDenom = dot(wo, wh) + eta * dot(wi, wh);
                //float factor = (mode == TransportMode::Radiance) ? (1 / eta) : 1;
                float factor = (1 / eta);

                return (float4(1., 1., 1., 1.) - F) * T * 
                                abs( TrowbridgeReitzDistribution_D(alphax, alphay, wh) * TrowbridgeReitzDistribution_G(alphax, alphay, wo, wi)  * eta * eta *
                                AbsDot(wi, wh) * AbsDot(wo, wh) * factor * factor / (cosThetaI * cosThetaO * sqrtDenom * sqrtDenom));
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Diffuse Transmission: Microfacet
            float4 LambertianTransmission_f(float4 T, float3 wo, float3 wi)
            {
                return T * InvPi;
            }

            //-------------------------- 
            float4 frag(VertexOutput i) : COLOR 
            {
                //normal direction calculations
                float3 N = normalize(i.normalDir);
                float3 V = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float shiftAmount = dot(i.normalDir, V);
                N = shiftAmount < 0.0f ? N + V * (-shiftAmount + 1e-5f) : N;
                //light calculations
                float3 L = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                
                // To Render Frame
                //float3 normalDir : TEXCOORD3;
                //float3 tangentDir : TEXCOORD5;
                //float3 bitangentDir : TEXCOORD6;
                float3x3 rotation = transpose(float3x3( i.tangentDir, i.bitangentDir, i.normalDir));
                N = mul(N, rotation);
                V = mul(V, rotation);
                L = mul(L, rotation);

                // float3 lightReflectDirection = reflect( -L, N );
                // float3 viewReflectDirection = normalize(reflect( -V, N ));
                float NdotL = max(0.0, dot( N, L ));
                float3 H = normalize(V + L); 
                float NdotH =  max(0.0,dot( N, H));
                float NdotV =  max(0.0,dot( N, V));
                float VdotH = max(0.0,dot( V, H));
                float LdotH =  max(0.0,dot(L, H)); 
                float LdotV = max(0.0,dot(L, V)); 
                float RdotV = max(0.0, dot( reflect( -L, N ), V ));
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.rgb;

                // mulit compile
                bool thin = true;                

                // Diffuse
	            float4 c = basecolor * tex2D(mainTex, i.uv0);
                float metallicWeight = metallic;
                float e = eta;
                float strans = specTrans;
                float diffuseWeight = (1 - metallicWeight) * (1 - specTrans);
                float dt = Y(diffTrans.rgb) / 2;  // 0: all diffuse is reflected -> 1, transmitted
                float rough = roughness;
                float lum = Y(c.rgb);
                // normalize lum. to isolate hue+sat
                float4 Ctint; 
                if(lum > 0)
                    Ctint = (c / lum);
                else
                    Ctint = float4(1.0, 1.0, 1.0, 1.0);

                float sheenWeight = sheen;
                float4 Csheen;
                if (sheenWeight > 0) 
                {
                    float stint = sheenTint;
                    Csheen = lerp(float4(1.0, 1.0, 1.0, 1.0), Ctint, stint);
                }


                float3 ng = N;
                if (V.z == 0) 
                    return float4(0.0, 0.0, 0.0, 1.0);
                float reflect = sign(dot(V, N) * dot(L, N));
                float transmit = 1.0 - reflect;

                float4 finalColor = float4(0.0, 0.0, 0.0, 1.0);
                if(diffuseWeight > 0)
                {
                    if (thin) 
                    {
                        float flat = flatness;
                        // Blend between DisneyDiffuse and fake subsurface based on
                        // flatness.  Additionally, weight using diffTrans.
                        // si->bsdf->Add(ARENA_ALLOC(arena, DisneyDiffuse)(diffuseWeight * (1 - flat) * (1 - dt) * c));
                        // si->bsdf->Add(ARENA_ALLOC(arena, DisneyFakeSS)(diffuseWeight * flat * (1 - dt) * c, rough));
                        if(reflect > 0)
                            finalColor += DisneyDiffuse_f(diffuseWeight * (1 - flat) * (1 - dt) * c, V, L);
                        if(reflect > 0)
                            finalColor += DisneyFakeSS_f(diffuseWeight * flat * (1 - dt) * c, rough, V, L);                        
                    } 
                    else 
                    {
                        float4 sd = scatterDistance; 
                        if (IsBlack(sd.rgb))
                        {
                            // 1) DiffuseMaterial(SpectrumTexture reflectance)  
                           
                            // No subsurface scattering; use regular (Fresnel modified)
                            // diffuse.
                            if(reflect > 0)
                            {
                                finalColor += DisneyDiffuse_f(diffuseWeight * c, V, L);
                            }
                        }
                        else 
                        {
                            // 4) SubsurfaceMaterial(SpectrumTexture reflectance, SpectrumTexture mfp, Float scale, SpectrumTexture sigma_a, SpectrumTexture sigma_s	   -> Scattering
                            // Float eta, 									      	   	   -> Fresnel
		                    // FloatTexture uRoughness, FloatTexture vRoughness, bool remapRoughness,			      		   -> Dielectric G, D(uRoughness, vRoughness)
		                    // Float g) 

                            // Use a BSSRDF instead.
                            if(reflect > 0)
                            {
                                finalColor += SpecularTransmission_f(1.f, 1.f, e, V, L);
                                //finalColor += DisneyBSSRDF(c * diffuseWeight, sd, *si, e, this, mode);
                            }
                        }
                    }

                    // Retro-reflection.
                    if(reflect > 0)
                        finalColor += DisneyRetro_f(diffuseWeight * c, rough, V, L);

                    // Sheen (if enabled)
                    if (sheenWeight > 0)
                    {
                        if(reflect > 0)
                        {
                            finalColor += DisneySheen_f(diffuseWeight * sheenWeight * Csheen, V, L);
                        }
                    }
                }

                if(reflect > 0)
                {
                    // 2) ConductorMaterial(Spectrum eta, (SpectrumTexture k,				             			      -> Conductor Fresnel
                    // FloatTexture uRoughness, FloatTexture vRoughness, bool remapRoughness) 			      -> Conductor G, D(uRoughness, vRoughness)


                    // Create the microfacet distribution for metallic and/or specular
                    // transmission.
                    float aspect = sqrt(1 - anisotropic * .9);
                    float ax = max(float(.001), sqr(rough) / aspect);
                    float ay = max(float(.001), sqr(rough) * aspect);
                    // MicrofacetDistribution *distrib = ARENA_ALLOC(arena, DisneyMicrofacetDistribution)(ax, ay);
                    // Specular is Trowbridge-Reitz with a modified Fresnel function.
                    float specTint = specularTint;
                    float4 Cspec0 = Lerp(metallicWeight, SchlickR0FromEta(e) * Lerp(specTint, float4(1., 1., 1., 1.), Ctint), c);
                    // Fresnel *fresnel = ARENA_ALLOC(arena, DisneyFresnel)(Cspec0, metallicWeight, e);
                    
                    finalColor += MicrofacetReflection_f(float4(1., 1., 1., 1.), ax, ay, Cspec0, metallicWeight, e, V, L);
                }

                if(reflect > 0)
                {
                    float cc = clearcoat;
                    if(cc > 0) 
                    {
                        finalColor += DisneyClearcoat_f(cc, Lerp(clearcoatGloss, .1, .001), V, L);
                    }
                }

                if(transmit > 0)
                {
                    // Walter et al's model, with the provided transmissive term scaled
                    // by sqrt(color), so that after two refractions, we're back to the
                    // provided color.
                    float aspect = sqrt(1 - anisotropic * .9);
                    float4 T = strans * sqrt(c);
                    float ax = 0.0;
                    float ay = 0.0;

                    if (thin) 
                    {
                        // 5) ThinDielectricMaterial(Spectrum eta)	

                        float rscaled = (0.65f * e - 0.35f) * rough;
                        float ax = max(0.001f, sqr(rscaled) / aspect);
                        float ay = max(0.001f, sqr(rscaled) * aspect);
                    }
                    else
                    {
                        // 3) DielectricMaterial(Spectrum eta, 									      -> Dielectric Fresnel
                        // FloatTexture uRoughness, FloatTexture vRoughness, bool remapRoughness)			      -> Dielectric G, D(uRoughness, vRoughness)
                        
                        float aspect = sqrt(1 - anisotropic * .9);
                        float ax = max(0.001f, sqr(rough) / aspect);
                        float ay = max(0.001f, sqr(rough) * aspect);
                    }

                    finalColor += MicrofacetTransmission_f(T, ax, ay, 1., e, V, L);
                }

                if (thin) 
                {
                    // 6) DiffuseTransmissionMaterial(SpectrumTexture reflectance, SpectrumTexture transmittance)			                      -> Diffuse Transmisse and Diffuse
                    finalColor += LambertianTransmission_f(dt * c, V, L);
                }
 
                UNITY_APPLY_FOG(i.fogCoord, finalColor * float4(attenColor, 1.0));
                return finalColor;
            }
        ENDCG
        }
    }

    FallBack "Legacy Shaders/Diffuse"
}