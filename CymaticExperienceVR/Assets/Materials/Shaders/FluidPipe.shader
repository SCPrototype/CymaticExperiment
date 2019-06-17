Shader "Custom/FluidPipe" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalTex("Normal", 2D) = "white" {}
		_OcclusionTex("Occlusion", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_GlowColor("Glow Color", Color) = (1,1,1,1)
		_GlowTraceColor("Glow Trace Color", Color) = (0,0,0,1)
		_GlowTex("Glow", 2D) = "white" {}
		_GlowInterval("Glow Interval", float) = 1.5
		_GlowDirection("Glow Direction", Vector) = (0,1,1,0)
		_GlowSpeed("Glow Speed", Range(0.1, 10)) = 1
		_GlowLength("Glow Length", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		//ZWrite On

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _OcclusionTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
			float2 uv_OcclusionTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 occlusion = tex2D(_OcclusionTex, IN.uv_MainTex);
			o.Albedo = albedo.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
			o.Occlusion = occlusion.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = albedo.a;
		}
		ENDCG

		Pass
		{
			//ZWrite On
			BlendOp Add
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back

			CGPROGRAM
			#pragma vertex myVertexShader
			#pragma fragment myFragmentShader
			#define UNITY_SHADER_NO_UPGRADE 1

			float4 _GlowColor;
			float4 _GlowTraceColor;
			sampler2D _GlowTex;
			float _GlowInterval;
			float _GlowLength;
			float _GlowSpeed;
			float4 _GlowDirection;

			struct vertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertexToFragment {
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			vertexToFragment myVertexShader(vertexInput v) {
				vertexToFragment o;
				// Transform the point to clip space:
				//o.vertex = mul(UNITY_MATRIX_MVP,v.vertex); //mul = multiplication
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = v.vertex;
				o.uv = mul(UNITY_MATRIX_P, v.vertex);
				return o;
			}

			fixed4 myFragmentShader(vertexToFragment i) : SV_Target{

				float interval = _GlowSpeed * _GlowInterval;
				float timeScale = ((_Time.y * _GlowSpeed) % interval) / interval;
				float t = (1 - timeScale) * (1 / _GlowLength);

				float4 glowTex = tex2D(_GlowTex, i.uv);

				float4 output = _GlowColor;

				//if (t > 0.5)
				//{
					if (glowTex.a * (1 / _GlowLength) < t || glowTex.a == 0)
					{
						output *= 0;
					}
					else if (t < (1 / _GlowLength))
					{
						t = t;
						if (glowTex.a > t || glowTex.a == 0) //TODO: Fix the ending which is too fast.
						{
							output *= 0;
						}
					}
				//}
				//else {
	
				//}


				return output;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
