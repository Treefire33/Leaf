using System.Numerics;
using Raylib_cs;

namespace Leaf.Utilities;

public partial class Utilities
{
    public static void FillImage(Image source, Color pixelColour)
    {
        for (int y = 0; y < source.Width; y++)
        {
            for (int x = 0; x < source.Height; x++)
            {
                Raylib.ImageDrawPixel(
                    ref source,
                    x,
                    y,
                    pixelColour
                );
            }
        }
    }

    public static Image CreateEmptyImage(Vector2 size) => CreateEmptyImage(size, new Color(0, 0, 0, 0));
    public static Image CreateEmptyImage(Vector2 size, Color pixelColour)
    {
        Image nImage = Raylib.GenImageColor((int)size.X, (int)size.Y, pixelColour);
        return nImage;
    }

    /// <summary>
    /// Applies a blend mode to an image.
    /// Changes the source image.
    /// </summary>
    /// <param name="source">The source image to be blended.</param>
    /// <param name="blend">The image to blend with.</param>
    /// <param name="blendMode">The blend mode.</param>
    public static unsafe void BlendImage(ref Image source, Image blend, BlendMode blendMode = BlendMode.Multiply)
    {
        Color* srcPixels = Raylib.LoadImageColors(source);
        Color* blendPixels = Raylib.LoadImageColors(blend);
        for (int i = 0; i < source.Width * source.Height; i++)
        {
            srcPixels[i] = blendMode switch
            {
                BlendMode.Multiply => RgbaBlendMultiply(srcPixels[i], blendPixels[i]),
                BlendMode.Additive => RgbaBlendAdditive(srcPixels[i], blendPixels[i]),
                BlendMode.Subtract => RgbaBlendSubtract(srcPixels[i], blendPixels[i]),
                BlendMode.Divide => RgbaBlendDivide(srcPixels[i], blendPixels[i]),
                BlendMode.Min => RgbaBlendMin(srcPixels[i], blendPixels[i]),
                BlendMode.Max => RgbaBlendMax(srcPixels[i], blendPixels[i]),
                BlendMode.Screen => RgbaBlendScreen(srcPixels[i], blendPixels[i]),
                BlendMode.Overlay => RgbaBlendOverlay(srcPixels[i], blendPixels[i]),
                BlendMode.ColourDodge => RgbaBlendColourDodge(srcPixels[i], blendPixels[i]),
                BlendMode.ColourBurn => RgbaBlendColourBurn(srcPixels[i], blendPixels[i]),
                BlendMode.Difference => RgbaBlendDifference(srcPixels[i], blendPixels[i]),
                BlendMode.Negation => RgbaBlendNegation(srcPixels[i], blendPixels[i]),
                
                BlendMode.RgbMultiply => RgbBlendMultiply(srcPixels[i], blendPixels[i]),
                BlendMode.RgbAdditive => RgbBlendAdditive(srcPixels[i], blendPixels[i]),
                BlendMode.RgbSubtract => RgbBlendSubtract(srcPixels[i], blendPixels[i]),
                BlendMode.RgbDivide => RgbBlendDivide(srcPixels[i], blendPixels[i]),
                BlendMode.RgbMin => RgbBlendMin(srcPixels[i], blendPixels[i]),
                BlendMode.RgbMax => RgbBlendMax(srcPixels[i], blendPixels[i]),
                BlendMode.RgbScreen => RgbBlendScreen(srcPixels[i], blendPixels[i]),
                BlendMode.RgbOverlay => RgbBlendOverlay(srcPixels[i], blendPixels[i]),
                BlendMode.RgbColourDodge => RgbBlendColourDodge(srcPixels[i], blendPixels[i]),
                BlendMode.RgbColourBurn => RgbBlendColourBurn(srcPixels[i], blendPixels[i]),
                BlendMode.RgbDifference => RgbBlendDifference(srcPixels[i], blendPixels[i]),
                BlendMode.RgbNegation => RgbBlendNegation(srcPixels[i], blendPixels[i])
            };
        }
        for (int i = 0; i < source.Width; i++)
        {
            for (int j = 0; j < source.Height; j++)
            {
                Raylib.ImageDrawPixel(
                    ref source,
                    i, j,
                    srcPixels[j * source.Width + i]
                );
            }
        }
        Raylib.UnloadImageColors(srcPixels);
        Raylib.UnloadImageColors(blendPixels);
    }

    public static void BlendImage(ref Texture2D source, Image blend, BlendMode blendMode = BlendMode.Multiply)
    {
        Image textureImage = Raylib.LoadImageFromTexture(source);
        BlendImage(ref textureImage, blend, blendMode);
        Raylib.UnloadTexture(source);
        source = Raylib.LoadTextureFromImage(textureImage);
    }

    public static Image BlendImage(Image source, Image blend, BlendMode blendMode = BlendMode.Multiply)
    {
        // This function just copies the source image.
        Image sourceCopy = Raylib.ImageCopy(source);
        BlendImage(ref sourceCopy, blend, blendMode);
        return sourceCopy;
    }
    
    public static Texture2D BlendImage(Texture2D source, Image blend, BlendMode blendMode = BlendMode.Multiply)
    {
        // This function just copies the source texture.
        Image textureImage = Raylib.LoadImageFromTexture(source);
        Texture2D copy = Raylib.LoadTextureFromImage(textureImage);
        Raylib.UnloadImage(textureImage);
        BlendImage(ref copy, blend, blendMode);
        return copy;
    }

    public static void DrawTextOnTexture(ref Texture2D source, Vector2 position, string text, int fontSize, Color color)
    {
        Image textureImage = Raylib.LoadImageFromTexture(source);
        Raylib.ImageDrawText(
            ref textureImage,
            text,
            (int)position.X,
            (int)position.Y,
            fontSize,
            color
        );
        Raylib.UnloadTexture(source);
        source = Raylib.LoadTextureFromImage(textureImage);
        Raylib.UnloadImage(textureImage);
    }
}

public enum BlendMode
{
    Multiply,
    Additive,
    Subtract,
    Divide,
    Min,
    Max,
    Screen,
    Overlay,
    ColourDodge,
    ColourBurn,
    Difference,
    Negation,
    
    RgbMultiply,
    RgbAdditive,
    RgbSubtract,
    RgbDivide,
    RgbMin,
    RgbMax,
    RgbScreen,
    RgbOverlay,
    RgbColourDodge,
    RgbColourBurn,
    RgbDifference,
    RgbNegation
}