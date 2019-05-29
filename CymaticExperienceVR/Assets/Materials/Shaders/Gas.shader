Shader "Custom/Gas" {
	Properties{
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_TimeTex("Time Offset", 2D) = "white" {}
		_GasColor("Gas Color", Color) = (0.463, 0.35, 0.247, 1)
		_SubColor("Sub Color", Color) = (0, 0, 0, 0)
		_GasSpeed("Gas Speed", Range(0, 10)) = 1
	}

	SubShader 
	{
		Tags{ "RenderType" = "Transparent" }
		BlendOp Add
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back

		Pass 
		{
			CGPROGRAM
			#pragma vertex myVertexShader
			#pragma fragment myFragmentShader
			#define UNITY_SHADER_NO_UPGRADE 1

			sampler2D _NoiseTex;
			sampler2D _TimeTex;
			float4 _GasColor;
			float4 _SubColor;
			float _GasSpeed;

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

				float sinX = sin(_GasSpeed * _Time);
				float cosX = cos(_GasSpeed * _Time);
				float sinY = sin(_GasSpeed * _Time);
				float2x2 rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);
				o.modelPosition = mul(v.uv, rotationMatrix);

				o.vertex = UnityObjectToClipPos(v.vertex);
				rotationMatrix = float2x2(1, cosX, 0, 1);
				o.uv = mul(v.uv, rotationMatrix);
				return o;
			}


			fixed4 myFragmentShader(vertexToFragment i) : SV_Target{
				fixed4 noise = tex2D(_NoiseTex, i.uv);
				fixed4 time = tex2D(_TimeTex, i.modelPosition);

				float4 output = _GasColor * noise * time;
				if (output.r <= 0.03 && output.g <= 0.03 && output.b <= 0.03)
				{
					output = _SubColor;
				}
				// coordinate-wise multiplication!:
				//fixed4 c = Swirl(_NoiseTex, i.uv, 1) * _GasColor;
				return output;	// cast down to fixed4 + GPU automatically clamps color components between 0 and 1!
			}
			ENDCG
		}
	}
}
