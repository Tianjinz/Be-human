Shader "Hidden/EdgeDetection"
{

    // Edge rendering issues at larger distances 
    
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold("Threshold", float) = 0.01
        _EdgeColor("Edge Color", color) = (0,0,0,1)
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
                float2 depth : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_DEPTH(o.depth);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthNormalsTexture;
            float4 _MainTex_TexelSize;
            float _Threshold;
            fixed4 _EdgeColor;

            float4 GrabPixelValue(in float2 uv)
            {
                
                // Input Values for DecodeDepthNormal function
                half3 normal;
                float depth;

                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv), depth, normal); //Tells the camera to create a depth texture for each objects UV.
                
                depth = log(depth);
                
                return fixed4(normal, depth); // return the normal and depth values calculated by the decodeDepthNormal function

            }

            fixed4 frag (v2f i) : SV_Target
            {

                //Functionally, this is a sobel convolution matrix 
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 orValue = GrabPixelValue(i.uv);
                float2 offsets[8] = {
                    float2(-1, -1),
                    float2(-1, 0),
                    float2(-1, 1),
                    float2(0, -1),
                    float2(0, 1),
                    float2(1, -1),
                    float2(1, 0),
                    float2(1, 1)
                    
                    //Increasing sampled offsets reduces artifacting
                    // float2(-2, -2),
                    // float2(-2, 2),
                    // float2(-2, 2),
                    // float2(2, -2),
                    // float2(2, 2),
                    // float2(2, -2),
                    // float2(2, 2),
                    // float2(2, 2)

                };
                fixed4 sampledValue = fixed4(0,0,0,0);
                
                for(int j = 0; j < 8; j++)
                {
                    sampledValue += GrabPixelValue(i.uv + offsets[j] * _MainTex_TexelSize.xy);
                }
                sampledValue /= 8;

                
                
                return lerp(col, _EdgeColor, step(_Threshold, length(orValue - sampledValue)));
            }
            ENDCG
        }
    }
}
