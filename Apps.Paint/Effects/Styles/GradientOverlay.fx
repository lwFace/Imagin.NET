sampler2D input : register(s0);

float4 Color1 : register(C0);
float4 Color2 : register(C1);
float4 Color3 : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color= tex2D(input, uv.xy); 
	
    float a1 = 1; //Color1.a;
    float r1 = 1; //Color1.r;
    float g1 = 0; //Color1.g;
    float b1 = 0; //Color1.b;

	float y = uv.y;
	uv.y = sin(45) + uv.x;
	uv.x = cos(45) + y;
	
    float a2 = uv.x;
    float r2 = 1; //Color2.r;
    float g2 = 1; //Color2.g;
    float b2 = 1; //Color2.b;

    float a = 1.0 - (1.0 - a2) * (1.0 - a1);
    float r = r2 * a2 / a + r1 * a1 * (1.0 - a2) / a;
    float g = g2 * a2 / a + g1 * a1 * (1.0 - a2) / a;
    float b = b2 * a2 / a + b1 * a1 * (1.0 - a2) / a;
    
    color.a = a;
    color.r = r;
    color.g = g;
    color.b = b;
	return color; 
}