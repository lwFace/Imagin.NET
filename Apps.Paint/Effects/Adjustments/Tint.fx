sampler2D input : register(s0);

float Red : register(C0);
float Green : register(C1);
float Blue : register(C2);

float Coerce(float input, float maximum, float minimum = 0)
{
	return max(min(maximum, input), minimum);
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	
	float r = color.r * 255, g = color.g * 255, b = color.b * 255;
	color.r = round(r + (255 - r) * (Red / 100.0)) / 255;
	color.g = round(g + (255 - g) * (Green / 100.0)) / 255;
	color.b = round(b + (255 - b) * (Blue / 100.0)) / 255;
	
	color.r = Coerce(color.r, 1);
	color.g = Coerce(color.g, 1);
	color.b = Coerce(color.b, 1);
	return color; 
}