sampler2D g_samSrcColor;

float4 CircleColor;
float Arc[3];

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{
    float4 Color;
    
    Color = tex2D( g_samSrcColor, Tex.xy);

	float deltaX = Tex.x - Arc[0];
	float deltaY = Tex.y - Arc[0];
	float angle = atan2(deltaY, deltaX);
	if (angle > Arc[1] && angle < Arc[2])
	{
		float4 newColor;
		newColor.r = Color.r * CircleColor.r;
		newColor.g = Color.g * CircleColor.g;
		newColor.b = Color.b * CircleColor.b;
		newColor.a = 1;
		if (newColor.r == 0 && newColor.g == 0 && newColor.b == 0)
		{
			return 0;
		}
		return newColor;
	}
	
	return 0;
}


technique PostProcess
{
    pass p1
    {
        VertexShader = null;
        PixelShader = compile ps_2_0 MyShader();
    }

}
