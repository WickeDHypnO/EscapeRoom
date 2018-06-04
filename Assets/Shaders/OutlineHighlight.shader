// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/OutlineHighlight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Amount ("Amount", Range(0,0.05)) = 0.02
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
		 Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert alpha:blend

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		 float _Amount;
      void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * _Amount;
      }


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
			o.Emission = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
