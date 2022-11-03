Shader "Ree/InvertPostProcess"
{
	Properties
	{
		
	}
	
	SubShader
	{
		Tags { 
			"RenderType"="Transparent"
			"Queue"="Overlay+1"
			"PreviewType"="Plane"
		}
		
		Cull Off 
		ZWrite Off 
		ZTest Always
		ColorMask RGB
		
		GrabPass {
			"_GrabTex"
		}

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = float4(v.vertex.x, -v.vertex.y, 0, 1);
				o.uv = TransformStereoScreenSpaceTex(v.uv, float4(1, 1, 0, 0));
				return o;
			}
			
			sampler2D _GrabTex;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_GrabTex, i.uv);
				col = pow(col, 1/2.2); // Linear -> Gamma
				col = 1 - col;         // Invert in Gamma
				col = pow(col, 2.2);   // Gamma -> Linear
				return col;
			}
			ENDCG
		}
	}
}