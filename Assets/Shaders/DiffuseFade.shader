Shader "Custom/DiffuseFade"
{
	Properties 
	{
_Color("_Color", Color) = (0.0597015,0.0597015,0.0597015,0.5215687)
_Diffuse("_Diffuse", 2D) = "white" {}
_Normal("_Normal", 2D) = "bump" {}
_Cube("_Cube", Cube) = "white" {}
_SpecLevel("_SpecLevel", Range(0,1) ) = 0.5
_SpecMap("_SpecMap", 2D) = "black" {}

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Blend SrcAlpha OneMinusSrcAlpha
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 3.0


float4 _Color;
sampler2D _Diffuse;
sampler2D _Normal;
samplerCUBE _Cube;
float _SpecLevel;
sampler2D _SpecMap;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}

			inline half4 LightingBlinnPhongEditor_DirLightmap (EditorSurfaceOutput s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
			{
				UNITY_DIRBASIS
				half3 scalePerBasisVector;
				
				half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector);
				
				half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
				half3 h = normalize (lightDir + viewDir);
			
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular * 128.0);
				
				// specColor used outside in the forward path, compiled out in prepass
				specColor = lm * _SpecColor.rgb * s.Gloss * spec;
				
				// spec from the alpha component is used to calculate specular
				// in the Lighting*_Prepass function, it's not used in forward
				return half4(lm, spec);
			}
			
			struct Input {
				float2 uv_Diffuse;
float2 uv_Normal;
float3 simpleWorldRefl;
float2 uv_SpecMap;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

o.simpleWorldRefl = -reflect( normalize(WorldSpaceViewDir(v.vertex)), normalize(mul((float3x3)_Object2World, SCALED_NORMAL)));

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D0=tex2D(_Diffuse,(IN.uv_Diffuse.xyxy).xy);
float4 Subtract1=Tex2D0 - float4( 1E-07,1E-07,1E-07,1E-07 );
float4 Tex2DNormal0=float4(UnpackNormal( tex2D(_Normal,(IN.uv_Normal.xyxy).xy)).xyz, 1.0 );
float4 TexCUBE0=texCUBE(_Cube,float4( IN.simpleWorldRefl.x, IN.simpleWorldRefl.y,IN.simpleWorldRefl.z,1.0 ));
float4 Tex2D2=tex2D(_SpecMap,(IN.uv_SpecMap.xyxy).xy);
float4 Multiply1=TexCUBE0 * Tex2D2.aaaa;
float4 Multiply0=Multiply1 * _Color;
float4 SplatAlpha0=_Color.w;
float4 Add0=float4( 1E-07,1E-07,1E-07,1E-07 ) + SplatAlpha0;
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Add0;
float4 Subtract0=Tex2D0.aaaa - Invert0;
float4 Master0_7_NoInput = float4(0,0,0,0);
clip( Tex2D0.aaaa );
o.Albedo = Subtract1;
o.Normal = Tex2DNormal0;
o.Emission = Multiply0;
o.Specular = _SpecLevel.xxxx;
o.Gloss = Tex2D2.aaaa;
o.Alpha = Subtract0;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}