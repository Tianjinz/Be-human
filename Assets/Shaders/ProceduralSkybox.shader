Shader "Unlit/ProceduralSkybox"
{
    Properties
    {
         [Header(Skybox Settings)]
        _MainTex ("Main Texture", 2D) = "white" {}
        _BaseNoise ("Noise Texture", 2D) = "black" {}
        _SecondNoise ("Noise Texture", 2D) = "black" {}
        _Distort("Distortion Texture", 2D) = "black" {}
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)

        [Header(Noise Settings)]
        _BaseNoiseScale("Base Noise Scale",  Range(0, 1)) = 0.2
        _DistortScale("Distort Noise Scale",  Range(0, 1)) = 0.06
        _SecNoiseScale("Secondary Noise Scale",  Range(0, 1)) = 0.05


        [Head(Clouds Settings)]
        _CloudColorA ("Color A", Color) = (1,1,1,1)
        _CloudColorB  ("Color B", Color) = (1,1,1,1)
        _CloudCutoff("Cloud Cutoff",  Range(0, 1)) = 0.3
        _Fuzziness("Cloud Fuzziness",  Range(0, 1)) = 0.04
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
 
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog
                #pragma shader_feature FUZZY
                #include "UnityCG.cginc"
 
                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 uv : TEXCOORD0;
                };
 
                struct v2f
                {
                    float3 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                    float3 worldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                sampler2D _SecondNoise;
                sampler2D _BaseNoise;
                sampler2D _Distort;
                float _BaseNoiseScale;
                float _DistortScale;
                float _SecNoiseScale;
                float4 _MainTex_ST;
                float4 _ColorA;
                float4 _ColorB;
                float _CloudCutoff;
                float _Fuzziness;
                float _CloudColorA;
                float _CloudColorB;
 
                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                #define PI 3.14
                fixed4 frag(v2f i) : SV_Target
                {
 
                //Reposition UV coordinates 
                float3 pos = normalize(i.worldPos);
                float2 skyboxUV;
                skyboxUV.x = 0.5 + atan2(pos.z,pos.x)/(PI*2);
                skyboxUV.y = 0.5 - asin(pos.y)/PI;
                
                
                float2 dx = ddx(skyboxUV);
                float2 dy = ddy(skyboxUV);
                float2 du = float2(dx.x, dy.x);
                du -= (abs(du) > 0.5f) * sign(du);
                dx.x = du.x;
                dy.x = du.y;

                // In case you want to rotate your view using the texture x-offset.
                skyboxUV.x += _MainTex_ST.z;
 
                // uv for the sky
                float2 skyUV = i.worldPos.xz / i.worldPos.y;

                
                //Generate Gradient
                float4 gradientColor = lerp(_ColorA, _ColorB, saturate(i.uv.y));

                //Spherical skybox
                fixed4 sphereUV = tex2D(_MainTex,skyboxUV,dx,dy);
 
                // moving clouds
                float baseNoise = tex2D(_BaseNoise, ((skyUV - _Time.x) * _BaseNoiseScale)).x;
                float firstNoise = tex2D(_Distort, ((skyUV + baseNoise)- (_Time.x * 0.2)) * _DistortScale);
                float secondNoise = tex2D(_SecondNoise, ((skyUV + firstNoise ) -  (_Time.x * 0.4)) * _SecNoiseScale);
                float finalNoise = saturate(firstNoise * secondNoise) * saturate(i.worldPos.y);

                float clouds = saturate(smoothstep(_CloudCutoff, _CloudCutoff + _Fuzziness, finalNoise));
                clouds = 1 - clouds;

                float3 combined = sphereUV * clouds * gradientColor;
                UNITY_APPLY_FOG(i.fogCoord, combined);
                return float4(combined,1);
 
           }
           ENDCG
       }
        }
}
