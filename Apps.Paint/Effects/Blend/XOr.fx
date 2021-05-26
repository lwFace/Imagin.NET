//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- NormalEffect
//
//--------------------------------------------------------------------------------------

sampler2D input : register(s0);
sampler2D blend : register(s1);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float3 RGBToHSL(float3 color)
{
	float3 hsl;
	
	float fmin = min(min(color.r, color.g), color.b);
	float fmax = max(max(color.r, color.g), color.b);
	float delta = fmax - fmin;
	float maxplusmin = fmax + fmin;

	// set luminance
	hsl.z = maxplusmin / 2.0;

	if (delta == 0.0)
	{
		// a gray color, set hue and satuation to 0
		hsl.x = 0.0;
		hsl.y = 0.0;
	}
	else
	{
		// not a gray color

		// set saturation
		if (hsl.z < 0.5)
			hsl.y = delta / (maxplusmin);
		else
			hsl.y = delta / (2.0 - maxplusmin);
		
		if (color.r == fmax)
			hsl.x = ((color.g - color.b) / delta);
		else if (color.g == fmax)
			hsl.x = 2.0 + ((color.b - color.r) / delta);
		else if (color.b == fmax)
			hsl.x = 4.0 + ((color.r - color.g) / delta);

		hsl.x = hsl.x / 6.0;

		if (hsl.x < 0.0)
			hsl.x += 1.0;
	}

	return hsl;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 inputColor;
	inputColor = tex2D(input, uv);

	float4 blendColor;
	blendColor = tex2D(blend, uv);

	float4 result = { 0, 0, 0, 0 };
	
	if (blendColor.a == 0)
		return inputColor;

	float3 rgb = { inputColor.r, inputColor.g, inputColor.b };
	float l = RGBToHSL(rgb);
	
	if (l < 0.5)
	{
		result.a = blendColor.a;
		result.r = 1;
		result.g = 1;
		result.b = 1;
	}
	else 
	{
		result.a = blendColor.a;
		result.r = 0;
		result.g = 0;
		result.b = 0;
	}
	
	return result;
}