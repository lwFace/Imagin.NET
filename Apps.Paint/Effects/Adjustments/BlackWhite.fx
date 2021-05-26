sampler2D input : register(s0);

float Reds : register(C0);
float Greens : register(C1);
float Blues : register(C2);
float Cyans : register(C3);
float Yellows : register(C4);
float Magentas : register(C5);

float3 FromRGB(float3 input)
{
	float maximum = max(max(input[0], input[1]), input[2]);
	float minimum = min(min(input[0], input[1]), input[2]);
	
	float chroma = maximum - minimum;
	
	float h = 0, s = 0, l = (maximum + minimum) / 2.0;
	
	if (chroma != 0)
	{
	    s
	        = l < 0.5
	        ? chroma / (2.0 * l)
	        : chroma / (2.0 - 2.0 * l);
	
	    if (input[0] == maximum)
	    {
	        h = (input[1] - input[2]) / chroma;
	        h = input[1] < input[2]
	        ? h + 6.0
	        : h;
	    }
	    else if (input[2] == maximum)
	    {
	        h = 4.0 + ((input[0] - input[1]) / chroma);
	    }
	    else if (input[1] == maximum)
	        h = 2.0 + ((input[2] - input[0]) / chroma);
	
	    h *= 60;
	}
	
	float3 result = { h, s, l };
	return result;
}

float3 FromHSL(float3 input)
{
	float h = input[0], s = input[1], l = input[2];
	h /= 60;

	float3 result = { 0, 0, 0 };
	
	if (s > 0)
	{
		float chroma = (1.0 - abs(2.0 * l - 1.0)) * s;
		float v = chroma * (1.0 - abs((h % 2.0) - 1));
	
		if (0 <= h && h <= 1)
		{
			result[0] = chroma;
			result[1] = v;
			result[2] = 0;
		}
		else if (1 <= h && h <= 2)
		{                
			result[0] = v;
			result[1] = chroma;
			result[2] = 0;
		}
		else if (2 <= h && h <= 3)
		{
			result[0] = 0;
			result[1] = chroma;
			result[2] = v;
		}
		else if (3 <= h && h <= 4)
		{
			result[0] = 0;
			result[1] = v;
			result[2] = chroma;
		}
		else if (4 <= h && h <= 5)
		{
			result[0] = v;
			result[1] = 0;
			result[2] = chroma;
		}
		else if (5 <= h && h <= 6)
		{
			result[0] = chroma;
			result[1] = 0;
			result[2] = v;
		}
		
		float w = l - (0.5 * chroma);
		result[0] += w;
		result[1] += w;
		result[2] += w;
	}
	else
	{
		result[0] = result[1] = result[2] = l;
	}
	return result;
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 

	float3 rgb = { color.r, color.g, color.b };
	float3 hsl = FromRGB(rgb);
	
    float hue
        = hsl[0];
    float saturation
        = hsl[1];
    float brightness
        = hsl[2];

    float r = color.r * 255;
    float g = color.g * 255;
    float b = color.b * 255;

    //Figure out which component is most dominant and increase or decrease appropriately
    float oldBrightness = brightness;

    //Yellow
    if (r > b && g > b)
    {
        brightness += (oldBrightness * Yellows / 100);
    }
    //Magenta
    else if (r > g && b > g)
    {
        brightness += (oldBrightness * Magentas / 100);
    }
    //Cyan
    else if (g > r && b > r)
    {
        brightness += (oldBrightness * Cyans / 100);
    }

    //Red
    if (r > g && r > b)
    {
        brightness += (oldBrightness * Reds / 100);
    }
    //Green
    else if (g > r && g > b)
    {
        brightness += (oldBrightness * Greens / 100);
    }
    //Blue
    else if (b > r && b > g)
    {
        brightness += (oldBrightness * Blues / 100);
    }

	float3 hsl2 = { hue, 0, brightness };
	float3 rgb2 = FromHSL(hsl2);
	
	color.r = rgb2[0];
	color.g = rgb2[1];
	color.b = rgb2[2];
	return color;
}