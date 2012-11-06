sampler2D g_samSrcColor;

float4 StarColor;

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{
    float4 Color;
    
    Color = tex2D( g_samSrcColor, Tex.xy);
	if (Color.r == (189.0f/255.0f) && Color.g == (222.0f/255.0f) && Color.b == (243.0f/255.0f))
	{
		Color.r = ((StarColor.r * 75) + 180) / 255.0f;
		Color.g = ((StarColor.g * 75) + 180) / 255.0f;
		Color.b = ((StarColor.b * 75) + 180) / 255.0f;
		return Color;
	}
	if (Color.r == (85.0f/255.0f) && Color.g == (181.0f/255.0f) && Color.b == (240.0f/255.0f))
	{
		Color.r = ((StarColor.r * 150) + 55) / 255.0f;
		Color.g = ((StarColor.g * 150) + 55) / 255.0f;
		Color.b = ((StarColor.b * 150) + 55) / 255.0f;
		return Color * 0.95f;
	}
	if (Color.r == (25.0f/255.0f) && Color.g == (123.0f/255.0f) && Color.b == (181.0f/255.0f))
	{
		return StarColor * 0.75f;
	}
	return Color;
}


technique PostProcess
{
    pass p1
    {
        VertexShader = null;
        PixelShader = compile ps_2_0 MyShader();
    }

}
