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
	
	color.r *= 255;
	color.g *= 255;
	color.b *= 255;
	
    color.r = round(color.r * (Red / 100.0));
    color.g = round(color.g * (Green / 100.0));
    color.b = round(color.b * (Blue / 100.0));
    
	color.r /= 255;
	color.g /= 255;
	color.b /= 255;
	
	color.r = Coerce(color.r, 1);
	color.g = Coerce(color.g, 1);
	color.b = Coerce(color.b, 1);
	return color; 
}