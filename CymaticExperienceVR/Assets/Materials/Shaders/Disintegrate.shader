Shader "Custom/Disintegrate"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
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
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DisintegrateNoise;
		fixed4 _Color;
		fixed4 _DisintegrateColor;
		half  _DisintegrateAmount;
		float _DisintegrateSize;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			half dissolve_value = tex2D(_DisintegrateNoise, IN.uv_MainTex).r; //Get how much we have to dissolve based on our dissolve texture
			clip(dissolve_value - _DisintegrateAmount); //Dissolve!
			o.Emission = _DisintegrateColor * step(dissolve_value - _DisintegrateAmount, _DisintegrateSize); //emits white color with 0.05 border size

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
