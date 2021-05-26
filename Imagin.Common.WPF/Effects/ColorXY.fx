sampler2D input : register(s0);

float Model : register(C0);
float Component : register(C1);
float Value : register(C2);

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
	
	//RGB; HSM; TSL; YES
	if (model == 0 || model == 5 || model == 8 || model == 10)
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
	else if (model == 6)
	{
	    result[0] = 359;
	    result[1] = 100;
	    result[2] = 255;
	}
	
	//HWB
	else if (model == 7)
	{
	    result[0] = 359;
	    result[1] = 100;
	    result[2] = 100;
	}
	
	//YCoCg
	else if (model == 9)
	{
	    result[0] = 1;
	    result[1] = 0.5;
	    result[2] = 0.5;
	}
	
	//YIQ
	else if (model == 11)
	{
	    result[0] = 1;
	    result[1] = 0.5957;
	    result[2] = 0.5226;
	}
	
	//YUV
	else if (model == 12)
	{
	    result[0] = 1;
	    result[1] = .436;
	    result[2] = .615;
	}           
    return result;
}

float3 Minimums(float model)
{
	float3 result = { 0, 0, 0 };
	
	//RGB, HCG, HSB, HSI, HSL, HSM, HSP, HWB, TSL || YES (respectively!)
	if (model <= 8 || model == 10)
	{
	    result[0] = 0;
	    result[1] = 0;
	    result[2] = 0;
	}		
	//YCoCg
	else if (model == 9)
	{
        result[0] = 0;
        result[1] = -0.5;
        result[2] = -0.5;
	}
	//YIQ
	else if (model == 11)
	{
        result[0] = 0;
        result[1] = -0.5957;
        result[2] = -0.5226;
	}
	//YUV
	else if (model == 12)
	{
        result[0] = 0;
        result[1] = -.436;
        result[2] = -.615;
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

float3 FromHSM(float3 input)
{	
	float3 result = { 0, 0, 0 };

	float x = cos(input[0]);
	float w = sqrt(41.0) * input[1] * x;
	
	float a = .0, b = .0, c = .0;
	
	a = 3.0 / 41.0 * input[1] * x;
	b = input[2];
	c = 4.0 / 861.0 * sqrt(861.0 * pow(input[1], 2) * (1.0 - pow(x, 2)));
	
	result[0] =  a + b - c;

	a = w;
	b = 23.0 * input[2];
	c = 19.0 * result[0];
	
	result[1] = (a + b - c) / 4.0;

	a = 11 * result[0];
	b = 9.0 * input[2];
	c = w;
		
	result[2] = (a - b - c) / 2.0;
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

float3 FromHWB(float3 input)
{
	float3 result = { 0, 0, 0 };
	
	float h = input[0] / 360, wh = input[1] / 100, bl = input[2] / 100;
	float ratio = wh + bl;
	
	float i = 0;
	float v = 0, f = 0, n = 0;
	float r = 0, g = 0, b = 0;
	
	//wh + bl cant be > 1
	if (ratio > 1)
	{
	    wh /= ratio;
	    bl /= ratio;
	}
	
	i = floor(6 * h);
	v = 1 - bl;
	f = 6 * h - i;
	
	//If it is even...
	if (i % 2 == 0)
	{
	    f = 1 - f;
	}
	
	//Linear interpolation
	n = wh + f * (v - wh);
	
	if (i == 1)
	{
	    r = n; g = v; b = wh;
	}
	else if (i == 2)
	{
	    r = wh; g = v; b = n;
	}
	else if (i == 3)
	{
	    r = wh; g = n; b = v;
	}
	else if (i == 4)
	{
	    r = n; g = wh; b = v;
	}
	else if (i == 5)
	{
	    r = v; g = wh; b = n;
	}
	else
	{
		r = v; g = n; b = wh;
	}
	
	result[0] = r;
	result[1] = g;
	result[2] = b;
	return result;
}

float3 FromTSL(float3 input)
{          
  	float3 result = { 0, 0, 0 };
	float T = input[0], S = input[1], L = input[2];

    float x = -1 * (1.0 / tan(2.0 * 3.14 * T));

    float r_ = .0;
    float g_ = .0;

    if (T > .5)
    {
        g_ = -1 * sqrt(5.0 / (9.0 * (pow(x, 2) + 1.0)));
    }
    else if (T < .5)
    {
        g_ = sqrt(5.0 / (9.0 * (pow(x, 2) + 1.0)));
    }
    else if (T == 0)
    {
        g_ = 0;
    }

    if (T == 0)
    {
        r_ = abs(sqrt(5.0) / 3.0 * S);
    }
    else
    {
        r_ = x * g_;
    }

    float r = r_ + 1.0 / 3.0;
    float g = g_ + 1.0 / 3.0;

    float k = L / (.185 * r + .473 * g + .114);

    result[0] = k * r;
    result[1] = k * g;
    result[2] = k * (1.0 - r - g);
    return result;
}

float3 FromYCoCg(float3 input)
{
	float3 result = { 0, 0, 0 };
	float ycg = input[0] - input[2];
	result[0] = ycg + input[1];
	result[1] = input[0] + input[2];
	result[2] = ycg - input[1];
	return result;
}

float3 FromYES(float3 input)
{     
	float3 result = { 0, 0, 0 };
	result[0] = input[0] * 1 + input[1] * 1.431 + input[2] * 0.126;
	result[1] = input[0] * 1 + input[1] * -0.569 + input[2] * 0.126;
	result[2] = input[0] * 1 + input[1] * 0.431 + input[2] * -1.874;
	return result;
}

float3 FromYIQ(float3 input)
{            
	float r = 0, g = 0, b = 0;

    r = (input[0] * 1.0) + (input[1] * 0.956) + (input[2] * 0.621);
    g = (input[0] * 1.0) + (input[1] * -0.272) + (input[2] * -0.647);
    b = (input[0] * 1.0) + (input[1] * -1.108) + (input[2] * 1.705);

	r = Coerce(r, 1);
	g = Coerce(g, 1);
	b = Coerce(b, 1);
	
	float3 result = { r, g, b };
	return result;	
}

float3 FromYUV(float3 input)
{
	float3 result;
	result[0] = input[0] + 1.140 * input[2];
	result[1] = input[0] - 0.395 * input[1] - 0.581 * input[2];
	result[2] = input[0] + 2.032 * input[1];
	return result;
}

//-------------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 Color; 
	Color= tex2D( input , uv.xy); 
	
	//x, y, z = [0, 1]
	float x = uv.xy.x;
	float y = 1 - uv.xy.y;
	float z = Value;
	
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
	
	if (c == 0)
	{
		input[0] = z;
		input[1] = x;
		input[2] = y;
	}
	else if (c == 1)
	{
		input[0] = x;
		input[1] = z;
		input[2] = y;
	}
	else if (c == 2)
	{
		input[0] = x;
		input[1] = y;
		input[2] = z;
	}
	
	float3 maximums = Maximums(m);
	float3 minimums = Minimums(m);
	
	input[0] = (input[0] * (maximums[0] - minimums[0])) + minimums[0]; 
	input[1] = (input[1] * (maximums[1] - minimums[1])) + minimums[1]; 
	input[2] = (input[2] * (maximums[2] - minimums[2])) + minimums[2]; 
	
	//RGB
	if (m == 0)
	{
	    output = input;
	}
	//HCG
	else if (m == 1)
	{
	    output = FromHCG(input);
	}
	//HSB
	else if (m == 2)
	{
	    output = FromHSB(input);
	}
	//HSI
	else if (m == 3)
	{
	    output = FromHSI(input);
	}
	//HSL
	else if (m == 4)
	{
	    output = FromHSL(input);
	}
	//HSM
	else if (m == 5)
	{
	    output = FromHSM(input);
	}
	//HSP
	else if (m == 6)
	{
	    output = FromHSP(input);
	}
	//HWB
	else if (m == 7)
	{
	    output = FromHWB(input);
	}
	//TSL
	else if (m == 8)
	{
	    output = FromTSL(input);
	}
	//YCoCg
	else if (m == 9)
	{
	    output = FromYCoCg(input);
	}
	//YES
	else if (m == 10)
	{
	    output = FromYES(input);
	}
	//YIQ
	else if (m == 11)
	{
	    output = FromYIQ(input);
	}
	//YUV
	else if (m == 12)
	{
	    output = FromYUV(input);
	}
	
	Color.r = output[0];
	Color.g = output[1];
	Color.b = output[2];
	return Color; 
}