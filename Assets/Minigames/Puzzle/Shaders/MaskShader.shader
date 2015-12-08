Shader "Unlit/MaskShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AlphaTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		ZWrite Off

		Pass
		{			
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _AlphaTex;
			sampler2D _MainTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.uv2 = v.uv2;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float maskValue = tex2D(_AlphaTex, i.uv2).r;
				
				//if (maskValue < 0.2) discard;
				return float4(tex2D(_MainTex, i.uv).xyz, maskValue);
			}
			ENDCG
		}
	}
}
