using System.Numerics;

namespace Leaf.UI;

public struct Anchor
{
    public string AnchorPosition = "top-left";
    public Vector2 AnchorPoint = Vector2.Zero;
    public Vector2 Offset = Vector2.Zero;

    public Anchor(Vector2 anchorPoint, Vector2 offset)
    {
        AnchorPoint = anchorPoint;
        Offset = offset;
    }

    public Vector2 GetAnchored(Vector2 containerSize)
    {
        AnchorPoint = AnchorPosition switch
        {
            "top-left" => Vector2.Zero,
            "top-center" => containerSize with { X = containerSize.X / 2 },
            "top-right" => containerSize with { X = containerSize.X },
            "left" => containerSize with { Y = containerSize.Y / 2 },
            "center" => containerSize with { X = containerSize.X / 2, Y = containerSize.Y / 2 },
            "right" => containerSize with { X = containerSize.X, Y = containerSize.Y / 2 },
            "bottom-left" => containerSize with { Y = containerSize.Y },
            "bottom-center" => containerSize with { X = containerSize.X / 2, Y = containerSize.Y },
            "bottom-right" => containerSize with { X = containerSize.X, Y = containerSize.Y },
            _ => AnchorPoint
        };
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