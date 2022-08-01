Shader "Unlit/Noise_Final"
{
    Properties
    {
        [Header(Skybox Settings)]
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)

        [Header(Noise Settings)]
        _Scale ("Noise Scaling", Range(0,10)) = 0.2
        _CellSize ("Cell Size", vector) = (1,1,1,1)
        _Roughness ("Roughness", Range(1, 8)) = 3
        _Persistance ("Persistance", Range(0, 1)) = 0.4



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
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _ColorA;
            float4 _ColorB;
            float _Scale;
            float3 _CellSize;
            float _Roughness;
            float _Persistance;



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.direction = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            //Random Generation of values in 3D
            float4 rand(float3 value)
            {
                //make value smaller to avoid artefacts
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

            float2 random2(float2 st)
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


            float Noise(float2 value) 
            {
                
                float2 f = frac(value);

                float2 lowerLeftDirection = rand2dTo2d(float2(floor(value.x), floor(value.y))) * 2 - 1;
                float2 lowerRightDirection = rand2dTo2d(float2(ceil(value.x), floor(value.y))) * 2 - 1;
                float2 upperLeftDirection = rand2dTo2d(float2(floor(value.x), ceil(value.y))) * 2 - 1;
                float2 upperRightDirection = rand2dTo2d(float2(ceil(value.x), ceil(value.y))) * 2 - 1;

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

            #define OCTAVES 4
            
            float sampleLayeredNoise(float2 value)
            {
                float noise = 0;
                float frequency = 1;
                float factor = 1;

                [unroll]
                for(int i=0; i<OCTAVES; i++)
                {
                    noise = noise + Noise(value * frequency + i * 0.72354) * factor;
                    factor *= _Persistance;
                    frequency *= _Roughness;
                }

                return noise;
            }
            
            #define PI 3.14
            fixed4 frag (v2f i) : SV_Target
            {   
                float2 value = i.uv / _CellSize;
                
                float perlinNoise = sampleLayeredNoise(value) + 0.5;
                perlinNoise = sampleLayeredNoise(value) + 0.5;
                perlinNoise = sampleLayeredNoise(value) + 0.5;
                
                //Apply noises to textures.
               
                //Saturate + Smoothstep to adjust how much noise is visible in the skybox
                //float clouds = saturate(smoothstep(_CloudCutoff, _CloudCutoff, finalNoise));

                // Sample the texture with our calculated UV & seam fixup.
                fixed4 UVs = tex2D(_MainTex,i.uv) * perlinNoise;

                return UVs;
            }
            ENDCG
        }
    }
}
