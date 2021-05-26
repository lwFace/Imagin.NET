sampler2D input : register(s0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv.xy); 
    float r = color.r * 255, g = color.r * 255, b = color.b * 255;
    
    float nr = (r * 0.393) + (g * 0.769) + (b * 0.189);
    float ng = (r * 0.349) + (g * 0.686) + (b * 0.168);
    float nb = (r * 0.272) + (g * 0.534) + (b * 0.131);
    
    color.r = nr / 255;
    color.g = ng / 255;
    color.b = nb / 255;
	return color; 
}