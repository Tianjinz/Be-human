Shader "Custom/Cel_Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Gloss", float) = 0
        _Antialiasing("Shading AA", Float) = 5.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            //"LightMode" = "ForwardBase"
	        //"PassFlags" = "OnlyDirectional"
            
        }
        
        LOD 200

        CGPROGRAM
        // Cel shaded lighting model, and enable shadows on all light types
        #pragma surface surf Cel 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "UnityCG.cginc"
        
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Antialiasing;
        float _Emission;
    

        half4 LightingCel(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {

            //Create Cel Shaded Lighting
            half NdotL = dot(s.Normal, lightDir);
			if (NdotL >= 0.0) NdotL = 1;
			else NdotL = 0;

            //Diffuse Component 
            float diffuse = (NdotL * atten * 2);
            float delta = fwidth(diffuse) * _Antialiasing;
            float diffuseSmooth = smoothstep(0, delta, diffuse);

            
			half4 c;
			c.rgb = s.Albedo * diffuseSmooth * _LightColor0 * unity_AmbientSky;
			c.a = s.Alpha;
			return c;
        
        }

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 uv_Normal;

        };

   

        void surf (Input IN, inout SurfaceOutput o)
        {

            // Albedo comes from a texture tinted by color
            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
