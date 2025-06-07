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
                BlendMode.Add => RgbaBlendAdditive(srcPixels[i], blendPixels[i]),
                BlendMode.Subtract => RgbaBlendSubtract(srcPixels[i], blendPixels[i]),
                BlendMode.Divide => RgbaBlendDivide(srcPixels[i], blendPixels[i]),
                BlendMode.Min => RgbaBlendMin(srcPixels[i], blendPixels[i]),
                BlendMode.Max => RgbaBlendMax(srcPixels[i], blendPixels[i])
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
}

public enum BlendMode
{
    Multiply,
    Add,
    Subtract,
    Divide,
    Min,
    Max
}