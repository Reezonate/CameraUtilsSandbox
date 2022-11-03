Shader "BeatLeader/Monke"
{
    Properties
    {
        _ReflectionColor("Reflection Color", Color) = (1,1,1,1)
        _Cube("Reflection Cubemap", Cube) = "_Skybox" {}
        _MipLevel("Reflecion Mip Level", Range(0, 9)) = 0
    }

    Category
    {
        Tags{ 
            "RenderType" = "Opaque"
        }

        SubShader
        {
            Pass
            {
                Cull Back
                ZWrite On

                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float4 normal : NORMAL;
                    float4 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float4 uv: TEXCOORD0;
                    float4 worldPos: TEXCOORD1;
                    float4 I : TEXCOORD2;
                };

                uniform samplerCUBE _Cube;
                float4 _ReflectionColor;
                float _MipLevel;
    
                v2f vert(appdata v)
                {
                    v2f o;

                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;

                    const float3 view_dir = WorldSpaceViewDir(v.vertex);
                    const float3 world_normal = UnityObjectToWorldNormal(v.normal);
                    o.I = float4(reflect(-view_dir, world_normal), _MipLevel);
                    
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    
                    return o;
                }
    
                fixed4 frag(v2f i) : SV_Target
                {
                    float4 col = texCUBElod(_Cube, i.I) * _ReflectionColor;
                    col.a = 0;
                    return col;
                }
    
                ENDCG
            }
        }
    }

    FallBack Off
}
