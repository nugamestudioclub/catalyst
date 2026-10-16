Shader "Custom/UI/CRT_Scanlines_UI_DynamicOverlay_Fixed"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Scanline Color and Opacity", Color) = (0.0, 0.0, 0.0, 1.0)
        _ScanlineDensity ("Scanline Density", Range(10.0, 1000.0)) = 360.0
        _ScanlineIntensity ("Scanline Intensity", Range(0.0, 1.0)) = 0.25
        _ScanlineSpeed ("Scanline Speed", Range(0.0, 2.0)) = 0.08
        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ PIXELSNAP_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _ScanlineDensity;
            float _ScanlineIntensity;
            float _ScanlineSpeed;

            v2f vert(appdata v)
            {
                v2f o;

                #ifdef PIXELSNAP_ON
                    v.vertex = UnityPixelSnap(v.vertex);
                #endif

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float timeOffset = frac(_Time.y * _ScanlineSpeed);
                float yCoord = (i.uv.y - timeOffset) * _ScanlineDensity;
                float scan = 1.0 - pow(frac(yCoord), 5.0);
                float finalScan = lerp(1.0 - _ScanlineIntensity, 1.0, scan);
                float darkness = 1.0 - finalScan;
                float finalAlpha = darkness * i.color.a;

                fixed4 outputColor = fixed4(i.color.rgb, finalAlpha);
                outputColor.a *= tex2D(_MainTex, i.uv).a;
                return outputColor;
            }
            ENDCG
        }
    }
}
