Shader "Unlit/Screen Space Texture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pos ("Screen pos", Vector) = (0,0,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            // note: no SV_POSITION in this struct
            struct v2f {
                float2 uv : TEXCOORD0;
            };

            v2f vert (
                float4 vertex : POSITION, // vertex position input
                float2 uv : TEXCOORD0, // texture coordinate input
                out float4 outpos : SV_POSITION // clip space position output
                )
            {
                v2f o;
                o.uv = uv;
                outpos = UnityObjectToClipPos(vertex);
                return o;
            }

            sampler2D _MainTex;
            float4 _Pos;

            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                // screenPos.xy will contain pixel integer coordinates.
                // clip HLSL instruction stops rendering a pixel if value is negative

                clip(screenPos.x - _Pos.x);
                clip(_Pos.z - screenPos.x);
                clip(screenPos.y - _Pos.y);
                clip(_Pos.w - screenPos.y);

                return tex2D(_MainTex, float2(
                    float((screenPos.x - _Pos.x) / (_Pos.z - _Pos.x)),
                    float((screenPos.y - _Pos.y) / (_Pos.w - _Pos.y))
                ));
            }
            ENDCG
        }
    }
}
