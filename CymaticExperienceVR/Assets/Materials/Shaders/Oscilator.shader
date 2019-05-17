Shader "Custom/Oscilator"
{
	Properties
	{
		_PanelColor("Panel Color", Color) = (1,1,1,1)
		_LineColor("Line Color", Color) = (1,1,1,1)
		_LineWidth("Width", Float) = 0.1
		_LineOffsetY("Line Offset Y", Float) = 0.0

		_WaveOffsetX("Wave Offset X", Float) = 0.0

		_Frequency("Frequency", Float) = 0.0
		_Amplitude("Amplitude", Float) = 0.0
    }
		SubShader{
				Tags { "RenderType" = "Opaque" }
				Cull Off

				CGPROGRAM
				#pragma surface surf Lambert vertex:vert
				#include <UnityCG.cginc>

				float4 _PanelColor;
				float4 _LineColor;
				float _LineWidth;
				float _LineOffsetY;

				float _WaveOffsetX;

				float _Frequency;
				float _Amplitude;

				struct Input {
					float2 uv_MainTex;
					float4 color : COLOR;
				};

				void vert(inout appdata_full v) {

					float freqOffset = sin((_WaveOffsetX + v.vertex.x) * _Frequency) * _Amplitude;

					if ((v.vertex.z + _LineOffsetY) < freqOffset + _LineWidth && (v.vertex.z + _LineOffsetY) > freqOffset - _LineWidth)
					{
						v.color = _LineColor;
					}
					else {
						//lerp by factor
						v.color = _PanelColor;
					}
				}

				void surf(Input IN, inout SurfaceOutput o) {
					o.Albedo = IN.color;
				}

				ENDCG
	}
		FallBack "Diffuse"
}