Shader "Hidden/Depth_Coloring"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 scrPos : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 scrPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.scrPos = ComputeScreenPos(o.vertex);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthNormalsTexture;

            fixed4 frag (v2f i) : SV_TARGET
            {
                //read depthnormal
                float4 depthnormal = tex2D(_CameraDepthNormalsTexture, i.uv);

                //decode depthnormal
                float3 normal;
                float depth;
                DecodeDepthNormal(depthnormal, depth, normal);
                
                
                fixed4 orgColor = tex2Dproj(_MainTex, i.scrPos); //Get the orginal rendered color
                float4 tintColor;


                //Use depthnormals to drive the material colours
    
                fixed4 col = tex2D(_MainTex, i.uv) * depth;
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return depthnormal;
            }
            ENDCG
        }
    }
}
