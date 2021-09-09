sampler2D input : register(s0);

float X : register(C0);
float Y : register(C1);
float Z : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
	return color; 
}