/// <class>ReplaceEffect</class>

/// <description>An effect that makes pixels of a particular color another color.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The color that becomes transparent.</summary>
/// <defaultValue>Green</defaultValue>
float4 Color1 : register(C0);
float4 Color2 : register(C1);

/// <summary>The tolerance in color differences.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.3</defaultValue>
float Tolerance : register(C2);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 color = tex2D(Texture1Sampler, uv);
   
   float4 color1 = Color1;
   float4 color2 = Color2;
   
   if (all(abs(color.rgb - color1.rgb) < Tolerance)) 
      color.rgba = color2.rgba;

   return color;
}