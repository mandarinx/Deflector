Shader "Unlit/Screen Space Texture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackgroundColor ("Background Color", Color) = (0,0,0,1)
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
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal

            #include "UnityCG.cginc"

            // note: no SV_POSITION in this struct
            struct v2f {
                float2 uv : TEXCOORD0;
            };

            v2f vert (
                float4     vertex : POSITION,   // vertex position input
                float2     uv     : TEXCOORD0,  // texture coordinate input
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
            float4 _BackgroundColor;

            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                // screenPos.xy will contain pixel integer coordinates.

                float4 color = _BackgroundColor;
                // 1 when outside, 0 when inside the frame
                float x = -1 * clamp(min(screenPos.x - _Pos.x, 0), -1, 0);
                float y = -1 * clamp(min(screenPos.y - _Pos.y, 0), -1, 0);
                float z = -1 * clamp(min(_Pos.z - screenPos.x, 0), -1, 0);
                float w = -1 * clamp(min(_Pos.w - screenPos.y, 0), -1, 0);

                // 1 when color should be drawn, 0 when image should be drawn
                int drawColor = clamp(x + y + z + w, 0, 1);

                float u = float((screenPos.x - _Pos.x) / (_Pos.z - _Pos.x));
                float v = float((screenPos.y - _Pos.y) / (_Pos.w - _Pos.y));

                #if SHADER_API_METAL
                    v = 1 - v;
                #endif

                return color * drawColor + tex2D(_MainTex, float2(u, v)) * (1 - drawColor);
            }
            ENDCG
        }
    }
}
