
Shader "SemestroProjektas/HexTileColored" 
{
    Properties 
    {
        _MainTex ("Main Texture (A)", 2D) = "white" {}
        _Color("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    SubShader 
    {
        Tags 
        {
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent"
        }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_instancing
                //#pragma target 2.0

                #include "UnityCG.cginc"

                struct appdata_t 
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f 
                {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert (appdata_t v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_TRANSFER_INSTANCE_ID(v, o);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }

                //In order not to duplicate materials, we will be using instancing
                UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)     //uniform we want to change
                UNITY_INSTANCING_BUFFER_END(Props)

                fixed4 frag (v2f i) : SV_Target
                {
                    UNITY_SETUP_INSTANCE_ID(i);
                    fixed4 tex = tex2D(_MainTex, i.texcoord);
                    return tex * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                }
            ENDCG
        }
    }

}
