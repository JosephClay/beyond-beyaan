sampler2D g_samSrcColor;

float4 StarColor;

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{
    float4 Color;
    
    Color = tex2D( g_samSrcColor, Tex.xy);
	if (Color.r == (178.0f/255.0f) && Color.g == (225.0f/255.0f) && Color.b == (248.0f/255.0f))
	{
		Color.r = ((StarColor.r * 75) + 180) / 255.0f;
		Color.g = ((StarColor.g * 75) + 180) / 255.0f;
		Color.b = ((StarColor.b * 75) + 180) / 255.0f;
		return Color;
	}
	if (Color.r == (108.0f/255.0f) && Color.g == (200.0f/255.0f) && Color.b == (245.0f/255.0f))
	{
		Color.r = ((StarColor.r * 150) + 55) / 255.0f;
		Color.g = ((StarColor.g * 150) + 55) / 255.0f;
		Color.b = ((StarColor.b * 150) + 55) / 255.0f;
		return Color * 0.95f;
	}
	if (Color.r == (45.0f/255.0f) && Color.g == (163.0f/255.0f) && Color.b == (225.0f/255.0f))
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
