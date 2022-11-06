Shader "Ree/ClearShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScreenRect ("ScreenRect", Vector) = (0, 0, 1, 1)
    }
    
    SubShader
    {
        Tags { "PreviewType"="Plane" }
        
        Cull Off
        ZTest Off
        ZWrite On
        Blend One Zero
        ColorMask RGB

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _ScreenRect;

            v2f vert (appdata v)
            {
                const float2 uv = float2(
                    (v.uv.x * _ScreenRect.z) + _ScreenRect.x,
                    (v.uv.y * _ScreenRect.w) + _ScreenRect.y
                );
                
                v2f o;
                o.vertex = float4(v.vertex.x, -v.vertex.y, 1e-6, 1);
                o.uv = uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
