Shader "Custom/Planet" {
	Properties{
		_SurfaceColor("Surface Color", Color) = (0.463, 0.35, 0.247, 1)
		_SurfaceTex("Surface Texture", 2D) = "white" {}
		_SurfaceNoise("Surface Noise Texture", 2D) = "white" {}
		_SurfaceColor2("2nd Surface Color", Color) = (0.463, 0.35, 0.247, 1)
		_SurfaceTex2("2nd Surface Texture", 2D) = "white" {}
		_CloudNoise("Cloud Noise Texture", 2D) = "white" {}
		_CloudColor("Cloud Color", Color) = (0.463, 0.35, 0.247, 1)
		_CloudTex("Cloud Texture", 2D) = "white" {}

		_CloudSpeed("Cloud Speed", Range(0, 10)) = 1
	}

		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			BlendOp Add
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back

			Pass
			{
				CGPROGRAM
				#pragma vertex myVertexShader
				#pragma fragment myFragmentShader
				#define UNITY_SHADER_NO_UPGRADE 1

				float4 _SurfaceColor;
				sampler2D _SurfaceTex;
				sampler2D _SurfaceNoise;
				float4 _SurfaceColor2;
				sampler2D _SurfaceTex2;
				sampler2D _CloudNoise;
				float4 _CloudColor;
				sampler2D _CloudTex;
				float _CloudSpeed;

				struct vertexInput {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct vertexToFragment {
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 modelPosition : TEXCOORD1;
				};

				vertexToFragment myVertexShader(vertexInput v) {
					vertexToFragment o;
					// Transform the point to clip space:

					float sinX = sin(_CloudSpeed * _Time);
					float cosX = cos(_CloudSpeed * _Time);
					float sinY = sin(_CloudSpeed * _Time);
					float2x2 rotationMatrix = float2x2(cosX, sinX, -sinY, cosX);
					o.modelPosition = mul(v.uv, rotationMatrix);

					//o.vertex = UnityObjectToClipPos(v.vertex);
					//rotationMatrix = float2x2(1, cosX, 0, 1);
					//o.uv = mul(v.uv, rotationMatrix);

					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex); //mul = multiplication
					//o.modelPosition = mul(UNITY_MATRIX_P, v.vertex);
					o.uv = v.uv;
					return o;
				}


				fixed4 myFragmentShader(vertexToFragment i) : SV_Target{
					fixed4 surface = tex2D(_SurfaceTex, i.uv) * _SurfaceColor;

					fixed4 surfaceNoise = tex2D(_SurfaceNoise, i.uv);
					fixed4 surface2 = tex2D(_SurfaceTex2, i.uv) * _SurfaceColor2;

					fixed4 cloudNoise = tex2D(_CloudNoise, i.modelPosition);
					fixed4 cloud = tex2D(_CloudTex, i.modelPosition) * _CloudColor;

					//fixed4 time = tex2D(_TimeTex, i.modelPosition);

					float4 output = (surface * surfaceNoise.r) + (surface2 * (1.0 - surfaceNoise.r)) + (cloud * (1.0 - cloudNoise.r));
					//if (output.r <= 0.03 && output.g <= 0.03 && output.b <= 0.03)
					//{
					//	output = _SubColor;
					//}


					// coordinate-wise multiplication!:
					//fixed4 c = Swirl(_NoiseTex, i.uv, 1) * _GasColor;
					return output;	// cast down to fixed4 + GPU automatically clamps color components between 0 and 1!
				}
				ENDCG
			}
		}
}
