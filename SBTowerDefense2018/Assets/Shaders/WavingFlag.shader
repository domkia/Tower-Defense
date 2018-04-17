Shader "SemestroProjektas/WavingFlag"
{
    Properties
    {
        _Color("Color", Color) = (0.0, 0.5, 1.0, 1.0)
        _Amplitude("Amplitude", Range(0.0, 1.0)) = 0.5
        _WaveLength("Wave length", Float) = 0.5
        _Speed("Speed", Float) = 5.0
    }

	SubShader
	{
		Tags { "RenderType"="Opaque" }
        Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			struct VertexInput
			{
				float4 pos : POSITION;
                float4 col : COLOR0;
			};

			struct VertexOutput
			{
				float4 pos : SV_POSITION;
                float vertexOffset : TEXCOORD0;
			};

            half _Amplitude;
            half _WaveLength;
            half _Speed;
            half4 _Color;
			
			VertexOutput vert (VertexInput v)
			{
				VertexOutput o;
                o.vertexOffset = cos(v.pos.y * -(1.0 / _WaveLength) * 10.0f + _Time.y * _Speed) * v.col.x * 0.1f;
                v.pos.x += o.vertexOffset * _Amplitude;
				o.pos = UnityObjectToClipPos(v.pos);
				return o;
			}
			
			fixed4 frag (VertexOutput i) : SV_Target
			{
				return _Color + i.vertexOffset;
			}
			ENDCG
		}
	}
}
