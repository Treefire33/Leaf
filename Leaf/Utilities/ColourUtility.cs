using Raylib_cs;

namespace Leaf.Utilities;

public partial class Utilities
{
    public static Color RgbaBlendMultiply(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return c1 * c2;
    }

    public static Color RgbaBlendAdditive(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return c1 + c2;
    }
    
    public static Color RgbaBlendSubtract(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return c1 - c2;
    }
    
    public static Color RgbaBlendDivide(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return c1 / c2;
    }
    
    public static Color RgbaBlendMin(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return new Colour01(
            (int)Math.Min(c1.R, c2.R),
            (int)Math.Min(c1.G, c2.G),
            (int)Math.Min(c1.B, c2.B),
            (int)Math.Min(c1.A, c2.A)
        );
    }
    
    public static Color RgbaBlendMax(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return new Colour01(
            (int)Math.Max(c1.R, c2.R),
            (int)Math.Max(c1.G, c2.G),
            (int)Math.Max(c1.B, c2.B),
            (int)Math.Max(c1.A, c2.A)
        );
    }
}

/// <summary>
/// Custom struct to make colour channel manipulation easier.
/// </summary>
internal struct Colour01(float r, float g, float b, float a)
{
    public float R = r;
    public float G = g;
    public float B = b;
    public float A = a;

    public Colour01 Invert()
    {
        return new Colour01
        {
            R = 1 - R, 
            G = 1 - G, 
            B = 1 - B, 
            A = 1 - A
        };
    }
    
    public static implicit operator Colour01(Color c)
    {
        return new Colour01
        {
            R = c.R / 255.0f, 
            G = c.G / 255.0f, 
            B = c.B / 255.0f,
            A = c.A / 255.0f
        };
    }

    public static implicit operator Color(Colour01 c)
    {
        return new Color(
            (int)Math.Min(c.R * 255.0f, 255),
            (int)Math.Min(c.G * 255.0f, 255),
            (int)Math.Min(c.B * 255.0f, 255),
            (int)Math.Min(c.A * 255.0f, 255)
        );
    }
    
    public static Colour01 operator +(Colour01 a, Colour01 b)
    {
        return new Colour01
        {
            R = a.R + b.R, 
            G = a.G + b.G, 
            B = a.B + b.B, 
            A = a.A + b.A
        };
    }
    
    public static Colour01 operator -(Colour01 a, Colour01 b)
    {
        return new Colour01
        {
            R = a.R - b.R, 
            G = a.G - b.G, 
            B = a.B - b.B, 
            A = a.A - b.A
        };
    }
    
    public static Colour01 operator *(Colour01 a, Colour01 b)
    {
        return new Colour01
        {
            R = a.R * b.R, 
            G = a.G * b.G, 
            B = a.B * b.B, 
            A = a.A * b.A
        };
    }
    
    public static Colour01 operator /(Colour01 a, Colour01 b)
    {
        return new Colour01
        {
            R = a.R / b.R, 
            G = a.G / b.G, 
            B = a.B / b.B, 
            A = a.A / b.A
        };
    }
}