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
		_Strenght("Strenght", Range( 0 , 10)) = 3
		_SCALE("SCALE", Range( 0 , 1)) = 0.05
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
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
		uniform sampler2D _TextureSample0;
		uniform float _SCALE;
		uniform float _Strenght;


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
			float clampResult45 = clamp( ( ( ase_worldPos - _WorldSpaceCameraPos ).z + _Distance ) , 0.0 , 90000.0 );
			float3 temp_output_36_0 = ( ( _Courbure * pow( clampResult45 , 2.0 ) ) * _VECTOR );
			float clampResult67 = clamp( ( ( abs( ( ase_worldPos.x - temp_output_36_0.x ) ) / _SideCurveAttenuation ) - _SideOffset ) , 0.0 , 5000.0 );
			float3 appendResult61 = (float3(0.0 , pow( clampResult67 , 2.0 ) , 0.0));
			float3 worldToObj37 = mul( unity_WorldToObject, float4( ( ( ase_worldPos + temp_output_36_0 ) - appendResult61 ), 1 ) ).xyz;
			v.vertex.xyz = worldToObj37;
			v.vertex.w = 1;
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
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19100
Node;AmplifyShaderEditor.TransformPositionNode;37;-344.1315,303.2608;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1473.177,395.1299;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1331.774,395.5425;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1675.177,389.9934;Inherit;False;Property;_Courbure;Courbure;2;0;Create;True;0;0;0;False;0;False;0.001;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;22;-1496.702,493.3777;Inherit;False;Property;_VECTOR;VECTOR;3;0;Create;True;0;0;0;False;0;False;2,2,0;2,2,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-2383.283,414.7658;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;34;-2797.698,536.1475;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-2225.283,412.7658;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;43;-2222.602,649.0881;Inherit;False;Property;_Distance;Distance;4;0;Create;True;0;0;0;False;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-2026.602,531.0881;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;45;-1877.603,535.0881;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;90000;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;10;-2783.285,355.7658;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;14;-1694.283,533.7656;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;58;-1932.852,136.2074;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;60;-1779.516,56.64315;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;59;-1991.916,-26.73367;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;50;-1631.524,67.33687;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;66;-1367.261,113.2285;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;67;-1239.261,128.2284;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;5000;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1621.524,182.3366;Inherit;False;Property;_SideOffset;Side Offset;5;0;Create;True;0;0;0;False;0;False;0.75;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;49;-1495.212,68.97466;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1741.965,-67.89067;Inherit;False;Property;_SideCurveAttenuation;Side Curve Attenuation;6;0;Create;True;0;0;0;False;0;False;20;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;61;-935.1167,211.0807;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;-776.2053,328.4701;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1158.95,369.622;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;145.0632,-33.56519;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;HeightColor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.GradientNode;3;-593.9712,-498.0634;Inherit;False;0;4;2;0.5770614,0.7169812,0.2198291,0.2500038;0.3798161,0.6226415,0.1674083,0.3499962;0.0745098,0.4039216,0.2499914,0.5000076;0.2334906,0.5,0.4873491,0.9000076;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.TFHCRemapNode;2;-573.8143,-412.3558;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-762.2649,-402.4009;Inherit;False;Property;_CouleurMin;CouleurMin;1;0;Create;True;0;0;0;False;0;False;-10;-5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-764.2649,-319.4009;Inherit;False;Property;_CouleurMax;CouleurMax;0;0;Create;True;0;0;0;False;0;False;10;8.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;39;-1017.266,-283.3407;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-1040.776,-570.7176;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-837.2657,-487.3405;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;64;-1106.087,137.9413;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;4;-395.498,-498.827;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-80.06362,-261.8502;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;70;-758.6575,-149.5887;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-300.134,-112.8379;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;74;-319.6128,-189.0918;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-336.9336,-10.43805;Inherit;False;Property;_Strenght;Strenght;7;0;Create;True;0;0;0;False;0;False;3;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-967.4636,29.65724;Inherit;False;Property;_SCALE;SCALE;8;0;Create;True;0;0;0;False;0;False;0.05;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;78;-643.5755,62.78202;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;76;-615.5779,-149.0329;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;37;0;62;0
WireConnection;25;0;35;0
WireConnection;25;1;14;0
WireConnection;36;0;25;0
WireConnection;36;1;22;0
WireConnection;12;0;10;0
WireConnection;12;1;34;0
WireConnection;13;0;12;0
WireConnection;42;0;13;2
WireConnection;42;1;43;0
WireConnection;45;0;42;0
WireConnection;14;0;45;0
WireConnection;58;0;36;0
WireConnection;60;0;59;1
WireConnection;60;1;58;0
WireConnection;50;0;60;0
WireConnection;66;0;49;0
WireConnection;66;1;52;0
WireConnection;67;0;66;0
WireConnection;49;0;50;0
WireConnection;49;1;68;0
WireConnection;61;1;64;0
WireConnection;62;0;26;0
WireConnection;62;1;61;0
WireConnection;26;0;10;0
WireConnection;26;1;36;0
WireConnection;0;0;71;0
WireConnection;0;11;37;0
WireConnection;2;0;41;0
WireConnection;2;1;6;0
WireConnection;2;2;7;0
WireConnection;39;0;36;0
WireConnection;41;0;5;2
WireConnection;41;1;39;1
WireConnection;64;0;67;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;71;0;4;0
WireConnection;71;1;74;0
WireConnection;70;0;5;1
WireConnection;70;1;5;3
WireConnection;73;0;76;0
WireConnection;73;1;72;0
WireConnection;74;0;73;0
WireConnection;78;0;70;0
WireConnection;78;1;75;0
WireConnection;76;1;78;0
ASEEND*/
//CHKSM=92A0DBAE01E3789B66AC8019097EA54AE41D284C