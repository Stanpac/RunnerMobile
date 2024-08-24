// Base found on https://lindenreid.wordpress.com/2018/01/07/waving-grass-shader-in-unity/
// Had to change many details to make it work. This asset should help people to save their time
// for debugging and experimenting. I also added some improvements like using an emission color.
// This needs a ramp and a gradient texture to work.
Shader "Custom/GrassWind" {
	Properties {
		_RampTex("Ramp", 2D) = "white" {} // Horizontal ramps
		_Color("Color", Color) = (1, 1, 1, 1)
        _WaveSpeed("Wave Speed", float) = 7.0
        _WaveAmp("Wave Amp", float) = 1.2
        _HeightFactor("Height Factor", float) = 1.5
		_HeightCutoff("Height Cutoff", float) = 0.1
        _WindTex("Wind Texture", 2D) = "white" {} // Horizontal repeatable gray scale gradient
        _WorldSize("World Size", vector) = (40, 40, 0, 0)   // Use only x and y (y is z in 3D space)
        _WindSpeed("Wind Speed", vector) = (1.5, 1.5, 0, 0) // Use only x and y
        _YOffset("Y offset", float) = 0.0 // y offset, below this is no animation
        _MaxWidth("Max Displacement Width", Range(0, 2)) = 0.1 // width of the line around the dissolve
        _Radius("Radius", Range(0,5)) = 1 // width of the line around the dissolve
        _Brightness("Brightness", Range(0,20)) = 1.8 // Brightness factor
        _Emission("Emission", Color) = (0, 0, 0, 0) // Emission color
		// _DebugBuffer("Debug Buffer", RWStructuredBuffer<float4>) = null
	}

	SubShader {
		Pass {
            Tags {
                "DisableBatching" = "True"
            }

			CGPROGRAM
			#pragma vertex vert surface surf
			#pragma fragment frag
			#pragma multi_compile_fwdbase // Shadows
			// #pragma target 4.5 // Needed for debugging, can be removed
            #include "UnityCG.cginc"
			struct Input
		{
			float3 worldPos;
		};
			
			// RWStructuredBuffer<float4> debugBuffer : register(u1); // Needed for debugging, can be removed

			// Properties
			sampler2D _RampTex;
            sampler2D _WindTex;
            float4 _WindTex_ST;
			float4 _Color;
			float4 _LightColor0; // Provided by Unity
            float4 _WorldSize;
            float _WaveSpeed;
            float _WaveAmp;
            float _HeightFactor;
			float _HeightCutoff;
            float2 _WindSpeed;

			float _MaxWidth;
			float _Radius;
			float _YOffset;

			float _Brightness;
			float4 _Emission;
			
			uniform float3 _Positions[100];
			uniform float _PositionArray;

					struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}

			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
                // float2 sp : TEXCOORD0; // Test sample position by making it visible as color
			};

			vertexOutput vert(vertexInput input) {
				vertexOutput output;

				// Convert input to clip & world space
				output.pos = UnityObjectToClipPos(input.vertex);
				float4 normal4 = float4(input.normal, 0.0);
				output.normal = normalize(mul(normal4, unity_WorldToObject).xyz);

                // Get vertex world position
                float4 worldPos = mul(unity_ObjectToWorld, input.vertex);

                // Normalize position based on world size
                float2 samplePos = worldPos.xz/_WorldSize.xy;
                // Scroll sample position based on time
                samplePos += _Time.x * _WindSpeed.xy;
				samplePos = float2(fmod(samplePos.x, 1), fmod(samplePos.y, 1)).xyxy;

                // Sample wind texture
                float windSample = tex2Dlod(_WindTex, float4(samplePos, 0, 0));
                
				// output.sp = samplePos; // Test sample position by making it visible as color

                // No animation below _HeightCutoff
                float heightFactor = input.vertex.y > _HeightCutoff;
				// Make animation stronger with height
				heightFactor = heightFactor * pow(abs(input.vertex.y), _HeightFactor);

                // Apply wave animation
                // output.pos.z += (sin(_WaveSpeed*windSample)*_WaveAmp * heightFactor);
				float interactionFactor;
				if (UNITY_MATRIX_P[3][3] == 1) { // Orthographic
					output.pos.x += cos(_WaveSpeed*windSample)*_WaveAmp * heightFactor / 10;
					interactionFactor = 0.5;
				} else { // With perspective
					output.pos.x += cos(_WaveSpeed*windSample)*_WaveAmp * heightFactor;
					interactionFactor = 4;
				}

				// Interaction radius movement for every position in array
			    for (int i = 0; i < _PositionArray; i++){
					float3 dis = distance(_Positions[i], worldPos); // Distance for radius
					float3 radius = 1 - saturate(dis / _Radius); // In world radius based on objects interaction radius
					float3 sphereDisp = worldPos - _Positions[i]; // Position comparison
					sphereDisp *= radius; // Position multiplied by radius for falloff

					// Vertex movement based on falloff and clamped
					output.pos.x += /*float2(interactionFactor, 1)*/ interactionFactor * clamp(sphereDisp.x/*z*/ /** step(_YOffset, output.pos.y)*/, -_MaxWidth, _MaxWidth);
				}

				return output;
			}

			float4 frag(vertexOutput input) : COLOR {
				// Normalize light dir
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

				// Apply lighting
				float ramp = clamp(dot(input.normal, lightDir), 0.001, 1.0);
				float3 lighting = tex2D(_RampTex, float2(ramp, 0.5)).rgb;
				
                // return float4(frac(input.sp.x), 0, 0, 1); // Test sample position by making it visible as color
				
				float3 rgb = _LightColor0.rgb * _Brightness * lighting * _Color.rgb + _Emission.xyz;
				return float4(rgb, 1.0);
			}

			void surf( Input i , inout SurfaceOutputStandard o )
		{
			Gradient gradient3 = NewGradient( 0, 4, 2, float4( 0.5770614, 0.7169812, 0.2198291, 0.2500038 ), float4( 0.3798161, 0.6226415, 0.1674083, 0.3499962 ), float4( 0.0745098, 0.4039216, 0.2499914, 0.5000076 ), float4( 0.2334906, 0.5, 0.4873491, 0.9000076 ), 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float3 ase_worldPos = i.worldPos;
			float clampResult45 = clamp( ( ( ase_worldPos - _WorldSpaceCameraPos ).z + _Distance ) , 0.0 , 90000.0 );
			float3 temp_output_36_0 = ( ( _Courbure * pow( clampResult45 , 2.0 ) ) * _VECTOR );
			float2 appendResult70 = (float2(ase_worldPos.x , ase_worldPos.z));
			o.Albedo = ( SampleGradient( gradient3, (0.0 + (( ase_worldPos.y - temp_output_36_0.y ) - _CouleurMin) * (1.0 - 0.0) / (_CouleurMax - _CouleurMin)) ) * ( 1.0 - ( tex2D( _TextureSample0, (appendResult70*_SCALE + 0.0) ) * _Strenght ) ) ).rgb;
			o.Alpha = 1;
		}

			ENDCG
		}

	}
}
