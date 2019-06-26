Shader "Custom/Water" {
	Properties{
		_FoamTex("Foam Texture", 2D) = "white" {}
		_WaterColor("Water Color", Color) = (0.463, 0.35, 0.247, 1)
		_WaterFoamColor("Foam Color", Color) = (0.435, 0.33, 0.196, 1)
		_WaveHeight("Wave Heigth", float) = 0.1
		_WaveInterval("Wave Interval", Range(1, 10)) = 3
		_WaveBuildUp("Wave Build-Up", Range(0.05, 0.95)) = 0.1
		_WaveLength("Wave Length", Range(0.05, 0.95)) = 1
		_WaveSpeed("Wave Speed", Range(0.01, 1)) = 1
		_WaveDirection("Wave Direction", Vector) = (0,1,1,0)
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

			sampler2D _FoamTex;
			float4 _WaterColor;
			float4 _WaterFoamColor;
			float _WaveHeight;
			float _WaveInterval;
			float _WaveBuildUp;
			float _WaveLength;
			float _WaveSpeed;
			float4 _WaveDirection;

			struct vertexInput {
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct vertexToFragment {
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 modelPosition : TEXCOORD1;
			};

			vertexToFragment myVertexShader(vertexInput v) {
				float4 direction = normalize(_WaveDirection);
				float interval = _WaveSpeed * _WaveInterval;//2 = 0.5 * 4
				float timeScale = (((v.uv.x * -direction.x) + (v.uv.y * -direction.y) + (v.uv.z * -direction.z) + (_Time.y + 1000) * _WaveSpeed) % interval) / interval; //11 * 0.5 = 5.5 % 2 = 1.5
				float t = saturate(1 - timeScale * (1 / _WaveLength)); // 1 - 1.5 * (1 / 0.6) = 1 - 1.5 * 1.66 = 1 - 2.49 = -1.49 = 0
				float buildUp = saturate(1 - timeScale); // 1 - 1 = 0
				if (buildUp <= _WaveBuildUp)
				{
					t = 1 - (buildUp / _WaveBuildUp); // 1 - (0 / 0.2) = 1 - 0 = 1
				}
				v.vertex.y += t * _WaveHeight;
				vertexToFragment o;
				// Transform the point to clip space:
				o.vertex = mul(UNITY_MATRIX_MVP,v.vertex); //mul = multiplication
				o.modelPosition = mul(UNITY_MATRIX_P, v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 myFragmentShader(vertexToFragment i) : SV_Target{
				float4 direction = normalize(_WaveDirection);
				float interval = _WaveSpeed * _WaveInterval; //0.2 * 4 = 0.8
				float timeScale = (((i.uv.x * -direction.x) + (i.uv.y * -direction.y) + (i.uv.z * -direction.z) + _Time.y * _WaveSpeed) % interval) / interval; //2 * 0.2 % 0.8
				float alpha = saturate(1 - timeScale * (1 / _WaveLength));
				float buildUp = saturate(1 - timeScale);
				if (buildUp <= _WaveBuildUp)
				{
					alpha = 1 - (buildUp / _WaveBuildUp);
				}
				fixed4 col = tex2D(_FoamTex, i.uv);

				float4 output = _WaterColor + (col * (_WaterFoamColor * (1 - alpha)));
				// coordinate-wise multiplication!:
				return output;	// cast down to fixed4 + GPU automatically clamps color components between 0 and 1!
			}
			ENDCG
		}
	}
}
