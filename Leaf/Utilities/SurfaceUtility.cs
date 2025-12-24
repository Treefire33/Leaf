using System.Numerics;
using Leaf.UI;
using Raylib_cs;

namespace Leaf.Utilities;

public static class SurfaceUtility
{
    public static void DrawSurface(Surface surface, Vector2 position, Vector2 size)
    {
        Utility.DrawRectangle(
            new Rectangle(position, size),
            surface.BorderRadius,
            surface.BorderWidth,
            surface.BackgroundColour,
            surface.BorderColour
        );
        Raylib.DrawTextureNPatch(
            surface.Texture,
            surface.NPatchInfo,
            new Rectangle(position, size),
            Vector2.Zero,
            0,
            Color.White
        );
    }
}