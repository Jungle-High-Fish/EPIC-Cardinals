Shader "Planete/CustomDistortion"
{
    Properties
    {
        _RippleAlpha("Ripple Alpha", Float) = 1
        _RippleIntensity("Ripple Intensity", Float) = 1
        _Hue("Hue", Color) = (1, 1, 1, 1)
        _NormalMap("Normal Map", 2D) = "white" {}
        _Density("Soft Particles Factor", Range(0, 3)) = 1
        _RelativeRefractionIndex("Relative Refraction Index", Range(0.0, 1.0)) = 0.67
        [PowerSlider(5)]_Distance("Distance", Range(0.0, 100.0)) = 10.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass
        {
            Tags
            {
                // Specify LightMode correctly.
                "LightMode" = "UseColorTexture"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                half4 grabPos : TEXCOORD1;
                half2 samplingViewportPos : TEXCOORD2;
                half4 samplingScreenPos : TEXCOORD3;
            };

            float _RippleAlpha;
            float _RippleIntensity;
            float4 _Hue;
            sampler2D _GrabbedTexture;
            sampler2D _NormalMap;
            float _Density;
            float _RelativeRefractionIndex;
            float _Distance;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                float3 positionW = TransformObjectToWorld(IN.positionOS);
                half3 normalW = TransformObjectToWorldNormal(IN.normal);

                half3 viewDir = normalize(positionW - _WorldSpaceCameraPos.xyz);
                half3 refractDir = refract(viewDir, normalW, _RelativeRefractionIndex);
                half3 samplingPos = positionW + refractDir * _Distance;
                half4 samplingScreenPos = mul(UNITY_MATRIX_VP, half4(samplingPos, 1.0));
                OUT.samplingScreenPos = samplingScreenPos;
                OUT.samplingViewportPos = (samplingScreenPos.xy / samplingScreenPos.w) * 0.5 + 0.5;
                #if UNITY_UV_STARTS_AT_TOP
                OUT.samplingViewportPos.y = 1.0 - OUT.samplingViewportPos.y;
                #endif
                OUT.uv = IN.uv;
                OUT.color = IN.color;

                return OUT;
            }

            inline half3 UnpackNormal(half4 packednormal)
            {
                half3 normal;
                normal.xy = packednormal.wy * 2 - 1;
                normal.z = sqrt(1 - normal.x * normal.x - normal.y * normal.y);
                return normal;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half3 ripple = UnpackNormal(tex2D(_NormalMap, IN.uv));
                float4 grabPassUV = IN.samplingScreenPos;
                grabPassUV.xy = IN.samplingViewportPos;
                grabPassUV.xy += ripple.xy / ripple.z * _RippleIntensity * IN.color.a;
                half4 backgroundColor = tex2Dproj(_GrabbedTexture, grabPassUV);
                _Hue.a = _RippleAlpha;
                return backgroundColor * _Hue;
            }
            ENDHLSL
        }
    }
}
