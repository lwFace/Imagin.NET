sampler2D input : register(s0);

float Model : register(C0);
float Component : register(C1);
float4 Color : register(C2);

//-------------------------------------------------------------------------------------------

static const float pi = 3.14159265;

//-------------------------------------------------------------------------------------------

float Coerce(float input, float maximum, float minimum = 0)
{
	return max(min(maximum, input), minimum);
}

float Modulo(float value, float left, float right)
{
    //Swap frame order
    if (left > right)
    {
        float t = right;
        right = left;
        left = t;
    }

    float frame = right - left;
    value = ((value + left) % frame) - left;

    if (value < left)
        value += frame;

    if (value > right)
        value -= frame;

    return value;
}

//-------------------------------------------------------------------------------------------

float3 Maximums(float model)
{
	float3 result = { 0, 0, 0 };
	
	//RGB
	if (model == 0)
	{
	    result[0] = 1;
	    result[1] = 1;
	    result[2] = 1;
	}
	
	//HCG
	else if (model == 1)
	{
	    result[0] = 359;
	    result[1] = 100;
	    result[2] = 100;
	}
	
	//HSB
	else if (model == 2)
	{
	    result[0] = 359;
	    result[1] = 100;
	    result[2] = 100;
	}
	
	//HSI			
	else if (model == 3)
	{
	    result[0] = 359;
	    result[1] = 100;
	    result[2] = 255;
	}

    //HSL
	else if (model == 4)
	{
        result[0] = 359;
        result[1] = 1;
        result[2] = 1;
        return result;
	}
	//HSP
	else if (model == 5)
	{
	    result[0] = 359;
	    result[1] = 100;
	    result[2] = 255;
	}
    return result;
}

float3 Minimums(float model)
{
	float3 result = { 0, 0, 0 };
	
	//RGB, HCG, HSB, HSI, HSL, HSP
	if (model <= 5)
	{
	    result[0] = 0;
	    result[1] = 0;
	    result[2] = 0;
	}		
    return result;
}

//-------------------------------------------------------------------------------------------

float3 FromHCG(float3 input)
{
	float3 result = { 0, 0, 0 };
	
	float h = input[0] / 359.0, c = input[1] / 100.0, g = input[2] / 100.0;

    if (c == 0)
    {
    	result[0] = result[1] = result[2] = g;
    	return result;
    }

	float hi = (h % 1.0) * 6.0;
	float v = hi % 1.0;
	float3 pure = { 0, 0, 0 };
	float w = 1.0 - v;
	
	float fhi = floor(hi);
	
	if (fhi == 0)
	{
        pure[0] = 1; 
        pure[1] = v; 
        pure[2] = 0;
	}
	else if (fhi == 1)
	{
        pure[0] = w; 
        pure[1] = 1; 
        pure[2] = 0;
	}
	else if (fhi == 2)
	{
        pure[0] = 0; 
        pure[1] = 1; 
        pure[2] = v;
	}
	else if (fhi == 3)
	{
        pure[0] = 0; 
        pure[1] = w; 
        pure[2] = 1;
	}
	else if (fhi == 4)
	{
        pure[0] = v; 
        pure[1] = 0; 
        pure[2] = 1;
	}
	else
	{	        
		pure[0] = 1; 
		pure[1] = 0;
		pure[2] = w;
	}
	
	float mg = (1.0 - c) * g;
	
	result[0] = c * pure[0] + mg;
	result[1] = c * pure[1] + mg;
	result[2] = c * pure[2] + mg;
	return result;
}

float3 FromHSB(float3 input)
{
	float3 result = { 0, 0, 0 };

	float _h = input[0] / 359, _s = input[1] / 100, _b = input[2] / 100;
	float r = 0, g = 0, b = 0;
	
	if (_s == 0)
	{
	    r = g = b = _b;
	}
	else
	{
	    _h *= 359;
	
	    //The color wheel consists of 6 sectors: Figure out which sector we're in...
	    float SectorPosition = _h / 60.0;
	    float SectorNumber = floor(SectorPosition);
	
	    //Get the fractional part of the sector
	    float FractionalSector = SectorPosition - SectorNumber;
	
	    //Calculate values for the three axes of the color. 
	    float p = _b * (1.0 - _s);
	    float q = _b * (1.0 - (_s * FractionalSector));
	    float t = _b * (1.0 - (_s * (1.0 - FractionalSector)));
	
	    //Assign the fractional colors to r, g, and b based on the sector the angle is in.
        if (SectorNumber == 0)
        {
            r = _b;
            g = t;
            b = p;
        }
        else if (SectorNumber == 1)
        {
            r = q;
            g = _b;
            b = p;
        }
        else if (SectorNumber == 2)
        {
            r = p;
            g = _b;
            b = t;
        }
        else if (SectorNumber == 3)
        {
            r = p;
            g = q;
            b = _b;
        }
        else if (SectorNumber == 4)
        {
            r = t;
            g = p;
            b = _b;
        }
        else if (SectorNumber == 5)
        {
            r = _b;
            g = p;
            b = q;
        }
	}
	result[0] = r;
	result[1] = g;
	result[2] = b;
	return result;
}

float3 FromHSI(float3 input)
{
	float3 result = { 0, 0, 0 };

	float pi = 3.1415926535897931;
	float pi3 = pi / 3;

	float h = Modulo(input[0], 0, 359) * pi / 180.0;
	float s = Coerce(input[1], 100) / 100.0;
	float i = Coerce(input[2], 255) / 255.0;
	
	float r = 0, g = 0, b = 0;
	if (h < (2 * pi3))
	{
	    b = i * (1 - s);
	    r = i * (1 + (s * cos(h) / cos(pi3 - h)));
	    g = i * (1 + (s * (1 - cos(h) / cos(pi3 - h))));
	}
	else if (h < (4 * pi3))
	{
	    h = h - 2 * pi3;
	    r = i * (1 - s);
	    g = i * (1 + (s * cos(h) / cos(pi3 - h)));
	    b = i * (1 + (s * (1 - cos(h) / cos(pi3 - h))));
	}
	else
	{
	    h = h - 4 * pi3;
	    g = i * (1 - s);
	    b = i * (1 + (s * cos(h) / cos(pi3 - h)));
	    r = i * (1 + (s * (1 - cos(h) / cos(pi3 - h))));
	}
	
	result[0] = Coerce(r, 1);
	result[1] = Coerce(g, 1);
	result[2] = Coerce(b, 1);
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

float3 FromHSP(float3 input)
{
	float3 result = { 0, 0, 0 };

	const float Pr = 0.299;
	const float Pg = 0.587;
	const float Pb = 0.114;
	
	float h = input[0] / 360.0, s = input[1] / 100.0, p = input[2];
	float r = 0, g= 0, b= 0;
	
	float part= 0, minOverMax = 1.0 - s;
	
	if (minOverMax > 0.0)
	{
	    // R > G > B
	    if (h < 1.0 / 6.0)
	    {
	        h = 6.0 * (h - 0.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        b = p / sqrt(Pr / minOverMax / minOverMax + Pg * part * part + Pb);
	        r = (b) / minOverMax;
	        g = (b) + h * ((r) - (b));
	    }
	    // G > R > B
	    else if (h < 2.0 / 6.0)
	    {
	        h = 6.0 * (-h + 2.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        b = p / sqrt(Pg / minOverMax / minOverMax + Pr * part * part + Pb);
	        g = (b) / minOverMax;
	        r = (b) + h * ((g) - (b));
	    }
	    // G > B > R
	    else if (h < 3.0 / 6.0)
	    {
	        h = 6.0 * (h - 2.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        r = p / sqrt(Pg / minOverMax / minOverMax + Pb * part * part + Pr);
	        g = (r) / minOverMax;
	        b = (r) + h * ((g) - (r));
	    }
	    // B > G > R
	    else if (h < 4.0 / 6.0)
	    {
	        h = 6.0 * (-h + 4.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        r = p / sqrt(Pb / minOverMax / minOverMax + Pg * part * part + Pr);
	        b = (r) / minOverMax;
	        g = (r) + h * ((b) - (r));
	    }
	    // B > R > G
	    else if (h < 5.0 / 6.0)
	    {
	        h = 6.0 * (h - 4.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        g = p / sqrt(Pb / minOverMax / minOverMax + Pr * part * part + Pg);
	        b = (g) / minOverMax;
	        r = (g) + h * ((b) - (g));
	    }
	    // R > B > G
	    else
	    {
	        h = 6.0 * (-h + 6.0 / 6.0);
	        part = 1.0 + h * (1.0 / minOverMax - 1.0);
	        g = p / sqrt(Pr / minOverMax / minOverMax + Pb * part * part + Pg);
	        r = (g) / minOverMax;
	        b = (g) + h * ((r) - (g));
	    }
	}
	else
	{
	    // R > G > B
	    if (h < 1.0 / 6.0)
	    {
	        h = 6.0 * (h - 0.0 / 6.0);
	        r = sqrt(p * p / (Pr + Pg * h * h));
	        g = (r) * h;
	        b = 0.0;
	    }
	    // G > R > B
	    else if (h < 2.0 / 6.0)
	    {
	        h = 6.0 * (-h + 2.0 / 6.0);
	        g = sqrt(p * p / (Pg + Pr * h * h));
	        r = (g) * h;
	        b = 0.0;
	    }
	    // G > B > R
	    else if (h < 3.0 / 6.0)
	    {
	        h = 6.0 * (h - 2.0 / 6.0);
	        g = sqrt(p * p / (Pg + Pb * h * h));
	        b = (g) * h;
	        r = 0.0;
	    }
	    // B > G > R
	    else if (h < 4.0 / 6.0)
	    {
	        h = 6.0 * (-h + 4.0 / 6.0);
	        b = sqrt(p * p / (Pb + Pg * h * h));
	        g = (b) * h;
	        r = 0.0;
	    }
	    // B > R > G
	    else if (h < 5.0 / 6.0)
	    {
	        h = 6.0 * (h - 4.0 / 6.0);
	        b = sqrt(p * p / (Pb + Pr * h * h));
	        r = (b) * h;
	        g = 0.0;
	    }
	    // R > B > G
	    else
	    {
	        h = 6.0 * (-h + 6.0 / 6.0);
	        r = sqrt(p * p / (Pr + Pb * h * h));
	        b = (r) * h;
	        g = 0.0;
	    }
	}
	result[0] = Coerce(round(r) / 255.0, 1);
	result[1] = Coerce(round(g) / 255.0, 1);
	result[2] = Coerce(round(b) / 255.0, 1);
	return result;
}

//-------------------------------------------------------------------------------------------

float3 From(float3 input)
{
	float3 result = { 0, 0, 0 };
	
	//RGB
	if (Model == 0)
	{
	    result = input;
	}
	//HCG
	else if (Model == 1)
	{
	    result = FromHCG(input);
	}
	//HSB
	else if (Model == 2)
	{
	    result = FromHSB(input);
	}
	//HSI
	else if (Model == 3)
	{
	    result = FromHSI(input);
	}
	//HSL
	else if (Model == 4)
	{
	    result = FromHSL(input);
	}
	//HSP
	else if (Model == 5)
	{
	    result = FromHSP(input);
	}
	return result;
}

//-------------------------------------------------------------------------------------------

float3 RGB_HCG(float3 input)
{
	float r = input[0], g = input[1], b = input[2];
	
	float maximum = max(max(r, g), b);
	float minimum = min(min(r, g), b);
	
	float chroma = maximum - minimum;
	float grayscale = 0;
	float hue;
	
	if (chroma < 1)
	    grayscale = minimum / (1.0 - chroma);
	
	if (chroma > 0)
	{
	    if (maximum == r)
	    {
	        hue = ((g - b) / chroma) % 6;
	    }
	    else if (maximum == g)
	    {
	        hue = 2 + (b - r) / chroma;
	    }
	    else hue = 4 + (r - g) / chroma;
	
	    hue /= 6;
	    hue = hue % 1;
	}
	else hue = 0;
	
	float3 result = { hue * 360.0, chroma * 100.0, grayscale * 100.0 };
	return result;
}

float3 RGB_HSB(float3 input)
{
	float r = input[0], g = input[1], b = input[2];
	
	float minimum = min(input[0], min(input[1], input[2]));
	float maximum = max(input[0], max(input[1], input[2]));
	
	float chroma = maximum - minimum;
	
	float _h = 0.0;
	float _s = 0.0;
	float _b = maximum;
	
	if (chroma == 0)
	{
	    _h = 0;
	    _s = 0;
	}
	else
	{
	    _s = chroma / maximum;
	
	    if (input[0] == maximum)
	    {
	        _h = (input[1] - input[2]) / chroma;
	        _h = input[1] < input[2] ? _h + 6 : _h;
	    }
	    else if (input[1] == maximum)
	    {
	        _h = 2.0 + ((input[2] - input[0]) / chroma);
	    }
	    else if (input[1] == maximum)
	        _h = 4.0 + ((input[0] - input[1]) / chroma);
	
	    _h *= 60;
	}
	
	float3 result = { _h, _s * 100, _b * 100 };
	return result;
}

float3 RGB_HSI(float3 input)
{
    float3 _input = { input[0] * 255, input[1] * 255, input[2] * 255 };
    float sum = _input[0] + _input[1] + _input[2];

    float r = _input[0] / sum;
    float g = _input[1] / sum;
    float b = _input[2] / sum;

    float h = acos
    (
        (0.5 * ((r - g) + (r - b)))
        /
        sqrt
        (
            (r - g) * (r - g) + (r - b) * (g - b)
        )
    );

    if (b > g)
        h = 2 * pi - h;

    float s = 1 - 3 * min(r, min(g, b));
    float i = sum / 3;

    float3 result = { h * 180 / pi, s * 100, i };
    return result;
}

float3 RGB_HSL(float3 input)
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

float3 RGB_HSP(float3 input)
{
    const float Pr = 0.299;
    const float Pg = 0.587;
    const float Pb = 0.114;

    float3 _input = { input[0] * 255, input[1] * 255, input[2] * 255 };
    float r = _input[0], g = _input[1], b = _input[2];
    float h = 0, s = 0, p = 0;

    p = sqrt(r * r * Pr + g * g * Pg + b * b * Pb);

    if (r == g && r == b)
    {
        h = 0.0;
        s = 0.0;
    }
    else
    {
        //R is largest
        if (r >= g && r >= b)
        {
            if (b >= g)
            {
                h = 6.0 / 6.0 - 1.0 / 6.0 * (b - g) / (r - g);
                s = 1.0 - g / r;
            }
            else
            {
                h = 0.0 / 6.0 + 1.0 / 6.0 * (g - b) / (r - b);
                s = 1.0 - b / r;
            }
        }

        //G is largest
        if (g >= r && g >= b)
        {
            if (r >= b)
            {
                h = 2.0 / 6.0 - 1.0 / 6.0 * (r - b) / (g - b);
                s = 1 - b / g;
            }
            else
            {
                h = 2.0 / 6.0 + 1.0 / 6.0 * (b - r) / (g - r);
                s = 1.0 - r / g;
            }
        }

        //B is largest
        if (b >= r && b >= g)
        {
            if (g >= r)
            {
                h = 4.0 / 6.0 - 1.0 / 6.0 * (g - r) / (b - r);
                s = 1.0 - r / b;
            }
            else
            {
                h = 4.0 / 6.0 + 1.0 / 6.0 * (r - g) / (b - g);
                s = 1.0 - g / b;
            }
        }
    }
    float3 result = { round(h * 360.0), s * 100.0, round(p) };
    return result;
}

//-------------------------------------------------------------------------------------------

float3 FromRGB(float3 input)
{
	float3 result = { 0, 0, 0 };
	
	//RGB
	if (Model == 0)
	{
	    result = input;
	}
	//HCG
	else if (Model == 1)
	{
	    result = RGB_HCG(input);
	}
	//HSB
	else if (Model == 2)
	{
	    result = RGB_HSB(input);
	}
	//HSI
	else if (Model == 3)
	{
	    result = RGB_HSI(input);
	}
	//HSL
	else if (Model == 4)
	{
	    result = RGB_HSL(input);
	}
	//HSP
	else if (Model == 5)
	{
		result = RGB_HSP(input);
	}
	return result;
}

//-------------------------------------------------------------------------------------------

float2 XY()
{
    //We need to convert the current color to a color in the current color model
    float3 rgb = { Color.r, Color.g, Color.b };
    float3 color = FromRGB(rgb);

    float3 maximums = Maximums(Model);
    float3 minimums = Minimums(Model);

	float2 result = { 0, 0 };
    //We then need to grab two of the three values based on the current component
    if (Component == 0)
    {
	    result[0] = (color[1] - minimums[1]) / (maximums[1] - minimums[1]);
	    result[1] = (color[2] - minimums[2]) / (maximums[2] - minimums[2]);
    }
    else if (Component == 1)
    {
  	    result[0] = (color[0] - minimums[0]) / (maximums[0] - minimums[0]);
	    result[1] = (color[2] - minimums[2]) / (maximums[2] - minimums[2]);
    }
    else if (Component == 2)
    {
  	    result[0] = (color[0] - minimums[0]) / (maximums[0] - minimums[0]);
	    result[1] = (color[1] - minimums[1]) / (maximums[1] - minimums[1]);
    }
    return result;
}

//-------------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	
	//We need to be able to figure out what x and y is based on the current color!
	float2 xy = XY();
	
	//x, y, z = [0, 1]
	float x = xy[0];
	float y = xy[1];
	
	float z = 1 - uv.xy.y;
	
	float c = Component;
	float m = Model;
	
	//0 RGB
	//1 HCG
	//2 HSB
	//3 HSI
	//4 HSL
	//5 HSM
	//6 HSP
	//7 HWB
	//8 TSL
	//9 YCoCg
	//10 YES
	//11 YIQ
	//12 YUV

	float3 output;
	float3 input;
		
	float3 maximums = Maximums(m);
	float3 minimums = Minimums(m);

	if (c == 0)
	{
		input[0] = (z * (maximums[0] - minimums[0])) + minimums[0];
		input[1] = (x * (maximums[1] - minimums[1])) + minimums[1];
		input[2] = (y * (maximums[2] - minimums[2])) + minimums[2];
	}
	else if (c == 1)
	{
		input[0] = (x * (maximums[0] - minimums[0])) + minimums[0];
		input[1] = (z * (maximums[1] - minimums[1])) + minimums[1];
		input[2] = (y * (maximums[2] - minimums[2])) + minimums[2];
	}
	else if (c == 2)
	{
		input[0] = (x * (maximums[0] - minimums[0])) + minimums[0];
		input[1] = (y * (maximums[1] - minimums[1])) + minimums[1];
		input[2] = (z * (maximums[2] - minimums[2])) + minimums[2];
	}
	
	output = From(input);
	
	color.r = output[0];
	color.g = output[1];
	color.b = output[2];
	return color; 
}