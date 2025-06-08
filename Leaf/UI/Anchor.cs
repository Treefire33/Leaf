using System.Numerics;

namespace Leaf.UI;

public struct Anchor
{
    public Vector2 AnchorPoint = Vector2.Zero;
    public Vector2 Offset = Vector2.Zero;

    public Anchor(Vector2 anchorPoint, Vector2 offset)
    {
        AnchorPoint = anchorPoint;
        Offset = offset;
    }

    public void Set(Vector2 anchorPoint)
    {
        AnchorPoint = anchorPoint;
    }

    public Vector2 GetAnchored()
    {
        return AnchorPoint + Offset;
    }
}


public enum AnchorPosition
{
    /// <summary>Top left of target element</summary>
    TopLeft,
    /// <summary>Top center of target element</summary>
    TopCenter,
    /// <summary>Top right of target element</summary>
    TopRight,
    /// <summary>Center left of target element</summary>
    Left,
    /// <summary>Center of target element</summary>
    Center,
    /// <summary>Center right of target element</summary>
    Right,
    /// <summary>Bottom left of target element</summary>
    BottomLeft,
    /// <summary>Bottom center of target element</summary>
    BottomCenter,
    /// <summary>Bottom right of target element</summary>
    BottomRight
}