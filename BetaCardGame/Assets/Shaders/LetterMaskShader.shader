Shader "Custom/LetterMaskShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _AlphaThreshold ("Alpha Threshold", Range(0,1)) = 0.1
        _ShrinkAmount ("Shrink Amount", Range(0, 10)) = 1.0 // Насколько уменьшить маску (в пикселях)
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" } 

        Stencil 
        {
            Ref 1
            Comp always  
            Pass replace 
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            AlphaTest Greater [_AlphaThreshold]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f 
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _AlphaThreshold;
            float _ShrinkAmount; // Насколько уменьшать маску (в пикселях)
            float4 _MainTex_TexelSize; // Специальная переменная Unity с размером текстуры (1/ширина, 1/высота)

            v2f vert (appdata_t v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Оставляем UV как есть
                return o;
            }

            fixed4 frag (v2f i) : SV_Target 
            {
                float pixelShrink = _ShrinkAmount * _MainTex_TexelSize.x; // Переводим пиксели в UV-координаты
                float2 shrinkDir = normalize(i.uv - float2(0.5, 0.5)) * pixelShrink;

                float maskValue = tex2D(_MainTex, i.uv - shrinkDir).a; // Читаем маску с отступом

                if (maskValue < _AlphaThreshold)
                {
                    discard; // Убираем пиксели по уменьшенной маске
                }

                return tex2D(_MainTex, i.uv) * _Color;
            }
            ENDCG
        }
    }
}
