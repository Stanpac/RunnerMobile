// Made with Amplify Shader Editor v1.9.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HeightColor"
{
	Properties
	{
		_CouleurMax("CouleurMax", Float) = 10
		_CouleurMin("CouleurMin", Float) = -10
		_Courbure("Courbure", Float) = 0.001
		_VECTOR("VECTOR", Vector) = (2,2,0,0)
		_Distance("Distance", Float) = 10
		_SideOffset("Side Offset", Float) = 0.75
		_SideCurveAttenuation("Side Curve Attenuation", Float) = 20
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _Courbure;
		uniform float _Distance;
		uniform float3 _VECTOR;
		uniform float _SideCurveAttenuation;
		uniform float _SideOffset;
		uniform float _CouleurMin;
		uniform float _CouleurMax;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float clampResult35 = clamp( ( ( ase_worldPos - _WorldSpaceCameraPos ).z + _Distance ) , 0.0 , 90000.0 );
			float3 temp_output_26_0 = ( ( _Courbure * pow( clampResult35 , 2.0 ) ) * _VECTOR );
			float clampResult41 = clamp( ( ( abs( ( ase_worldPos.x - float3( 0,0,0 ).x ) ) / _SideCurveAttenuation ) - _SideOffset ) , 0.0 , 5000.0 );
			float3 appendResult46 = (float3(0.0 , pow( clampResult41 , 2.0 ) , 0.0));
			float3 worldToObj28 = mul( unity_WorldToObject, float4( ( ( ase_worldPos + temp_output_26_0 ) - appendResult46 ), 1 ) ).xyz;
			v.vertex.xyz = worldToObj28;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			Gradient gradient3 = NewGradient( 0, 3, 2, float4( 0.2745098, 0.1384442, 0.03137255, 0 ), float4( 0.5843138, 0.4292093, 0.2235294, 0.2500038 ), float4( 0.6705883, 0.6095415, 0.2941177, 0.7499962 ), 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float3 ase_worldPos = i.worldPos;
			float clampResult35 = clamp( ( ( ase_worldPos - _WorldSpaceCameraPos ).z + _Distance ) , 0.0 , 90000.0 );
			float3 temp_output_26_0 = ( ( _Courbure * pow( clampResult35 , 2.0 ) ) * _VECTOR );
			o.Albedo = SampleGradient( gradient3, (0.0 + (( ase_worldPos.y - temp_output_26_0.y ) - _CouleurMin) * (1.0 - 0.0) / (_CouleurMax - _CouleurMin)) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19100
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;HeightColor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.GradientSampleNode;4;-397.2696,-267.881;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;2;-598.5859,-108.4098;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-832.0366,38.54506;Inherit;False;Property;_CouleurMax;CouleurMax;0;0;Create;True;0;0;0;False;0;False;10;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-838.0366,-65.45495;Inherit;False;Property;_CouleurMin;CouleurMin;1;0;Create;True;0;0;0;False;0;False;-10;-4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;3;-634.0699,-495.481;Inherit;False;0;3;2;0.2745098,0.1384442,0.03137255,0;0.5843138,0.4292093,0.2235294,0.2500038;0.6705883,0.6095415,0.2941177,0.7499962;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;19;-243.7493,88.76366;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;24;-1113.884,498.9304;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-892.779,360.2944;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-751.376,360.707;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-569.5459,286.8948;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TransformPositionNode;28;-322.845,285.5647;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;29;-1094.778,355.1579;Inherit;False;Property;_Courbure;Courbure;2;0;Create;True;0;0;0;False;0;False;0.001;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;30;-916.304,458.5422;Inherit;False;Property;_VECTOR;VECTOR;3;0;Create;True;0;0;0;False;0;False;2,2,0;2,2,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;31;-995.6759,-53.40469;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-1072.554,-310.2596;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;32;-834.501,-180.4234;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-1728.653,362.3805;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;21;-1553.653,362.3805;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WorldPosInputsNode;22;-2128.654,303.3805;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;23;-2143.068,483.7625;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1383.566,490.6664;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;35;-1263.848,486.6909;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;90000;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-370.662,544.7026;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1547.601,608.6664;Inherit;False;Property;_Distance;Distance;4;0;Create;True;0;0;0;False;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;36;-1870.802,919.1155;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-1717.466,839.5512;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;38;-1929.867,756.1744;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;39;-1569.474,850.2449;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;43;-1433.162,851.8828;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1679.915,715.0174;Inherit;False;Property;_SideCurveAttenuation;Side Curve Attenuation;6;0;Create;True;0;0;0;False;0;False;20;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;46;-672.5515,620.8546;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;45;-884.5104,744.5822;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;41;-1077.825,796.0577;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;5000;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;-1261.62,833.3666;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1461.832,958.2703;Inherit;False;Property;_SideOffset;Side Offset;5;0;Create;True;0;0;0;False;0;False;0.75;0.8;0;0;0;1;FLOAT;0
WireConnection;0;0;4;0
WireConnection;0;11;28;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;2;0;32;0
WireConnection;2;1;6;0
WireConnection;2;2;7;0
WireConnection;24;0;35;0
WireConnection;25;0;29;0
WireConnection;25;1;24;0
WireConnection;26;0;25;0
WireConnection;26;1;30;0
WireConnection;27;0;22;0
WireConnection;27;1;26;0
WireConnection;28;0;47;0
WireConnection;31;0;26;0
WireConnection;32;0;5;2
WireConnection;32;1;31;1
WireConnection;20;0;22;0
WireConnection;20;1;23;0
WireConnection;21;0;20;0
WireConnection;33;0;21;2
WireConnection;33;1;34;0
WireConnection;35;0;33;0
WireConnection;47;0;27;0
WireConnection;47;1;46;0
WireConnection;37;0;38;1
WireConnection;37;1;36;0
WireConnection;39;0;37;0
WireConnection;43;0;39;0
WireConnection;43;1;44;0
WireConnection;46;1;45;0
WireConnection;45;0;41;0
WireConnection;41;0;40;0
WireConnection;40;0;43;0
WireConnection;40;1;42;0
ASEEND*/
//CHKSM=546C2AF11AA67976B5B2C7483EDDF2B84EEEEE76