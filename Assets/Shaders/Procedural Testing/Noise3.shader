Shader "Unlit/Noise2"
{
    Properties
    {
        [Header(Skybox Settings)]
        _MainTex ("Texture", 2D) = "white" {}
        _FirstNoise ("Texture", 2D) = "white" {}
        _SecondNoise ("Texture", 2D) = "white" {}
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)

        [Header(Noise Settings)]
        _Scale ("Noise Scaling", float) = 1
        _CellSize ("Cell Size", range(0,1)) = 1
        _TimeAmount ("Time", float) = 1.0
        _CloudCutoff ("Noise Cutoff", range(0,1)) = 0.5
        _Fuzziness ("Fuzz", float) = 1


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 direction : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _FirstNoise;
            sampler2D _SecondNoise;
            float4 _MainTex_ST;
            float4 _ColorA;
            float4 _ColorB;
            float _Scale;
            float3 _CellSize;
            float _TimeAmount;
            float _Fuzziness;
            float _CloudCutoff;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            //Random Generation of values in 3D
            float4 rand(float3 value)
            {
                //make value smaller to avoid artifacts
                float3 smallValue = sin(value);

                float random = dot(smallValue, float3(12.9898, 78.233, 37.719)); 
                random = frac(random * 143758.5453);
                return random;
            }

            //Random value, 2D to 1D
            float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233))
            {
                float2 smallValue = sin(value);
                float random = dot(smallValue, dotDir);
                random = frac(sin(random) * 143758.5453);
                return random;
            }

            //Random Generation of Values in 2D
            float2 rand2dTo2d(float2 value)
            {
                return float2(rand2dTo1d(value, float2(12.989, 78.233)),rand2dTo1d(value, float2(39.346, 11.135)));
            }

            float2 random2d(float2 st)
            {
                st = float2( dot(st,float2(127.1,311.7)), dot(st,float2(269.5,183.3)) );
                return st;
            }

            //Easing functions interpolate between the upper/lower, and left/right quadrants of the noise texture 
            //removing the noticable edges between them.
            float easeIn(float interpolator)
            {
			    return interpolator * interpolator;
		    }

            float easeOut(float interpolator)
            {
                return 1 - easeIn(1 - interpolator);
            }

            float easeInOut(float interpolator)
            {
                float easeInValue = easeIn(interpolator);
                float easeOutValue = easeOut(interpolator);
                return lerp(easeInValue, easeOutValue, interpolator);
		    }


            float noise(float2 st) 
            {
                float2 i = floor(st);
                float2 f = frac(st);

                // Cubic Hermine Curve.  Same as SmoothStep()
                float2 u = f*f*(3.0-2.0*f);

                float2 lowerLeftDirection = random2d(float2(floor(st.x), floor(st.y))) * 2 - 1;
                float2 lowerRightDirection = random2d(float2(ceil(st.x), floor(st.y))) * 2 - 1;
                float2 upperLeftDirection = random2d(float2(floor(st.x), ceil(st.y))) * 2 - 1;
                float2 upperRightDirection = random2d(float2(ceil(st.x), ceil(st.y))) * 2 - 1;

                float lowerLeftFunctionValue = dot(lowerLeftDirection, f - float2(0.0,0.0));
                float lowerRightFunctionValue = dot(lowerRightDirection, f - float2(1.0,0.0) );
                float upperLeftFunctionValue = dot(upperLeftDirection, f - float2(0.0,1.0) );
                float upperRightFunctionValue = dot(upperRightDirection, f - float2(1.0,1.0));

                float interpolatorX = easeInOut(f.x);
			    float interpolatorY = easeInOut(f.y);

                float upperCells = lerp(upperLeftFunctionValue, upperRightFunctionValue, interpolatorX);
                float lowerCells = lerp(lowerLeftFunctionValue, lowerRightFunctionValue, interpolatorX);

                float genNoise = lerp(lowerCells, upperCells, interpolatorY);


                return genNoise;

                
            }
            
            #define PI 3.14
            fixed4 frag (v2f i) : SV_Target
            {
                
                //Generate Gradient
                float4 gradientColor = lerp(_ColorA, _ColorB, saturate(i.uv.y));

                
                
                //First Noise
                float randomNoise1 = noise(i.worldPos / _Scale) + 0.5;


                //Invert values to create "line" effect
                float pixelNoiseChange1 = fwidth(randomNoise1);

                //Smooth Line 
                float heightLine1 = smoothstep(1-pixelNoiseChange1, 1, randomNoise1);
                heightLine1 += smoothstep(pixelNoiseChange1, 0, randomNoise1);

               

                // Sample the texture with our calculated UV & seam fixup.
                fixed4 UVs = tex2D(_MainTex,i.uv) * gradientColor * randomNoise1;

                return UVs;
            }
            ENDCG
        }
    }
}
