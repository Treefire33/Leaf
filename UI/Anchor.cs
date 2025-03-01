using System.Numerics;

namespace Cattail.UI;

public struct Anchor
{
    public Vector2 AnchorPoint = Vector2.Zero;
    public Vector2 Offset = Vector2.Zero;

    public Anchor(Vector2 anchorPoint, Vector2 offset)
    {
        AnchorPoint = anchorPoint;
        Offset = offset;
    }

    public Vector2 GetAnchored()
    {
        return AnchorPoint + Offset;
    }
}


public enum AnchorFlags
{
    TopLeft,
    TopCenter,
    TopRight,
    Left,
    Center,
    Right,
    BottomLeft,
    BottomCenter,
    BottomRight,
}