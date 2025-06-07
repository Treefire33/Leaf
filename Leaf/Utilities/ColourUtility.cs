using System.Formats.Asn1;
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

    public static Color RgbaBlendScreen(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return (c1.Invert() * c2.Invert()).Invert();
    }

    public static Color RgbaBlendOverlay(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return new Colour01(
            OverlayChannel(c1.R, c2.R),
            OverlayChannel(c1.G, c2.G),
            OverlayChannel(c1.B, c2.B),
            OverlayChannel(c1.A, c2.A)
        );
        
        float OverlayChannel(float baseChannel, float blendChannel)
        {
            if (baseChannel < 0.5f)
            {
                return 2 * baseChannel * blendChannel;
            }
            
            return 1 - 2 * (1 - baseChannel) * (1 - blendChannel);
        }
    }

    public static Color RgbaBlendColourDodge(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return c1 / c2.Invert();
    }
    
    public static Color RgbaBlendColourBurn(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return (c1.Invert() / c2).Invert();
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
            Math.Min(c1.R, c2.R),
            Math.Min(c1.G, c2.G),
            Math.Min(c1.B, c2.B),
            Math.Min(c1.A, c2.A)
        );
    }
    
    public static Color RgbaBlendMax(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return new Colour01(
            Math.Max(c1.R, c2.R),
            Math.Max(c1.G, c2.G),
            Math.Max(c1.B, c2.B),
            Math.Max(c1.A, c2.A)
        );
    }
    
    public static Color RgbaBlendDifference(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return new Colour01(
            DifferenceChannel(c1.R, c2.R),
            DifferenceChannel(c1.G, c2.G),
            DifferenceChannel(c1.B, c2.B),
            DifferenceChannel(c1.A, c2.A)
        );

        float DifferenceChannel(float baseChannel, float blendChannel)
        {
            float diff = baseChannel - blendChannel;
            return Math.Abs(diff);
        }
    }
    
    // based on paint.NET's negation blend mode.
    public static Color RgbaBlendNegation(Color baseColour, Color blendColour)
    {
        Colour01 c1 = baseColour;
        Colour01 c2 = blendColour;
        return new Colour01(
            NegateChannel(c1.R, c2.R),
            NegateChannel(c1.G, c2.G),
            NegateChannel(c1.B, c2.B),
            NegateChannel(c1.A, c2.A)
        );

        float NegateChannel(float baseChannel, float blendChannel)
        {
            float diff = Math.Abs(1 - blendChannel - baseChannel);
            return 1 - diff;
        }
    }
    
    // RGB blend modes. Ignores the alpha channel, taking the base alpha over the blend.
    // To be improved.
    public static Color RgbBlendMultiply(Color baseColour, Color blendColour) =>
        RgbaBlendMultiply(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendScreen(Color baseColour, Color blendColour) =>
        RgbaBlendScreen(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendOverlay(Color baseColour, Color blendColour) =>
        RgbaBlendOverlay(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendColourDodge(Color baseColour, Color blendColour) =>
        RgbaBlendColourDodge(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendColourBurn(Color baseColour, Color blendColour) =>
        RgbaBlendColourBurn(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendAdditive(Color baseColour, Color blendColour) =>
        RgbaBlendAdditive(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendSubtract(Color baseColour, Color blendColour) =>
        RgbaBlendSubtract(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendDivide(Color baseColour, Color blendColour) =>
        RgbaBlendDivide(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendMin(Color baseColour, Color blendColour) =>
        RgbaBlendMin(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendMax(Color baseColour, Color blendColour) =>
        RgbaBlendMax(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendDifference(Color baseColour, Color blendColour) =>
        RgbaBlendDifference(baseColour, blendColour) with { A = baseColour.A };
    public static Color RgbBlendNegation(Color baseColour, Color blendColour) =>
        RgbaBlendNegation(baseColour, blendColour) with { A = baseColour.A };
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
            R = Math.Max(1 - R, 0),
            G = Math.Max(1 - G, 0),
            B = Math.Max(1 - B, 0),
            A = Math.Max(1 - A, 0),
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
            R = Math.Min(a.R + b.R, 1), 
            G = Math.Min(a.G + b.G, 1), 
            B = Math.Min(a.B + b.B, 1), 
            A = Math.Min(a.A + b.A, 1)
        };
    }
    
    public static Colour01 operator -(Colour01 a, Colour01 b)
    {
        return new Colour01
        {
            R = Math.Max(a.R - b.R, 0), 
            G = Math.Max(a.G - b.G, 0),
            B = Math.Max(a.B- b.B, 0), 
            A = Math.Max(a.A - b.A, 0),
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
            R = b.R > 0 ? a.R / b.R : 0, 
            G = b.G > 0 ? a.G / b.G : 0, 
            B = b.B > 0 ? a.B / b.B : 0,  
            A = b.A > 0 ? a.A / b.A : 0, 
        };
    }
}