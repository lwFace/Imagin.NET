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
	float4 color = tex2D( input , uv.xy); 

    float or = color.r * 255, og = color.g * 255, ob = color.b * 255;
    float nr = or, ng = og, nb = ob;

    nr = Red > or
        ? Red - or
        : or - Red;
    ng = Green > og
        ? Green - og
        : og - Green;
    nb = Blue > ob
        ? Blue - ob
        : ob - Blue;

	color.r = Coerce(nr / 255, 1);
	color.g = Coerce(ng / 255, 1);
	color.b = Coerce(nb / 255, 1);
	return color;
}