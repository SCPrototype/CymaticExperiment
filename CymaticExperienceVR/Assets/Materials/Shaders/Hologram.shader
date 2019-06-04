Shader "Custom/Hologram" {
	Properties {
		_HologramTex ("Hologram Texture", 2D) = "white" {}

		_Color("Alpha Color", Color) = (1,1,1,1)

		_LineColor("Line Color", Color) = (1,1,1,1)
		_BackgroundColor("Background Color", Color) = (0,0,0,1)
		_LineDirection("Move Direction", Vector) = (0,1,1,0)
		_LineSpeed("Move Speed", Range(0.1, 10)) = 1
	}
	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off Cull Back ZWrite On
		Blend SrcAlpha DstAlpha

		Pass {
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

				float sinX = sin(_LineSpeed * _Time);
				float cosX = cos(_LineSpeed * _Time);
				float sinY = sin(_LineSpeed * _Time);
				float2x2 rotationMatrix = float2x2(cosX, sinX, -sinY, cosX);
				o.modelPosition = mul(v.uv, rotationMatrix);
				
				// Transform the point to clip space:
				//o.vertex = mul(UNITY_MATRIX_MVP,v.vertex); //mul = multiplication
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = v.vertex;
				o.uv = mul(UNITY_MATRIX_P, v.uv);
				return o;
			}

			fixed4 myFragmentShader(vertexToFragment i) : SV_Target{

				float4 hologramTex = tex2D(_HologramTex, i.modelPosition/4);

				float4 output = _BackgroundColor;
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
