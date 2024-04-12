Shader"NerualGI" 
{
    Properties 
    {
        mainTex("Albedo", 2D) = "white" {}
        basecolor("BaseColor", Vector) = (1,1,1,1)
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
        // 自定义属性用于接收光源位置
        pointLightPosition("Light Position", Vector) = (-0.669, 1.211, 3.964, 1)

        scatterDistance("ScatterDistance", Vector) = (0,0,0,0)
        
        diffTrans("DiffTrans", Color) = (0,0,0,0)
        flatness("Flatness", Color) = (0,0,0,0)

        collectFlag("CollectFlag", Float) = 0.0
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
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "NerualFunction.cginc"
            #include "NerualFunction_Top.cginc"
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
            float4 pointLightPosition;

            float collectFlag;

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
                float3 posWorld : TEXCOORD4;          //world postion   
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)                   //this initializes the unity lighting and shadow
                UNITY_FOG_COORDS(9)                    //this initializes the unity fog
                float4 pointLightPosition : TEXCOORD10;
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
                o.pointLightPosition = pointLightPosition;
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

            int GetNodeID(float3 xWorld)
            {
                // table
                if (xWorld.z < 2.4 && xWorld.z >1.4 && xWorld.x > 1.7 && xWorld.x < 3.6 && xWorld.y < 1.5 && xWorld.y>0.5)
                    return 0;
                // ground
                if (xWorld.z < 1.8 && xWorld.z > 0.2 && xWorld.y < 1 && xWorld.x < 4 && xWorld.x > 3)
                    return 1;
                // ground_split_0
                if (xWorld.z > 1.6 && xWorld.z <= 2.1 && xWorld.y < 0.2 && xWorld.x <= 1.3 && xWorld.x > 0.8)
                    return 2;
                // ground_split_1
                if (xWorld.z > 2.1 && xWorld.z <= 2.6 && xWorld.y < 0.2 && xWorld.x <= 1.3 && xWorld.x > 0.8)
                    return 3;
                // ground_split_2
                if (xWorld.z > 1.6 && xWorld.z <= 2.1 && xWorld.y < 0.2 && xWorld.x <= 1.8 && xWorld.x > 1.3)
                    return 4;
                // ground_split_3
                if (xWorld.z > 2.1 && xWorld.z <= 2.6 && xWorld.y < 0.2 && xWorld.x <= 1.8 && xWorld.x > 1.3)
                    return 5;
                else
                    return 9;
            }

            //-------------------------- 
            float4 frag(VertexOutput i) : COLOR 
            {
                float3 N = normalize(i.normalDir);
                float3 V = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                //float3 L = float3(0.086171f, 0.691387f, 0.717327f);//normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz, _WorldSpaceLightPos0.w));
                //float3 L = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz, _WorldSpaceLightPos0.w));
                float3 L = normalize(pointLightPosition.xyz - i.posWorld.xyz);
                // 1 为点光源 0 为方向感
                // Data For Network
                float3 xWorld = i.posWorld.xyz;
                float3 nWorld = N;
                float3 vWorld = V;                
                float3 lWorld = L;
                float3 lxWorld = pointLightPosition.xyz;
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
                // float3 attenColor = attenuation * _LightColor0.rgb;
                float3 attenColor = float3(1.0,1.0,1.0);
                /*
                // mulit compile
                bool thin = true;                

                // Diffuse
	            float4 c = basecolor * tex2D(mainTex, i.uv0) * float4(attenColor, 1.0);
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
                            // No subsurface scattering; use regular (Fresnel modified)
                            // diffuse.
                            if(reflect > 0)
                            {
                                finalColor += DisneyDiffuse_f(diffuseWeight * c, V, L);
                            }
                        }
                        else 
                        {
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
                        float rscaled = (0.65f * e - 0.35f) * rough;
                        float ax = max(0.001f, sqr(rscaled) / aspect);
                        float ay = max(0.001f, sqr(rscaled) * aspect);
                    }
                    else
                    {
                        float aspect = sqrt(1 - anisotropic * .9);
                        float ax = max(0.001f, sqr(rough) / aspect);
                        float ay = max(0.001f, sqr(rough) * aspect);
                    }

                    finalColor += MicrofacetTransmission_f(T, ax, ay, 1., e, V, L);
                }

                if (thin) 
                {
                    finalColor += LambertianTransmission_f(dt * c, V, L);
                }
                */

                float4 finalColor = float4(0.0, 0.0, 0.0, 1.0);

                float4 albedo = basecolor * tex2D(mainTex, i.uv0);

                //f0,f1,f2,f3: store 16 input vectors
                //input: pos[3], wo[3], normal[3], light[3], albedo[3], roughness[1]
                fixed4 f0 = fixed4(xWorld.x, xWorld.y, xWorld.z, vWorld.x);
                fixed4 f1 = fixed4(vWorld.y, vWorld.z, nWorld.x, nWorld.y);
                fixed4 f2 = fixed4(nWorld.z, lWorld.x, lWorld.y, lWorld.z);
                fixed4 f3 = fixed4(albedo.x, albedo.y, albedo.z, roughness);

                //f0.w = (f0.w - (-0.999945)) * 2.0 / 1.9999069999999999 + (-1.0);
                //f1.x = (f1.x - (-0.999884)) * 2.0 / 1.999796 + (-1.0);
                //f1.y = (f1.y - (-0.999887)) * 2.0 / 1.999848 + (-1.0);
                //f1.z = (f1.z - (-1.0)) * 2.0 / 2.0 + (-1.0);
                //f1.w = (f1.w - (-1.0)) * 2.0 / 2.0 + (-1.0);
                //f2.x = (f2.x - (-1.0)) * 2.0 / 2.0 + (-1.0);
                //f3.x = (f3.x - (0.0)) * 2.0 / 1.000007 + (-1.0);
                //f3.y = (f3.y - (0.0)) * 2.0 / 1.000007 + (-1.0);
                //f3.z = (f3.z - (0.0)) * 2.0 / 1.000007 + (-1.0);
                //f3.w = (f3.w - (0.0)) * 2.0 / 1.000007 + (-1.0);
                 
                float vl = smoothstep(-0.2,0.2,xWorld.y);
                if(collectFlag > 0.1 && collectFlag <1.1){
                    finalColor = float4(EvaluateNetwort_Test(f0, f1, f2, f3),1.0);
                }
                else if(collectFlag==2.0)
                {
                    if(lxWorld.x<-1.4){
                        finalColor = lerp(float4(EvaluateNetwork_Left_l17_down(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Left_l17_up(f0, f1, f2, f3),1.0),vl);
                    }else if(lxWorld.x<-1.1){
                         float vt = smoothstep(-1.4,-1.1,lxWorld.x);
                         finalColor = lerp(
                             lerp(float4(EvaluateNetwork_Left_l17_down(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Left_l17_up(f0, f1, f2, f3),1.0),vl),
                            lerp(float4(EvaluateNetwork_Left_l1_down(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Left_l1_up(f0, f1, f2, f3),1.0),vl)
                        ,vt);
                    }
                    else if(lxWorld.x<-0.7){
                        finalColor = lerp(float4(EvaluateNetwork_Left_l1_down(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Left_l1_up(f0, f1, f2, f3),1.0),vl);
                    }else if(lxWorld.x<-0.3){
                         float vt = smoothstep(-0.7,-0.3,lxWorld.x);
                         finalColor = lerp(
                             lerp(float4(EvaluateNetwork_Left_l1_down(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Left_l1_up(f0, f1, f2, f3),1.0),vl),
                            lerp(float4(EvaluateNetwork_Left_0_down(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Left_0_up(f0, f1, f2, f3),1.0),vl)
                        ,vt);
                    }
                    else if(lxWorld.x<0.4){
                        finalColor = lerp(float4(EvaluateNetwork_Left_0_down(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Left_0_up(f0, f1, f2, f3),1.0),vl);
                    }else if(lxWorld.x<0.8){
                         float vt = smoothstep(0.4,0.8,lxWorld.x);
                         finalColor = lerp(
                             lerp(float4(EvaluateNetwork_Left_0_down(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Left_0_up(f0, f1, f2, f3),1.0),vl),
                            lerp(float4(EvaluateNetwork_Left_2_down(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Left_2_up(f0, f1, f2, f3),1.0),vl)
                        ,vt);
                    }
                    else{
                        finalColor = lerp(float4(EvaluateNetwork_Left_2_down(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Left_2_up(f0, f1, f2, f3),1.0),vl);
                    }
                }
                else if (collectFlag==3.0){
                   float v3 = smoothstep(-0.4,0.4,xWorld.x);
                   float v4 = smoothstep(-0.3,0.2,xWorld.y);
                   f0.x = (f0.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f0.y = (f0.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f0.z = (f0.z - (3.0)) * 2.0 / 5.0 + (-1.0);
                   f2.y = (lxWorld.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f2.z = (lxWorld.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f2.w = (lxWorld.z - (3.0)) * 2.0 / 5.0 + (-1.0); // spilt by light positon
                   if(lxWorld.x<-1.3){
                        finalColor = lerp(float4(EvaluateNetwork_Back_L_L(f0, f1, f2, f3),1.0),
                        float4(EvaluateNetwork_Back_L_R2(f0, f1, f2, f3),1.0),v3);
                   }else if (lxWorld.x<-1){
                        float tmp_lerp = smoothstep(-1.3,-1,lxWorld.x);
                        float4 left_color = lerp(float4(EvaluateNetwork_Back_L_L(f0, f1, f2, f3),1.0),
                                float4(EvaluateNetwork_Back_L_R2(f0, f1, f2, f3),1.0),v3);
                        float4 right_color = finalColor = lerp(float4(EvaluateNetwork_Back_C_D(f0, f1, f2, f3),1.0),
                                float4(EvaluateNetwork_Back_C_U(f0, f1, f2, f3),1.0),v4);
                        finalColor = lerp(left_color,right_color,tmp_lerp);
                   }
                   else if(lxWorld.x<1){
                        finalColor = lerp(float4(EvaluateNetwork_Back_C_D(f0, f1, f2, f3),1.0),
                        float4(EvaluateNetwork_Back_C_U(f0, f1, f2, f3),1.0),v4);
                   }else if(lxWorld.x<1.3){
                        float tmp_lerp = smoothstep(1,1.3,lxWorld.x);
                        float4 left_color = finalColor = lerp(float4(EvaluateNetwork_Back_C_D(f0, f1, f2, f3),1.0),
                                float4(EvaluateNetwork_Back_C_U(f0, f1, f2, f3),1.0),v4);
                        float4 right_color = lerp(float4(EvaluateNetwork_Back_R_L(f0, f1, f2, f3),1.0),
                            lerp(float4(EvaluateNetwork_Back_R_R_D(f0, f1, f2, f3),1.0),
                            float4(EvaluateNetwork_Back_R_R_U(f0, f1, f2, f3),1.0),v4),v3);
                        finalColor = lerp(left_color,right_color,tmp_lerp);
                   }
                   else{
                        finalColor = lerp(float4(EvaluateNetwork_Back_R_L(f0, f1, f2, f3),1.0),
                        lerp(float4(EvaluateNetwork_Back_R_R_D(f0, f1, f2, f3),1.0),
                        float4(EvaluateNetwork_Back_R_R_U(f0, f1, f2, f3),1.0),v4),v3);
                   }
                }
                else if (collectFlag==4.0){//right wall
                   float v3 = smoothstep(-0.4,0.4,xWorld.x);
                   float v4 = smoothstep(-0.2,0.2,xWorld.y);
                   f0.x = (f0.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f0.y = (f0.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f0.z = (f0.z - (3.0)) * 2.0 / 5.0 + (-1.0);
                   f2.y = (lxWorld.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f2.z = (lxWorld.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f2.w = (lxWorld.z - (3.0)) * 2.0 / 5.0 + (-1.0); // spilt by light positon
                   if(lxWorld.x>1.6){
                        finalColor = lerp(float4(EvaluateNetwork_Right_3_D(f0, f1, f2, f3),1.0),
                        float4(EvaluateNetwork_Right_3_U(f0, f1, f2, f3),1.0),vl);
                   }else if(lxWorld.x>1.4){
                        float tmp_lerp = smoothstep(1.4,1.6,lxWorld.x);
                        finalColor = lerp(
                            lerp(float4(EvaluateNetwork_Right_2_D(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Right_2_U(f0, f1, f2, f3),1.0),vl),
                            lerp(float4(EvaluateNetwork_Right_3_D(f0, f1, f2, f3),1.0),
                            float4(EvaluateNetwork_Right_3_U(f0, f1, f2, f3),1.0),vl),
                            tmp_lerp);
                   }else if(lxWorld.x>1.2){
                        finalColor = lerp(float4(EvaluateNetwork_Right_2_D(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Right_2_U(f0, f1, f2, f3),1.0),vl); 
                   }
                   else if(lxWorld.x>1.0){
                        float tmp_lerp = smoothstep(1.0,1.2,lxWorld.x);
                        finalColor = lerp(
                            lerp(float4(EvaluateNetwork_Right_1_D(f0, f1, f2, f3),1.0)
                            ,float4(EvaluateNetwork_Right_1_U(f0, f1, f2, f3),1.0),vl),
                            lerp(float4(EvaluateNetwork_Right_2_D(f0, f1, f2, f3),1.0)
                               ,float4(EvaluateNetwork_Right_2_U(f0, f1, f2, f3),1.0),vl),
                            tmp_lerp);
                   }else if(lxWorld.x>0.1){
                        finalColor = lerp(float4(EvaluateNetwork_Right_1_D(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Right_1_U(f0, f1, f2, f3),1.0),vl);
                   }else{
                        finalColor = lerp(float4(EvaluateNetwork_Right_0_D(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Right_0_U(f0, f1, f2, f3),1.0),vl);
                   }
                } 
                else if (collectFlag==5.0){
                   float v3 = smoothstep(-0.3,0.3,xWorld.x);
                   float v4 = smoothstep(-0.2,0.2,lxWorld.x);
                   f0.x = (f0.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f0.y = (f0.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f0.z = (f0.z - (3.0)) * 2.0 / 5.0 + (-1.0);
                   f2.y = (lxWorld.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f2.z = (lxWorld.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                   f2.w = (lxWorld.z - (3.0)) * 2.0 / 5.0 + (-1.0); // spilt by light positon
                   float4 left_color;
                   float4 right_color;
                   float4 center_color; 
                   if(lxWorld.y>1.2){
                        left_color = lerp(float4(EvaluateNetwork_Top_LT_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Top_LT_R(f0, f1, f2, f3),1.0),v3);
                        center_color = lerp(float4(EvaluateNetwork_Top_CT_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Top_CT_R(f0, f1, f2, f3),1.0),v3);
                        right_color = lerp(float4(EvaluateNetwork_Top_RT_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Top_RT_R(f0, f1, f2, f3),1.0),v3);
                   }else if(lxWorld.y>0.9){
                        float tmp_lerp = smoothstep(0.9,1.2,lxWorld.y);
                        left_color = lerp(
                             lerp(float4(EvaluateNetwork_Top_LD_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Top_LD_R(f0, f1, f2, f3),1.0),v3),
                            lerp(float4(EvaluateNetwork_Top_LT_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Top_LT_R(f0, f1, f2, f3),1.0),v3),
                            tmp_lerp);
                        center_color = lerp(
                             lerp(float4(EvaluateNetwork_Top_CD_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Top_CD_R(f0, f1, f2, f3),1.0),v3),
                            lerp(float4(EvaluateNetwork_Top_CT_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Top_CT_R(f0, f1, f2, f3),1.0),v3),
                            tmp_lerp);
                        right_color = lerp(
                             lerp(float4(EvaluateNetwork_Top_RD_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Top_RD_R(f0, f1, f2, f3),1.0),v3),
                            lerp(float4(EvaluateNetwork_Top_RT_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Top_RT_R(f0, f1, f2, f3),1.0),v3),
                            tmp_lerp);
                   }else{
                        left_color = lerp(float4(EvaluateNetwork_Top_LD_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Top_LD_R(f0, f1, f2, f3),1.0),v3);
                        center_color = lerp(float4(EvaluateNetwork_Top_CD_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Top_CD_R(f0, f1, f2, f3),1.0),v3);
                        right_color = lerp(float4(EvaluateNetwork_Top_RD_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Top_RD_R(f0, f1, f2, f3),1.0),v3);

                   }
                   if(lxWorld.x>-0.4 && lxWorld.x<0.4){
                        finalColor = center_color;
                   }else if(lxWorld.x>=-1 && lxWorld.x<=-0.4){
                        float tmp_lerp = smoothstep(-1,-0.4,lxWorld.x);
                        finalColor = lerp(left_color,center_color,tmp_lerp);
                   }else if(lxWorld.x>=0.4&&lxWorld.x<=1){
                        float tmp_lerp = smoothstep(0.4,1,lxWorld.x);
                        finalColor = lerp(center_color,right_color,tmp_lerp);
                   }else if(lxWorld.x>1){
                        finalColor = right_color;
                   }else if(lxWorld.x<-1){
                        finalColor = left_color;
                   }
                    
                }
                else if (collectFlag==6.0){
                    float v3 = smoothstep(-0.3,0.3,xWorld.x);
                    f0.x = (f0.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f0.y = (f0.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f0.z = (f0.z - (3.0)) * 2.0 / 5.0 + (-1.0);
                    f2.y = (lxWorld.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f2.z = (lxWorld.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f2.w = (lxWorld.z - (3.0)) * 2.0 / 5.0 + (-1.0); // spilt by light positon
                    if(lxWorld.x>1.3){
                        finalColor = lerp(float4(EvaluateNetwork_Bottom_R_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Bottom_R_R(f0, f1, f2, f3),1.0),v3);
                    }else if(lxWorld.x>1){
                        float tmp_lerp = smoothstep(1,1.3,lxWorld.x);
                        finalColor = lerp(
                            lerp(float4(EvaluateNetwork_Bottom_C_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Bottom_C_R(f0, f1, f2, f3),1.0),v3)
                            ,lerp(float4(EvaluateNetwork_Bottom_R_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Bottom_R_R(f0, f1, f2, f3),1.0),v3),
                        tmp_lerp);
                    }else if(lxWorld.x>-1){
                        finalColor = lerp(float4(EvaluateNetwork_Bottom_C_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Bottom_C_R(f0, f1, f2, f3),1.0),v3);
                    }else if(lxWorld.x>-1.3){
                        float tmp_lerp = smoothstep(-1.3,-1,lxWorld.x);
                        finalColor = lerp(
                            lerp(float4(EvaluateNetwork_Bottom_L_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Bottom_L_R(f0, f1, f2, f3),1.0),v3)
                            ,lerp(float4(EvaluateNetwork_Bottom_C_L(f0, f1, f2, f3),1.0)
                                ,float4(EvaluateNetwork_Bottom_C_R(f0, f1, f2, f3),1.0),v3),
                        tmp_lerp);
                    }else{
                        finalColor = lerp(float4(EvaluateNetwork_Bottom_L_L(f0, f1, f2, f3),1.0)
                        ,float4(EvaluateNetwork_Bottom_L_R(f0, f1, f2, f3),1.0),v3);
                    }
                    
                }
                else if (collectFlag == 7.0){
                    f0.x = (f0.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f0.y = (f0.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f0.z = (f0.z - (3.0)) * 2.0 / 5.0 + (-1.0);
                    f2.y = (lxWorld.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f2.z = (lxWorld.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f2.w = (lxWorld.z - (3.0)) * 2.0 / 5.0 + (-1.0); // spilt by light positon
                    finalColor = float4(EvaluateNetwork_Glossy2(f0, f1, f2, f3),1.0);
                }else if(collectFlag == 7.1){
                    f0.x = (f0.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f0.y = (f0.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f0.z = (f0.z - (3.0)) * 2.0 / 5.0 + (-1.0);
                    f2.y = (lxWorld.x - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f2.z = (lxWorld.y - (-2.0)) * 2.0 / 4.0 + (-1.0);
                    f2.w = (lxWorld.z - (3.0)) * 2.0 / 5.0 + (-1.0);
                    finalColor = lerp(float4(EvaluateNetwork_GEO_NL(f0, f1, f2, f3),1.0)
                    ,float4(EvaluateNetwork_GEO_NR(f0, f1, f2, f3),1.0),nWorld.x<0?1:0);
                    
                }
                else{
                    finalColor = albedo;
                }

                
                //UNITY_APPLY_FOG(i.fogCoord, finalColor);
	            //finalColor = albedo;
                //return float4(pointLightPosition.xyz,1.0);
                //float4 direct_light = basecolor * 0.5;
                return finalColor * float4(attenColor,1.0);
            }
            ENDCG
        }
    }

FallBack"Legacy Shaders/Diffuse"
}