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
		BlendOp Add
		Blend One Zero
		
		GrabPass {
			"_GrabTex"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata v)
			{
				v2f o;
				
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				o.vertex = float4(v.vertex.x, -v.vertex.y, 0, 1);
				o.uv = v.uv;
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