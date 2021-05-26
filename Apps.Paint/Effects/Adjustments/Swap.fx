sampler2D input : register(s0);

float Channels : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 

	//0 BGR,
    //1 BRG,
    //2 GBR,
    //3 GRB,
    //4 RBG,

	if (Channels == 0)
	{
		color.r = color.b;
		color.g = color.g;
		color.b = color.r;
	}
	else if (Channels == 1)
	{
		color.r = color.b;
		color.g = color.r;
		color.b = color.g;
	}
	else if (Channels == 2)
	{
		color.r = color.g;
		color.g = color.b;
		color.b = color.r;
	}
	else if (Channels == 3)
	{
		color.r = color.g;
		color.g = color.r;
		color.b = color.b;
	}
	else if (Channels == 4)
	{
		color.r = color.r;
		color.g = color.b;
		color.b = color.g;
	}
	
	return color; 
}