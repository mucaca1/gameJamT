Shader "Unlit/PlayerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [PerRendererData] _IsBlinking ("Blinking", Float) = 0
        _BlinkSpeed ("Blink Speed", Float) = 5
    }
    SubShader
    {
        Tags
        {
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite off
		Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _IsBlinking;
            float _BlinkSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (_IsBlinking != 0 && sin(_Time.z * _BlinkSpeed) > 0)
                    col *= float4(1, 0, 0, 1);
                else
                    col *= i.color;
                
                return col;
            }
            ENDCG
        }
    }
}
