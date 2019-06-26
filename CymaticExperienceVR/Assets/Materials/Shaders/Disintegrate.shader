Shader "Custom/Disintegrate"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalTex("Normal", 2D) = "white" {}
		_OcclusionTex("Occlusion", 2D) = "white" {}
		_MetallicSmoothnessTex("Metallic-Smoothness", 2D) = "white" {}
		_HeightTex("Height", 2D) = "white" {}
		_HeightTexScale("Height scale", Range(0, 0.08)) = 0.02
		_EmmisionTex("Emmision", 2D) = "white" {}

		_DisintegrateColor("Disintegrate Color", Color) = (1,0,0,1)
		_DisintegrateNoise("Disintegrate Noise", 2D) = "white" {}

		_DisintegrateSize("Disintegrate Size", Range(0, 0.2)) = 0.05
		_DisintegrateAmount("Disintegrate Amount", Range(0,1)) = 0
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _OcclusionTex;
		sampler2D _MetallicSmoothnessTex;
		sampler2D _HeightTex;
		float _HeightTexScale;
		sampler2D _EmmisionTex;

		sampler2D _DisintegrateNoise;
		fixed4 _Color;
		fixed4 _DisintegrateColor;
		half  _DisintegrateAmount;
		float _DisintegrateSize;

        struct Input
        {
			float2 uv_MainTex;
			float2 uv_NormalTex;
			float2 uv_OcclusionTex;
			half3 viewDir;
        };

        half _Glossiness;
        half _Metallic;

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

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//half h = tex2D(_HeightTex, IN.uv_MainTex).w;
			//float2 offset = ParallaxOffset(h, _HeightTexScale, IN.viewDir);
			float2 offset = ParallaxOffset(tex2D(_HeightTex, IN.uv_MainTex).r, _HeightTexScale, IN.viewDir);

			half dissolve_value = tex2D(_DisintegrateNoise, IN.uv_MainTex + offset).r; //Get how much we have to dissolve based on our dissolve texture
			clip(dissolve_value - _DisintegrateAmount); //Dissolve!
			o.Emission = tex2D(_EmmisionTex, IN.uv_MainTex + offset).rgba;
			if (_DisintegrateAmount > 0 && (_DisintegrateColor * step(dissolve_value - _DisintegrateAmount, _DisintegrateSize)).a >= 0.05)
			{
				o.Emission = (_DisintegrateColor * step(dissolve_value - _DisintegrateAmount, _DisintegrateSize)).rgba; //emits white color with 0.05 border size
			}
			
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex + offset) * _Color;
			fixed4 occlusion = tex2D(_OcclusionTex, IN.uv_OcclusionTex + offset);
			fixed4 metalSmooth = tex2D(_MetallicSmoothnessTex, IN.uv_MainTex + offset);
			
            o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex + offset));
			o.Occlusion = occlusion.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = metalSmooth.r;
            o.Smoothness = metalSmooth.a;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
