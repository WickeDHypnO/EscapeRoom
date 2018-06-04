Shader "Custom/PingShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_EmmisiveStrength("Emmisive Strength",float) = 1
		_Alpha("Alpha",float) = 1
	}
	SubShader {
      Tags { "RenderType" = "Transparent"  "Queue"="Transparent" "ForceNoShadowCasting"="True"}
	  		 Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
      CGPROGRAM
      #pragma surface surf Lambert alpha:blend
      struct Input {
          float3 worldPos;
      };
      
	  fixed4 _Color;
	  float _EmmisiveStrength;
	  float _Alpha;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Emission = _Color.rgb * _EmmisiveStrength; 
		  o.Alpha = _Alpha;
      }
      ENDCG
    } 
  }