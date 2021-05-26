sampler2D input : register(s0);

float SampleI : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 Color; 
	Color= tex2D( input , uv.xy); 

    int or = color.R, og = color.G, ob = color.B;
    int ir = Imagin.Common.Random.Current.Next(-Amount, Amount + 1), ig = 0, ib = 0;

    if (monochromatic)
    {
        ig = ir;
        ib = ir;
    }
    else
    {
        ig = Imagin.Common.Random.Current.Next(-Amount, Amount + 1);
        ib = Imagin.Common.Random.Current.Next(-Amount, Amount + 1);
    }

    byte
        nr = (or + ir).Coerce(255).Byte(),
        ng = (og + ig).Coerce(255).Byte(),
        nb = (ob + ib).Coerce(255).Byte();

    return Color.FromArgb(color.A, nr, ng, nb);
}