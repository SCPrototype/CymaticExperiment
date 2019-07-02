// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Global-Mapper" {
     Properties {
		_SandTex("Sand Texture", 2D) = "white" {}
		_SandMetallicTex("Sand Metallic Texture", 2D) = "white" {}
		_SandNormalTex("Sand Normal Texture", 2D) = "white" {}
		_SandHeightTex("Sand Height Texture", 2D) = "white" {}
		_SandOcclusionTex("Sand Occlusion Texture", 2D) = "white" {}
		_SandRoughnessTex("Sand Roughness Texture", 2D) = "white" {}

		_GrassTex("Grass Texture", 2D) = "white" {}
		_GrassMetallicTex("Grass Metallic Texture", 2D) = "white" {}
		_GrassNormalTex("Grass Normal Texture", 2D) = "white" {}
		_GrassHeightTex("Grass Height Texture", 2D) = "white" {}
		_GrassOcclusionTex("Grass Occlusion Texture", 2D) = "white" {}
		_GrassRoughnessTex("Grass Roughness Texture", 2D) = "white" {}

		_RockTex("Rock Texture", 2D) = "white" {}
		_RockMetallicTex("Rock Metallic Texture", 2D) = "white" {}
		_RockNormalTex("Rock Normal Texture", 2D) = "white" {}
		_RockHeightTex("Rock Height Texture", 2D) = "white" {}
		_RockOcclusionTex("Rock Occlusion Texture", 2D) = "white" {}
		_RockRoughnessTex("Rock Roughness Texture", 2D) = "white" {}

		_SnowTex("Snow Texture", 2D) = "white" {}
		_SnowMetallicTex("Snow Metallic Texture", 2D) = "white" {}
		_SnowNormalTex("Snow Normal Texture", 2D) = "white" {}
		_SnowHeightTex("Snow Height Texture", 2D) = "white" {}
		_SnowOcclusionTex("Snow Occlusion Texture", 2D) = "white" {}
		_SnowRoughnessTex("Snow Roughness Texture", 2D) = "white" {}

		_HeightTexScale("Height scale", Range(0, 0.08)) = 0.02

        _CenterHeight ("Center Height", Float) = 0.0

        _HighColor ("High Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_HighHeight("High Height", Float) = 0.0
		_MaxVarianceHigh("Maximum Variance High", Float) = 3.0
		_MediumColor ("Medium Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MediumHeight("Medium Height", Float) = 0.0
		_MaxVarianceMedium("Maximum Variance Medium", Float) = 3.0
        _LowColor ("Low Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_LowHeight("Low Height", Float) = 0.0
		_MaxVarianceLow("Maximum Variance Low", Float) = 3.0
		_BottomColor("Bottom Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BottomHeight("Bottom Height", Float) = 0.0
     }
     SubShader {
             Tags { "RenderType"="Opaque" }
             Cull Off
             
             CGPROGRAM
			 #pragma target 3.0
             #pragma surface surf Standard vertex:vert
             #include <UnityCG.cginc>
     
             float _CenterHeight;
             float4 _HighColor;
			 float _HighHeight;
			 float _MaxVarianceHigh;
			 float4 _MediumColor;
			 float _MediumHeight;
			 float _MaxVarianceMedium;
             float4 _LowColor;
			 float _LowHeight;
			 float _MaxVarianceLow;
			 float4 _BottomColor;
			 float _BottomHeight;

			 float _HeightTexScale;

			 sampler2D _SandTex;
			 //sampler2D _SandMetallicTex;
			 //sampler2D _SandNormalTex;
			 sampler2D _SandRoughnessTex;

			 sampler2D _GrassTex;
			 sampler2D _GrassMetallicTex;
			 sampler2D _GrassNormalTex;
			 sampler2D _GrassRoughnessTex;

			 sampler2D _RockTex;
			 sampler2D _RockMetallicTex;
			 sampler2D _RockNormalTex;
			 sampler2D _RockRoughnessTex;

			 sampler2D _SnowTex;
			 sampler2D _SnowMetallicTex;
			 sampler2D _SnowNormalTex;
			 sampler2D _SnowRoughnessTex;


             struct Input{
                 float2 uv_MainTex;
				 float2 uv_NormalTex;
				 float2 uv_OcclusionTex;
				 half3 viewDir;
                 float4 color : COLOR;
				 float pDiff;
             };
             
             void vert(inout appdata_full v, out Input o) {
				 UNITY_INITIALIZE_OUTPUT(Input, o);
                 // Convert to world position
                 //float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                 float diff = v.vertex.y - _CenterHeight;
				 o.pDiff = diff;

				 float cFactor = saturate(diff / (2 * _MaxVarianceHigh) + 0.5);

				 if (diff >= _HighHeight) //If the vertex is above the HighHeight.
				 {
					 //cFactor = saturate((diff - _HighHeight) / (2 * _MaxVarianceHigh) + 0.5);
					 v.color = _HighColor; //Make the vertex the HighColor.
				 } 
				 else if (diff >= _MediumHeight) //If the vertex is above the middle point between medium and high.
				 {
					 cFactor = saturate((diff - (_MediumHeight + ((_HighHeight - _MediumHeight) / 2))) / (2 * _MaxVarianceHigh) + 0.5);
					 v.color = lerp(_HighColor, _MediumColor, cFactor); //Lerp the color between medium and high.
				 }
				 else if (diff >= _LowHeight) //If the vertex is above the middle point between low and medium.
				 {
					 cFactor = saturate((diff - (_LowHeight + ((_MediumHeight - _LowHeight) / 2))) / (2 * _MaxVarianceMedium) + 0.5);
					 v.color = lerp(_MediumColor, _LowColor, cFactor); //Lerp the color between low and medium.
				 }
				 else if (diff >= _BottomHeight) //If the vertex is above the middle point between bottom and low.
				 {
					 cFactor = saturate((diff - (_BottomHeight + ((_LowHeight - _BottomHeight) / 2))) / (2 * _MaxVarianceLow) + 0.5);
					 v.color = lerp(_LowColor, _BottomColor, cFactor); //Lerp the color between bottom and low.
				 }
				 else { //If the vertex is below all thresholds
					 v.color = _BottomColor; //Make the vertex the bottom color.
				 }
                 
                 //lerp by factor

             }
             
             void surf(Input IN, inout SurfaceOutputStandard o){
				 float diff = IN.pDiff - _CenterHeight;

				 if (diff >= _HighHeight) //If the vertex is above the HighHeight.
				 {
					 //float2 offset = ParallaxOffset(tex2D(_SnowHeightTex, IN.uv_MainTex).r, _HeightTexScale, IN.viewDir);
					 //o.Emission = tex2D(_SnowEmmisionTex, IN.uv_MainTex + offset).rgba;
					 fixed4 albedo = tex2D(_SnowTex, IN.uv_MainTex);
					 //fixed4 occlusion = tex2D(_SnowOcclusionTex, IN.uv_OcclusionTex + offset);
					 fixed4 metallic = tex2D(_SnowMetallicTex, IN.uv_MainTex);
					 fixed4 roughness = tex2D(_SnowRoughnessTex, IN.uv_MainTex);

					 o.Albedo = albedo.rgb * IN.color;
					 o.Normal = UnpackNormal(tex2D(_SnowNormalTex, IN.uv_NormalTex));
					 //o.Occlusion = occlusion.rgb;
					 o.Metallic = metallic.b;
					 o.Smoothness = -roughness.a;
					 o.Alpha = albedo.a;
				 }
				 else if (diff >= _MediumHeight) //If the vertex is above the middle point between medium and high.
				 {
					 //float2 offset = ParallaxOffset(tex2D(_RockHeightTex, IN.uv_MainTex).r, _HeightTexScale, IN.viewDir);
					 //o.Emission = tex2D(_RockEmmisionTex, IN.uv_MainTex + offset).rgba;
					 fixed4 albedo = tex2D(_RockTex, IN.uv_MainTex);
					 //fixed4 occlusion = tex2D(_RockOcclusionTex, IN.uv_OcclusionTex);
					 fixed4 metallic = tex2D(_RockMetallicTex, IN.uv_MainTex);
					 fixed4 roughness = tex2D(_RockRoughnessTex, IN.uv_MainTex);

					 o.Albedo = albedo.rgb * IN.color;
					 o.Normal = UnpackNormal(tex2D(_RockNormalTex, IN.uv_NormalTex));
					 //o.Occlusion = occlusion.rgb;
					 o.Metallic = metallic.b;
					 o.Smoothness = -roughness.a;
					 o.Alpha = albedo.a;

					 //o.Albedo = tex2D(_RockTex, IN.uv_MainTex).rgb * IN.color;
					 //v.color = lerp(_HighColor, _MediumColor, cFactor); //Lerp the color between medium and high.
				 }
				 else if (diff >= _LowHeight) //If the vertex is above the middle point between low and medium.
				 {
					 //float2 offset = ParallaxOffset(tex2D(_GrassHeightTex, IN.uv_MainTex).r, _HeightTexScale, IN.viewDir);
					 //o.Emission = tex2D(_GrassEmmisionTex, IN.uv_MainTex).rgba;
					 fixed4 albedo = tex2D(_GrassTex, IN.uv_MainTex);
					 //fixed4 occlusion = tex2D(_GrassOcclusionTex, IN.uv_OcclusionTex);
					 fixed4 metallic = tex2D(_GrassMetallicTex, IN.uv_MainTex);
					 fixed4 roughness = tex2D(_GrassRoughnessTex, IN.uv_MainTex);

					 o.Albedo = albedo.rgb * IN.color;
					 o.Normal = UnpackNormal(tex2D(_GrassNormalTex, IN.uv_NormalTex));
					 //o.Occlusion = occlusion.rgb;
					 o.Metallic = metallic.b;
					 o.Smoothness = -roughness.a;
					 o.Alpha = albedo.a;

					// o.Albedo = tex2D(_GrassTex, IN.uv_MainTex).rgb * IN.color;
					 //v.color = lerp(_MediumColor, _LowColor, cFactor); //Lerp the color between low and medium.
				 }
				 else// if (diff >= _BottomHeight) //If the vertex is above the middle point between bottom and low.
				 {
					 //float2 offset = ParallaxOffset(tex2D(_SandHeightTex, IN.uv_MainTex).r, _HeightTexScale, IN.viewDir);
					 //o.Emission = tex2D(_SandEmmisionTex, IN.uv_MainTex + offset).rgba;
					 fixed4 albedo = tex2D(_SandTex, IN.uv_MainTex);
					 //fixed4 occlusion = tex2D(_SandOcclusionTex, IN.uv_OcclusionTex);
					 fixed4 metallic = 0;//tex2D(_SandMetallicTex, IN.uv_MainTex);
					 fixed4 roughness = tex2D(_SandRoughnessTex, IN.uv_MainTex);

					 o.Albedo = albedo.rgb * IN.color;
					 //o.Normal = UnpackNormal(tex2D(_SandNormalTex, IN.uv_NormalTex));
					 //o.Occlusion = occlusion.rgb;
					 o.Metallic = metallic.b;
					 o.Smoothness = -roughness.a;
					 o.Alpha = albedo.a;

					 //o.Albedo = tex2D(_SandTex, IN.uv_MainTex).rgb * IN.color;
					 //v.color = lerp(_LowColor, _BottomColor, cFactor); //Lerp the color between bottom and low.
				 }
				 //else { //If the vertex is below all thresholds
					// o.Albedo = tex2D(_SandTex, IN.uv_MainTex).rgb * IN.color;
					////v.color = _BottomColor; //Make the vertex the bottom color.
				 //}
				 
				 //o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
             }
             
             ENDCG
     }
     FallBack "Diffuse"
 }