sampler2D input : register(s0);

float4 Color : register(C0);
float Alpha : register(C1);
float Mode : register(C2);

#define BlendColorDodgef(base, blend)	((blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0))
#define BlendColorBurnf(base, blend)	((blend == 0.0) ? blend : max((1.0 - ((1.0 - base) / blend)), 0.0))
#define BlendNegation(base, blend)	(float3(1.0, 1.0, 1.0) - abs(float3(1.0, 1.0, 1.0) - base - blend))
#define BlendPhoenix(base, blend)	(min(base, blend) - max(base, blend) + float3(1.0, 1.0, 1.0))
#define BlendReflectf(base, blend)	((blend == 1.0) ? blend : min(base * base / (1.0 - blend), 1.0))
#define BlendSoftLightf(base, blend)	((blend < 0.5) ? (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) : (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend)))
#define BlendVividLightf(base, blend)	((blend < 0.5) ? BlendColorBurnf(base, (2.0 * blend)) : BlendColorDodgef(base, (2.0 * (blend - 0.5))))
#define BlendHardMixf(base, blend)		((BlendVividLightf(base, blend) < 0.5) ? 0.0 : 1.0)

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

float HueToRGB(float temp1, float temp2, float temp3)
{
	if (temp3 < 0.0)
		temp3 += 1.0;
	else if (temp3 > 1.0)
		temp3 -= 1.0;

	float rgbComponent;

	if ((6.0 * temp3) < 1.0)
		rgbComponent = temp1 + (temp2 - temp1) * 6.0 * temp3;
	else if ((2.0 * temp3) < 1.0)
		rgbComponent = temp2;
	else if ((3.0 * temp3) < 2.0)
		rgbComponent = temp1 + (temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0;
	else
		rgbComponent = temp1;

	return rgbComponent;
}

float3 HSLToRGB(float3 hsl)
{
	float3 rgb;

	if (hsl.y == 0.0)
	{
		rgb = float3(hsl.z, hsl.z, hsl.z);
	}
	else
	{
		float temp2;
		if (hsl.z < 0.5)
			temp2 = hsl.z * (1.0 + hsl.y);
		else
			temp2 = (hsl.y + hsl.z) - (hsl.y * hsl.z);

		float temp1 = 2.0 * hsl.z - temp2;

		rgb.r = HueToRGB(temp1, temp2, hsl.x + (1.0 / 3.0));
		rgb.g = HueToRGB(temp1, temp2, hsl.x);
		rgb.b = HueToRGB(temp1, temp2, hsl.x - (1.0 / 3.0));
	}

	return rgb;
}

float3 BlendHue(float3 base, float3 blend)
{
	float3 baseHSL = RGBToHSL(base);
	return HSLToRGB(float3(RGBToHSL(blend).x, baseHSL.y, baseHSL.z));
}

float3 BlendLuminosity(float3 base, float3 blend)
{
	float3 baseHSL = RGBToHSL(base);
	return HSLToRGB(float3(baseHSL.x, baseHSL.y, RGBToHSL(blend).z));
}

float3 BlendSaturation(float3 base, float3 blend)
{
	float3 baseHSL = RGBToHSL(base);
	return HSLToRGB(float3(baseHSL.x, RGBToHSL(blend).y, baseHSL.z));
}

float4 Blend(float4 inputColor, float4 blendColor)
{
	blendColor.a = Alpha == 0 ? 0.001 : Alpha;
	blendColor.r = blendColor.r == 0 ? 0.001 : blendColor.r;
	blendColor.g = blendColor.g == 0 ? 0.001 : blendColor.g;
	blendColor.b = blendColor.b == 0 ? 0.001 : blendColor.b;
	
	float4 result;
	result.a = inputColor.a;

	//Average
	if (Mode == 0)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.rgb = (inputColor.rgb + blendColor.rgb) / 2;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Color burn
	else if (Mode == 1)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = 1 - (1-Base) / Blend
		result.rgb = clamp(1 - (1 - inputColor.rgb) / blendColor.rgb, 0, 1);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Color dodge
	else if (Mode == 2)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = Base / (1-Blend)
		result.r = BlendColorDodgef(inputColor.r, blendColor.r);
		result.g = BlendColorDodgef(inputColor.g, blendColor.g);
		result.b = BlendColorDodgef(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Color
	else if (Mode == 3)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		float3 blendHSL = RGBToHSL(blendColor.rgb);
		result.rgb = HSLToRGB(float3(blendHSL.r, blendHSL.g, RGBToHSL(inputColor.rgb).b));

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Darken
	else if (Mode == 4)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		if (inputColor.r > blendColor.r)
		{
			result.r = blendColor.r;
		}
		else result.r = inputColor.r;

		if (inputColor.g > blendColor.g)
		{
			result.g = blendColor.g;
		}
		else
			result.g = inputColor.g;

		if (inputColor.b > blendColor.b)
			result.b = blendColor.b;
		else result.b = inputColor.b;
			
		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Difference
	else if (Mode == 5)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = | Base - Blend |
		result.rgb = abs(inputColor.rgb - blendColor.rgb);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Exclusion
	else if (Mode == 6)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = ½ - 2×(Base-½)×(Blend-½) 
		result.rgb = 0.5 - 2 * (inputColor.rgb - 0.5) * (blendColor.rgb - 0.5);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Glow
	else if (Mode == 7)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.r = BlendReflectf(blendColor.r, inputColor.r);
		result.g = BlendReflectf(blendColor.g, inputColor.g);
		result.b = BlendReflectf(blendColor.b, inputColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Hard light
	else if (Mode == 8)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// if (Blend > ½) R = 1 - (1-Base) × (1-2×(Blend-½))
		// if (Blend <= ½) R = Base × (2×Blend)
		if (blendColor.r > 0.5)
		{
			result.r = 1 - (1 - inputColor.r) * (1 - 2 * (blendColor.r - 0.5));
		}
		else result.r = inputColor.r * (2 * blendColor.r);

		if (blendColor.g > 0.5)
		{
			result.g = 1 - (1 - inputColor.g) * (1 - 2 * (blendColor.g - 0.5));
		}
		else result.g = inputColor.g * (2 * blendColor.g);

		if (blendColor.b > 0.5)
		{
			result.b = 1 - (1 - inputColor.b) * (1 - 2 * (blendColor.b - 0.5));
		}
		else result.b = inputColor.b * (2 * blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Hard mix
	else if (Mode == 9)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.r = BlendHardMixf(inputColor.r, blendColor.r);
		result.g = BlendHardMixf(inputColor.g, blendColor.g);
		result.b = BlendHardMixf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Hue
	else if (Mode == 10)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.rgb = BlendHue(inputColor.rgb, blendColor.rgb);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Lighten
	else if (Mode == 11)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		if (inputColor.r < blendColor.r)
		{
			result.r = blendColor.r;
		}
		else result.r = inputColor.r;

		if (inputColor.g < blendColor.g)
		{
			result.g = blendColor.g;
		}
		else result.g = inputColor.g;

		if (inputColor.b < blendColor.b)
		{
			result.b = blendColor.b;
		}
		else result.b = inputColor.b;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Linear burn
	else if (Mode == 12)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = Base + Blend - 1
		result.rgb = clamp(inputColor.rgb + blendColor.rgb - 1, 0, 1);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Linear dodge
	else if (Mode == 13)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = Base + Blend
		result.rgb = inputColor.rgb + blendColor.rgb;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Linear light
	else if (Mode == 14)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// if (Blend > ½) R = Base + 2×(Blend-½)
		// if (Blend <= ½) R = Base + 2×Blend - 1
		if (blendColor.r > 0.5)
		{
			result.r = inputColor.r + 2 * (blendColor.r - 0.5);
		}
		else result.r = inputColor.r + 2 * blendColor.r - 1;

		if (blendColor.g > 0.5)
		{
			result.g = inputColor.g + 2 * (blendColor.g - 0.5);
		}
		else result.g = inputColor.g + 2 * blendColor.g - 1;

		if (blendColor.b > 0.5)
		{
			result.b = inputColor.b + 2 * (blendColor.b - 0.5);
		}
		else result.b = inputColor.b + 2 * blendColor.b - 1;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Luminosity
	else if (Mode == 15)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.rgb = BlendLuminosity(inputColor.rgb, blendColor.rgb);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Multiply
	else if (Mode == 16)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = Base * Blend
		result.rgb = blendColor.rgb * inputColor.rgb;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}	
	//Negation
	else if (Mode == 17)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.rgb = BlendNegation(inputColor, blendColor);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Normal
	//else if (Mode == 18)
	//{
		//result.rgb = (1 - blendColor.a) * inputColor.rgb + blendColor.rgb;
	//}
	//Overlay
	else if (Mode == 19)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// if (Base > ½) R = 1 - (1-2×(Base-½)) × (1-Blend)
		// if (Base <= ½) R = (2×Base) × Blend

		if (inputColor.r > 0.5)
		{
			result.r = 1 - (1 - 2 * (inputColor.r - 0.5)) * (1 - blendColor.r);
		}
		else result.r = (2 * inputColor.r) * blendColor.r;

		if (inputColor.g > 0.5)
		{
			result.g = 1 - (1 - 2 * (inputColor.g - 0.5)) * (1 - blendColor.g);
		}
		else result.g = (2 * inputColor.g) * blendColor.g;

		if (inputColor.b > 0.5)
		{
			result.b = 1 - (1 - 2 * (inputColor.b - 0.5)) * (1 - blendColor.b);
		}
		else result.b = (2 * inputColor.b) * blendColor.b;

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Phoenix
	else if (Mode == 20)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.rgb = BlendPhoenix(inputColor, blendColor);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Pin light
	else if (Mode == 21)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// if (Blend > ½) R = max(Base,2×(Blend-½))
		// if (Blend <= ½) R = min(Base,2×Blend))
		if (blendColor.r > 0.5)
		{
			result.r = max(inputColor.r, 2 * (blendColor.r - 0.5));
		}
		else result.r = min(inputColor.r, 2 * blendColor.r);

		if (blendColor.g > 0.5)
		{
			result.g = max(inputColor.g, 2 * (blendColor.g - 0.5));
		}
		else result.g = min(inputColor.g, 2 * blendColor.g);

		if (blendColor.b > 0.5)
		{
			result.b = max(inputColor.b, 2 * (blendColor.b - 0.5));
		}			
		else result.b = min(inputColor.b, 2 * blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Reflect
	else if (Mode == 22)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.r = BlendReflectf(inputColor.r, blendColor.r);
		result.g = BlendReflectf(inputColor.g, blendColor.g);
		result.b = BlendReflectf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Saturation
	else if (Mode == 23)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.rgb = BlendSaturation(inputColor.rgb, blendColor.rgb);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Screen
	else if (Mode == 24)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		// R = 1 - (1-Base) × (1-Blend)
		result.rgb = 1 - (1 - inputColor.rgb) * (1 - blendColor.rgb);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Soft light
	else if (Mode == 25)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.r = BlendSoftLightf(inputColor.r, blendColor.r);
		result.g = BlendSoftLightf(inputColor.g, blendColor.g);
		result.b = BlendSoftLightf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	//Vivid light
	else if (Mode == 26)
	{
		// un-premultiply the blendColor alpha out from blendColor
		blendColor.rgb = clamp(blendColor.rgb / blendColor.a, 0, 1);

		// apply the blend mode math
		result.r = BlendVividLightf(inputColor.r, blendColor.r);
		result.g = BlendVividLightf(inputColor.g, blendColor.g);
		result.b = BlendVividLightf(inputColor.b, blendColor.b);

		// re-multiply the blendColor alpha in to blendColor, weight inputColor according to blendColor.a
		result.rgb = (1 - blendColor.a) * inputColor.rgb + result.rgb * blendColor.a;
	}
	return result;
}

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	return Blend(color, Color);
}