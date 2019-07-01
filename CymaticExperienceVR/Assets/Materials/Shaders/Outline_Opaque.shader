Shader "Outlined/Opaque"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalTex("Normal", 2D) = "white" {}
		_OcclusionTex("Occlusion", 2D) = "white" {}
		_MetallicSmoothnessTex("Metallic-Smoothness", 2D) = "black" {}
		_HeightTex("Height", 2D) = "white" {}
		_HeightTexScale("Height scale", Range(0, 0.08)) = 0.02
		_EmmisionTex("Emmision", 2D) = "black" {}

		_OutlineColor("Outline color", Color) = (0,0,0,1)
		_OutlineWidth("Outlines width", Range(0.0, 2.0)) = 1.1

		[Toggle] _UseNormal("Use Normal", Float) = 1
	}

		CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float4 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : POSITION;
		};

		uniform float _OutlineWidth;
		uniform float4 _OutlineColor;
		uniform sampler2D _MainTex;
		uniform float4 _Color;
		sampler2D _NormalTex;
		sampler2D _OcclusionTex;
		sampler2D _MetallicSmoothnessTex;
		sampler2D _HeightTex;
		float _HeightTexScale;
		sampler2D _EmmisionTex;
		bool _UseNormal;

		ENDCG

		SubShader
		{
			Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" "IgnoreProjector" = "True" }

			Pass //Outline
			{
				ZWrite Off
				Cull Front
				ZTest LEqual
				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag

				v2f vert(appdata v)
				{
					appdata original = v;
					if (_UseNormal == 0)
					{
						v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);
					}
					else {
						v.vertex.xyz += _OutlineWidth * normalize(v.normal.xyz);
					}

					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;

				}

				half4 frag(v2f i) : COLOR
				{
					return _OutlineColor;
				}

				ENDCG
			}

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows vertex:vert

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			struct Input {
				float2 uv_MainTex;
				float2 uv_NormalTex;
				float2 uv_OcclusionTex;
				half3 viewDir;
			};

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void vert(inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input, o);
				//float4 heightMap = tex2Dlod(_HeightTex, float4(v.texcoord.xy, 0, 0));
				//fixed4 heightMap = _HeightTex;
				//v.vertex.y += heightMap.b * _HeightTexScale;
			}

			void surf(Input IN, inout SurfaceOutputStandard o) {
				float2 offset = ParallaxOffset(tex2D(_HeightTex, IN.uv_MainTex).r, _HeightTexScale, IN.viewDir);

				o.Emission = tex2D(_EmmisionTex, IN.uv_MainTex + offset).rgba;
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex + offset) * _Color;
				fixed4 occlusion = tex2D(_OcclusionTex, IN.uv_OcclusionTex + offset);
				fixed4 metalSmooth = tex2D(_MetallicSmoothnessTex, IN.uv_MainTex + offset);
				o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex + offset));
				o.Occlusion = occlusion.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = metalSmooth.r;
				o.Smoothness = metalSmooth.a;

				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			ENDCG
		}
	Fallback "Diffuse"
}