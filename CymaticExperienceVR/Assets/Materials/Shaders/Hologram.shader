Shader "Custom/Hologram" {
	Properties {
		_HologramTex ("Hologram Texture", 2D) = "white" {}

		_Color("Alpha Color", Color) = (1,1,1,1)

		_LineColor("Line Color", Color) = (1,1,1,1)
		_BackgroundColor("Background Color", Color) = (0,0,0,1)
		_LineDirection("Move Direction", Vector) = (0,1,1,0)
		_LineSpeed("Move Speed", Range(0, 10)) = 1
		_LineDistance("Line Distance", Range(1, 500)) = 100
	}
	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off Cull Back
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			ColorMask 0
			ZWrite On
		}

		Pass {
			ZWrite Off
			ZTest Equal

			CGPROGRAM
			#pragma vertex myVertexShader
			#pragma fragment myFragmentShader
			#define UNITY_SHADER_NO_UPGRADE 1

			float4 _Color;

			sampler2D _HologramTex;
			float4 _LineColor;
			float4 _BackgroundColor;
			float4 _LineDirection;
			float _LineSpeed;
			float _LineDistance;

			struct vertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertexToFragment {
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0;
				float2 modelPosition : TEXCOORD1;
			};

			vertexToFragment myVertexShader(vertexInput v) {
				vertexToFragment o;

				o.modelPosition = float2((v.vertex.x / _LineDistance) + (_LineSpeed * _Time.x * _LineDirection.x), (v.vertex.z / _LineDistance) + (_LineSpeed * _Time.x * _LineDirection.z));// mul(float2(v.uv.x + _Time.x, v.uv.y), rotationMatrix);
				
				// Transform the point to clip space:
				//o.vertex = mul(UNITY_MATRIX_MVP,v.vertex); //mul = multiplication
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = v.vertex;
				o.uv = mul(UNITY_MATRIX_P, v.uv);
				return o;
			}

			fixed4 myFragmentShader(vertexToFragment i) : SV_Target{

				float4 output = _BackgroundColor;

				float4 hologramTex = tex2D(_HologramTex, i.modelPosition);
				if (hologramTex.a > 0)
				{
					output = _LineColor;
				}

				return output * _Color.a;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
