 /**
   * @file SeparateAlphaWithCustomUV.shader
   * @author Ján Bella
   * @brief Shader for mixing colors of one texture with its texture coordinates and the second texture serving as an alpha map with its texture coordinates.
   **/
 Shader "Custom/SeparateAlphaWithCustomUV"  {
 Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _AlphaTex ("Alpha (A)", 2D) = "white" {}
    }
	Category {
	   Lighting On
	   ZWrite Off
	   Cull Back
	   Blend SrcAlpha OneMinusSrcAlpha
	   Tags {Queue=Transparent}
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf BlinnPhong  

	  sampler2D _MainTex;
      sampler2D _AlphaTex;

      struct Input {
          float2 uv_MainTex;
		  float2 uv2_AlphaTex;
      };

      void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 c2 = tex2D(_AlphaTex, IN.uv2_AlphaTex);

		o.Albedo = c.rgb;
		o.Alpha = c2.a;
      }
      ENDCG
    }
    //Fallback "Diffuse"
  }
  }