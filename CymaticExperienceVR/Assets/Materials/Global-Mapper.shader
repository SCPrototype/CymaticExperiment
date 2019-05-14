// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Global-Mapper" {
     Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
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
             #pragma surface surf Lambert vertex:vert
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
             sampler2D _MainTex;
             
             struct Input{
                 float2 uv_MainTex;
                 float4 color : COLOR;
             };
             
             void vert(inout appdata_full v){
                 // Convert to world position
                 float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                 float diff = worldPos.y - _CenterHeight;
				 float cFactor = saturate(diff / (2 * _MaxVarianceHigh) + 0.5);

				 if (diff >= _HighHeight) //If the vertex is above the HighHeight.
				 {
					 //cFactor = saturate((diff - _HighHeight) / (2 * _MaxVarianceHigh) + 0.5);
					 v.color = _HighColor; //Make the vertex the HighColor.
				 } 
				 else if (diff >= _MediumHeight) //If the vertex is above the middle point between medium and high.
				 {
					 cFactor = saturate((diff - _MediumHeight) / (2 * _MaxVarianceHigh) + 0.5);
					 v.color = lerp(_HighColor, _MediumColor, cFactor); //Lerp the color between medium and high.
				 }
				 else if (diff >= _LowHeight) //If the vertex is above the middle point between low and medium.
				 {
					 cFactor = saturate((diff - _LowHeight) / (2 * _MaxVarianceMedium) + 0.5);
					 v.color = lerp(_MediumColor, _LowColor, cFactor); //Lerp the color between low and medium.
				 }
				 else if (diff >= _BottomHeight) //If the vertex is above the middle point between bottom and low.
				 {
					 cFactor = saturate((diff - _BottomHeight) / (2 * _MaxVarianceLow) + 0.5);
					 v.color = lerp(_LowColor, _BottomColor, cFactor); //Lerp the color between bottom and low.
				 }
				 else { //If the vertex is below all thresholds
					 v.color = _BottomColor; //Make the vertex the bottom color.
				 }
                 
                 //lerp by factor

             }
             
             void surf(Input IN, inout SurfaceOutput o){
                 o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
             }
             
             ENDCG
     }
     FallBack "Diffuse"
 }