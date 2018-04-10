Shader "SemestroProjektas/White"
{
	SubShader
    {
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            struct VertexInput
            {
                fixed4 vertex : POSITION;
            };

			struct VertexOutput
			{
				fixed4 vertex : SV_POSITION;
			};
			
			VertexOutput vert(VertexInput v)
			{
				VertexOutput o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (VertexOutput i) : COLOR0
			{
				return fixed4(1.0, 1.0, 1.0, 1.0);
			}
			ENDCG
		}
	}
}
