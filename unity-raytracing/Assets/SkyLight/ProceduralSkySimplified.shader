Shader "Custom/ProceduralSkySimplified"
{
    Properties
    {
        _SunSize("Sun Size", Range(0,1)) = 0.04
        _SunSizeConvergence("Sun Size Convergence", Range(1,10)) = 5
        _AtmosphereThickness("Atmosphere Thickness", Range(0,5)) = 1.0
        _SkyTint("Sky Tint", Color) = (.5, .5, .5, 1)
        _GroundColor("Ground", Color) = (.369, .349, .341, 1)
        _Exposure("Exposure", Range(0, 8)) = 1.3
        _Power("Power", Range(0, 1000.0)) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off ZWrite Off

        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            uniform half _Power;
            uniform half _Exposure;     // HDR exposure
            uniform half3 _GroundColor;
            uniform half _SunSize;
            uniform half _SunSizeConvergence;
            uniform half3 _SkyTint;
            uniform half _AtmosphereThickness;

            #define GAMMA 2.2
            // HACK: to get gfx-tests in Gamma mode to agree until UNITY_ACTIVE_COLORSPACE_IS_GAMMA is working properly
            #define COLOR_2_GAMMA(color) ((unity_ColorSpaceDouble.r>2.0) ? pow(color,1.0/GAMMA) : color)


            static const float3 kDefaultScatteringWavelength = float3(.65, .57, .475);
            static const float3 kVariableRangeForScatteringWavelength = float3(.15, .15, .15);

            #define OUTER_RADIUS 1.025
            static const float kOuterRadius = OUTER_RADIUS;
            static const float kOuterRadius2 = OUTER_RADIUS * OUTER_RADIUS;
            static const float kInnerRadius = 1.0;
            static const float kInnerRadius2 = 1.0;

            static const float kCameraHeight = 0.0001;

            #define kRAYLEIGH (lerp(0.0, 0.0025, pow(_AtmosphereThickness,2.5)))      // Rayleigh constant
            #define kMIE 0.0010             // Mie constant
            #define kSUN_BRIGHTNESS 20.0    // Sun brightness

            #define kMAX_SCATTER 50.0 // Maximum scattering value, to prevent math overflows on Adrenos

            static const half kHDSundiskIntensityFactor = 15.0;
            static const half kSimpleSundiskIntensityFactor = 27.0;

            static const half kSunScale = 400.0 * kSUN_BRIGHTNESS;
            static const float kKmESun = kMIE * kSUN_BRIGHTNESS;
            static const float kKm4PI = kMIE * 4.0 * 3.14159265;
            static const float kScale = 1.0 / (OUTER_RADIUS - 1.0);
            static const float kScaleDepth = 0.25;
            static const float kScaleOverScaleDepth = (1.0 / (OUTER_RADIUS - 1.0)) / 0.25;
            static const float kSamples = 2.0; // THIS IS UNROLLED MANUALLY, DON'T TOUCH

            #define MIE_G (-0.990)
            #define MIE_G2 0.9801

            #define SKY_GROUND_THRESHOLD 0.02

            struct appdata_t
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4  pos             : SV_POSITION;
                float3  vertex          : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };


            // Calculates the Rayleigh phase function
            half getRayleighPhase(half3 light, half3 ray)
            {
                half eyeCos = dot(light, ray);
                return 0.75 + 0.75 * eyeCos * eyeCos;
            }

            float scale(float inCos)
            {
                float x = 1.0 - inCos;
                return 0.25 * exp(-0.00287 + x * (0.459 + x * (3.83 + x * (-6.80 + x * 5.25))));
            }

            // Calculates the Mie phase function
            half getMiePhase(half eyeCos, half eyeCos2)
            {
                half temp = 1.0 + MIE_G2 - 2.0 * MIE_G * eyeCos;
                temp = pow(temp, pow(_SunSize,0.65) * 10);
                temp = max(temp,1.0e-4); // prevent division by zero, esp. in half precision
                temp = 1.5 * ((1.0 - MIE_G2) / (2.0 + MIE_G2)) * (1.0 + eyeCos2) / temp;
                return temp;
            }

            // Calculates the sun shape
            half calcSunAttenuation(half3 lightPos, half3 ray)
            {
                half focusedEyeCos = pow(saturate(dot(lightPos, ray)), _SunSizeConvergence);
                return getMiePhase(-focusedEyeCos, focusedEyeCos * focusedEyeCos);
            }

            
            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                // Get the ray from the camera to the vertex and its length (which is the far point of the ray passing through the atmosphere)
                float3 eyeRay = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz));
                OUT.vertex = -eyeRay;
                OUT.pos = UnityObjectToClipPos(v.vertex);
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                float3 kSkyTintInGammaSpace = COLOR_2_GAMMA(_SkyTint); // convert tint from Linear back to Gamma
                float3 kScatteringWavelength = lerp(
                    kDefaultScatteringWavelength - kVariableRangeForScatteringWavelength,
                    kDefaultScatteringWavelength + kVariableRangeForScatteringWavelength,
                    half3(1,1,1) - kSkyTintInGammaSpace); // using Tint in sRGB gamma allows for more visually linear interpolation and to keep (.5) at (128, gray in sRGB) point
                float3 kInvWavelength = 1.0 / pow(kScatteringWavelength, 4);

                float kKrESun = kRAYLEIGH * kSUN_BRIGHTNESS;
                float kKr4PI = kRAYLEIGH * 4.0 * 3.14159265;

                float3 cameraPos = float3(0,kInnerRadius + kCameraHeight,0);    // The camera's current position

                // Get the ray from the camera to the vertex and its length (which is the far point of the ray passing through the atmosphere)
                float3 eyeRay = -IN.vertex.xyz;

                float far = 0.0;
                half3 cIn, cOut;

                if (eyeRay.y >= 0.0)
                {
                    float height = 0.0;
                    float depth = 0.0;
                    float startAngle = 0.0;
                    float startOffset = 0.0;

                    float sampleLength = 0.0;
                    float scaledLength = 0.0;
                    float3 sampleRay = float3(0.0, 0.0, 0.0);
                    float3 samplePoint = float3(0.0, 0.0, 0.0);
                    float3 frontColor = float3(0.0, 0.0, 0.0);

                    float height_loop = 0.0;
                    float depth_loop = 0.0;
                    float lightAngle_loop = 0.0;
                    float cameraAngle_loop = 0.0;
                    float scatter_loop = 0.0;
                    float3 attenuate_loop = float3(0.0, 0.0, 0.0);
                    // Sky
                    // Calculate the length of the "atmosphere"
                    far = sqrt(kOuterRadius2 + kInnerRadius2 * eyeRay.y * eyeRay.y - kInnerRadius2) - kInnerRadius * eyeRay.y;

                    // Calculate the ray's starting position, then calculate its scattering offset
                    height = kInnerRadius + kCameraHeight;
                    depth = exp(kScaleOverScaleDepth * (-kCameraHeight));
                    startAngle = dot(eyeRay, cameraPos) / height;
                    startOffset = depth * scale(startAngle);


                    // Initialize the scattering loop variables
                    sampleLength = far / kSamples;
                    scaledLength = sampleLength * kScale;
                    sampleRay = eyeRay * sampleLength;
                    samplePoint = cameraPos + sampleRay * 0.5;

                    // Now loop through the sample rays
                    frontColor = float3(0.0, 0.0, 0.0);
                    // Weird workaround: WP8 and desktop FL_9_3 do not like the for loop here
                    // (but an almost identical loop is perfectly fine in the ground calculations below)
                    // Just unrolling this manually seems to make everything fine again.
                    for(int i=0; i<int(kSamples); i++)
                    {
                        height_loop = length(samplePoint);
                        depth_loop = exp(kScaleOverScaleDepth * (kInnerRadius - height_loop));
                        lightAngle_loop = dot(_WorldSpaceLightPos0.xyz, samplePoint) / height_loop;
                        cameraAngle_loop = dot(eyeRay, samplePoint) / height_loop;
                        scatter_loop = (startOffset + depth_loop * (scale(lightAngle_loop) - scale(cameraAngle_loop)));
                        attenuate_loop = exp(-clamp(scatter_loop, 0.0, kMAX_SCATTER) * (kInvWavelength * kKr4PI + kKm4PI));

                        frontColor += attenuate_loop * (depth_loop * scaledLength);
                        samplePoint += sampleRay;
                    }
                    // Finally, scale the Mie and Rayleigh colors and set up the varying variables for the pixel shader
                    cIn = frontColor * (kInvWavelength * kKrESun);
                    cOut = frontColor * kKmESun;
                }
                else
                {
                    float3 pos = float3(0.0, 0.0, 0.0);
                    float depth = 0.0;
                    float cameraAngle = 0.0;
                    float lightAngle = 0.0;
                    float cameraScale = 0.0;
                    float lightScale = 0.0;
                    float cameraOffset = 0.0;
                    float temp = 0.0;

                    float sampleLength = 0.0;
                    float scaledLength = 0.0;
                    float3 sampleRay = float3(0.0, 0.0, 0.0);
                    float3 samplePoint = float3(0.0, 0.0, 0.0);

                    float3 frontColor = float3(0.0, 0.0, 0.0);
                    float3 attenuate = float3(0.0, 0.0, 0.0);

                    float height = 0.0;
                    float scatter = 0.0;

                    // Ground
                    far = (-kCameraHeight) / (min(-0.001, eyeRay.y));

                    pos = cameraPos + far * eyeRay;

                    // Calculate the ray's starting position, then calculate its scattering offset
                    depth = exp((-kCameraHeight) * (1.0 / kScaleDepth));
                    cameraAngle = dot(-eyeRay, pos);
                    lightAngle = dot(_WorldSpaceLightPos0.xyz, pos);
                    cameraScale = scale(cameraAngle);
                    lightScale = scale(lightAngle);
                    cameraOffset = depth * cameraScale;
                    temp = (lightScale + cameraScale);

                    // Initialize the scattering loop variables
                    sampleLength = far / kSamples;
                    scaledLength = sampleLength * kScale;
                    sampleRay = eyeRay * sampleLength;
                    samplePoint = cameraPos + sampleRay * 0.5;

                    // Now loop through the sample rays
                    frontColor = float3(0.0, 0.0, 0.0);
                
                    height = length(samplePoint);
                    depth = exp(kScaleOverScaleDepth * (kInnerRadius - height));
                    scatter = depth * temp - cameraOffset;
                    attenuate = exp(-clamp(scatter, 0.0, kMAX_SCATTER) * (kInvWavelength * kKr4PI + kKm4PI));
                    frontColor += attenuate * (depth * scaledLength);
                    samplePoint += sampleRay;
                
                    cIn = frontColor * (kInvWavelength * kKrESun + kKmESun);
                    cOut = clamp(attenuate, 0.0, 1.0);
                }

                // if we want to calculate color in vprog:
                // 1. in case of linear: multiply by _Exposure in here (even in case of lerp it will be common multiplier, so we can skip mul in fshader)
                // 2. in case of gamma and SKYBOX_COLOR_IN_TARGET_COLOR_SPACE: do sqrt right away instead of doing that in fshader

                float3 outGroundColor = _Exposure * (cIn + _GroundColor * cOut);
                float3 outSkyColor = _Exposure * (cIn * getRayleighPhase(_WorldSpaceLightPos0.xyz, -eyeRay));


                // The sun should have a stable intensity in its course in the sky. Moreover it should match the highlight of a purely specular material.
                // This matching was done using the standard shader BRDF1 on the 5/31/2017
                // Finally we want the sun to be always bright even in LDR thus the normalization of the lightColor for low intensity.
                half lightColorIntensity = clamp(length(_LightColor0.xyz), 0.25, 1);
                float3 outSunColor = kHDSundiskIntensityFactor * saturate(cOut) * _LightColor0.xyz / lightColorIntensity;

                half3 col = half3(0.0, 0.0, 0.0);
                half3 ray = normalize(IN.vertex.xyz);
                half y = ray.y / SKY_GROUND_THRESHOLD;

                // if we did precalculate color in vprog: just do lerp between them
                col = lerp(outSkyColor, outGroundColor, saturate(y));

                if (y < 0.0)
                {
                    col += outSunColor * calcSunAttenuation(_WorldSpaceLightPos0.xyz, -ray);
                }

                col *= _Power;

                return half4(col,1.0);
            }

            ENDCG
        }
    }

    Fallback Off
}