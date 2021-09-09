sampler2D input : register(s0);

float4 Color : register(C0);
float Blend : register(C1);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	if (color.a > 0)
	{
		color.r = Color.r;
		color.g = Color.g;
		color.b = Color.b;
	}
	return color; 
}