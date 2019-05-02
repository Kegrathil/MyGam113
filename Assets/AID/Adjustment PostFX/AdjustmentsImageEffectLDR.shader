// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AdjustmentsImageEffectLDR"
{
    Properties
	{
        _MainTex ("Texture", 2D) = "white" {}

		//input range is known to be 0-1 as we should happen as LDR after tone mapping
		[Toggle(ENABLE_LEVELS)] _Enable_Levels ("Enable Levels?", Float) = 0
		_Min ("Min", Range(0,1)) = 0.0
		_Max ("Max", Range(0,1)) = 1.0
		_MidToneGamma ("MidToneGamma", Range(0,1)) = 1.0
		
		[Toggle(ENABLE_VIBRANCE)] _Enable_Vibrance ("Enable Vibrance?", Float) = 0
		_Vibrance ("Vibrance", Range(0,1)) = 1.0
		
		[Toggle(ENABLE_HSV)] _Enable_HSV ("Enable HSV?", Float) = 0
		_HSVShift ("Hue, Saturation, Value Shift", Vector) = (0.0,0.0,0.0,0.0)
		
		[Toggle(ENABLE_PHOTOFILTER)] _Enable_PhotoFilter ("Enable PhotoFilter?", Float) = 0
		_PhotoFilter ("PhotoFilter", Vector) = (0.0,0.0,0.0,0.0)
		_PhotoFilter_Luma ("Luma Preserve", Range(0,1)) = 0.0
		
		[Toggle(ENABLE_GAMMA_CORRECTON)] _Enable_Gamma_Correction ("Enable Gamma Correction?", Float) = 0
		_GammaCorrection ("Gamma Correction", Range(0,5)) = 1.0

		[Toggle(ENABLE_UNSHARP)] _Enable_Unsharp ("Enable Unsharp?", Float) = 0
		_Unsharp_Radius ("Unsharp Pixel Radius", Range(0,5)) = 0.0
		_Unsharp_Intensity ("Unsharp Intensity", Range(0,5)) = 0.0
		_Unsharp_Threshold ("Unsharp Threshold", Range(0,5)) = 0.0

		[Toggle(ENABLE_VIGNETTE)] _Enable_Vignette ("Enable Vignette?", Float) = 0
		_Vignette_Color ("Vignette Color", Vector) = (0.0,0.0,0.0,0.0)
		_Vignette_Scale ("Vignette Scale", Range(0,1)) = 0.0
		_Vignette_Squish ("Vignette Squish", Vector) = (1.0,1.0,0.0,0.0)
		_Vignette_Offset ("Vignette Offset", Range(0,1)) = 0.0
		_Vignette_Power ("Vignette Power", Range(0,1)) = 0.0
		
		[Toggle(ENABLE_NOISE)] _Enable_Noise ("Enable Noise?", Float) = 0
		_Noise_Intensity ("Noise Intensity", Range(0,5)) = 0.0
		_Noise_Scale ("Noise Scale", Range(0,5)) = 0.0
        _Noise_Texture ("noise", 2D) = "grey" {}
	}
	SubShader
	{
        // No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
            CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
			{
                float2 uv : TEXCOORD0;
				float4 screenpos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
			{
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenpos = ComputeScreenPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _Noise_Texture;
            float4 _Noise_Texture_TexelSize; 
            
			float _Enable_Levels;
            float _Min;
            float _Max;
            float _MidToneGamma;

            float _Enable_Vibrance;
            float _Vibrance;
            
			float _Enable_HSV;
            float3 _HSVShift;
            
			float _Enable_PhotoFilter;
            float4 _PhotoFilter;
            float _PhotoFilter_Luma;

			float _Enable_Gamma_Correction;
			float _GammaCorrection;

			float _Enable_Unsharp;
            float _Unsharp_Radius;
            float _Unsharp_Intensity;
            float _Unsharp_Threshold;
            
			float _Enable_Vignette;
			float4 _Vignette_Color;
			float _Vignette_Scale;
			float _Vignette_Offset;
			float _Vignette_Power;
			float2 _Vignette_Squish;

			float _Enable_Noise;
			float _Noise_Intensity;
			float _Noise_Scale;
			float2 _Noise_Offset;

            #define LUMA_COEFF float3(0.2126, 0.7152, 0.0722)

			#define EPSILON 1e-5

			float LumaOf(float3 color)
			{
                return dot(LUMA_COEFF, color);
            }

			//http://www.chilliant.com/rgb2hsv.html
			float3 HUEtoRGB(float H)
			{
                float R = abs(H * 6.0 - 3.0) - 1.0;
                float G = 2.0 - abs(H * 6.0 - 2.0);
                float B = 2.0 - abs(H * 6.0 - 4.0);
                return saturate(float3(R,G,B));
            }


			float3 HSLtoRGB(in float3 HSL)
			{
                float3 RGB = HUEtoRGB(HSL.x);
                float C = (1.0 - abs(2.0 * HSL.z - 1.0)) * HSL.y;
                return (RGB - 0.5) * C + HSL.zzz;
            }


			float3 RGBtoHCV(float3 RGB)
			{
                // Based on work by Sam Hocevar and Emil Persson
				float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0f, 2.0f/3.0f) : float4(RGB.gb, 0.00000f, -1.0f/3.0f);
                float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
                float C = Q.x - min(Q.w, Q.y);
                float H = abs((Q.w - Q.y) / (6.0f * C + EPSILON) + Q.z);
                return float3(H, C, Q.x);
            }

			float3 HSVtoRGB(in float3 HSV)
			{
				float3 RGB = HUEtoRGB(HSV.x);
				return ((RGB - 1) * HSV.y + 1) * HSV.z;
            }
			
			float3 RGBtoHSV(in float3 RGB)
			{
                float3 HCV = RGBtoHCV(RGB);
                float S = HCV.y / (HCV.z + EPSILON);
                return float3(HCV.x, S, HCV.z);
            }

			float3 RGBtoHSL(float3 RGB)
			{
                float3 HCV = RGBtoHCV(RGB);
                float L = HCV.z - HCV.y * 0.5;
                float S = HCV.y / (1.0 - abs(L * 2.0 - 1.0) + EPSILON);
                return float3(HCV.x, S, L);
            }


			float4 frag (v2f i) : SV_Target
			{
                float4 pixel = tex2D(_MainTex, i.uv);
                //float3 col = pow(pixel.rgb,2.2);
				float3 col = pixel.rgb;


				if(_Enable_Unsharp != 0)
				{
                    //kernel blur luma
					/*
					0.0625  0.125  0.0625  
					0.125   0.25    0.125
					0.0625  0.125 0.0625
					*/
					//centre
					float origLum = LumaOf(col);
                    float blurLum = origLum * 0.25f;
                    float2 offset = _MainTex_TexelSize.xy * _Unsharp_Radius;
                    //ok now start at 12 and move around clockwise
					blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(0,offset.y))) * 0.125f;
                    blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(offset.x,offset.y))) * 0.0625;
                    blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(offset.x,0))) * 0.125f;
                    blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(offset.x,-offset.y))) * 0.0625;
                    blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(0,-offset.y))) * 0.125f;
                    blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(-offset.x,-offset.y))) * 0.0625;
                    blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(-offset.x,0))) * 0.125f;
                    blurLum += LumaOf(tex2D(_MainTex, i.uv + float2(-offset.x,offset.y))) * 0.0625;
                    //diff luma
					float lumDif = origLum - blurLum;
                    //we can get rid of this compare but it doesn't seem to be a problem in testing right now and is easier to read
					lumDif = abs(lumDif) < _Unsharp_Threshold ? 0.0f : lumDif;
                    col += (lumDif * _Unsharp_Intensity).xxx;
					col = saturate(col);
                }


                if(_Enable_Levels != 0)
				{
                    //http://http.developer.nvidia.com/GPUGems/gpugems_ch22.html
					col = pow( (col - _Min) / (_Max - _Min), _MidToneGamma);
                }

				
				if(_Enable_Vibrance != 0)
				{
                    //adapted from https://github.com/terrasque/sweetfxui/blob/master/SweetFX/SweetFX/Shaders/Vibrance.h
  					float luma = LumaOf(col);
                    float maxComp = max(col.r, max(col.g,col.b));
                    float minComp = min(col.r, min(col.g,col.b));
                    float sat = maxComp - minComp;
                    float vib = 1 + (_Vibrance * (1.0 - (sign(_Vibrance) * sat)));
                    col = lerp( luma.xxx, col, vib.xxx);
                }

				
				if(_Enable_HSV != 0)
				{
                    float3 colAsHSV = RGBtoHSV(col) + _HSVShift;
                    //sanitise values
					colAsHSV.y = clamp(colAsHSV.y,0,1);

					col = HSVtoRGB( colAsHSV );
                }


				if(_Enable_PhotoFilter != 0)
				{
                    if(_PhotoFilter_Luma != 0)
					{
                        //grab luma 
						float previousLuma = LumaOf(col);
                        col = lerp(col, col * _PhotoFilter.rgb, _PhotoFilter.a);
                        // hsl it and stamp luma
						float newLuma = LumaOf(col);
                        //col.rgb = abs(newLuma - previousLuma).xxx;
						col /= newLuma;
                        col *= previousLuma;
                    }

					else
					{
                        col = lerp(col, col * _PhotoFilter.rgb, _PhotoFilter.a);
                    }

				}

				if(_Enable_Gamma_Correction != 0)
				{
					col = pow(col, 1 / 2.2f);
					col = pow(col, _GammaCorrection * 2.2f);
				}
				
				if(_Enable_Noise)
				{
					float2 noiseUV = i.uv * (_ScreenParams.xy / _Noise_Texture_TexelSize.zw);
					float4 noiseSample = tex2D(_Noise_Texture, noiseUV * _Noise_Scale + _Noise_Offset);

					col.rgb += (noiseSample.aaa - 0.5f) * _Noise_Intensity;
				}
								
				if(_Enable_Vignette != 0)
				{
					//float l = LumaOf(col);
					float amt = (length( ( i.uv - float2(0.5,0.5) ) * _Vignette_Squish ) * 2);
					amt *= _Vignette_Scale;
					amt = 1-amt;
					amt += _Vignette_Offset;
					amt = saturate(amt);
					amt = pow(amt, _Vignette_Power);
					amt +=1;
					amt -= _Vignette_Color.a;
					
					col = col * amt;// + (1-amt) * _Vignette_Color;
					//col = amt;
					//col = abs(l - LumaOf(col))*100;
				}

				return float4(col, pixel.a);
            }

			ENDCG
		}
	}
}
