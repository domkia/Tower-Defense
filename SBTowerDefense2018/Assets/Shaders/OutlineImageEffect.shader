Shader "SemestroProjektas/OutlineImageEffect"
{
    Properties
    {
        _HighlightAmount("Fill Alpha", Range(0.0, 1.0)) = 0.25
        _OutlineColor("Outline Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
        _OutlineWidth("Outline Width", FLOAT) = 0.5
        _Opacity("Opacity", Range(0.0, 1.0)) = 1.0
    }
	SubShader
	{
		Cull Off 
        ZWrite Off
        ZTest Always

        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			struct VertexInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			VertexOutput vert (VertexInput i)
			{
				VertexOutput o;
				o.vertex = UnityObjectToClipPos(i.vertex);
				o.uv = i.uv;
				return o;
			}
			
			uniform sampler2D _MainTex;
            fixed2 _MainTex_TexelSize;
            fixed _OutlineWidth;
            fixed4 _OutlineColor;
            fixed _HighlightAmount;
            fixed _Opacity;

			fixed4 frag (VertexOutput i) : SV_Target
			{
                //sample current value
				fixed col = tex2D(_MainTex, i.uv);

                //sample surrounding values
                fixed w = _MainTex_TexelSize * _OutlineWidth;
                fixed blur =        tex2D(_MainTex, i.uv + fixed2(-1.0, 0.0) * w);
                blur = max(blur,    tex2D(_MainTex, i.uv + fixed2(1.0,  0.0) * w));
                blur = max(blur,    tex2D(_MainTex, i.uv + fixed2(0.0,  1.0) * w));
                blur = max(blur,    tex2D(_MainTex, i.uv + fixed2(0.0, -1.0) * w));

                //subtract the middle
                blur -= col * (1.0 - _HighlightAmount);
				return fixed4(_OutlineColor.xyz, blur * _Opacity);
			}
			ENDCG
		}
	}
}
