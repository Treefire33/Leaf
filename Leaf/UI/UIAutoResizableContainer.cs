using System.Numerics;
using Leaf.UI.Interfaces;

namespace Leaf.UI;

public class UIAutoResizableContainer : UIContainer
{
    private readonly bool _scaleLeft;
    private readonly bool _scaleRight;
    private readonly bool _scaleTop;
    private readonly bool _scaleBottom;
    
    public UIAutoResizableContainer(
        UIRect posScale,
        bool scaleLeft = false,
        bool scaleRight = true,
        bool scaleTop = false,
        bool scaleBottom = true,
        bool visible = true,
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, @class, anchor, origin, tooltip)
    {
        _scaleLeft = scaleLeft;
        _scaleRight = scaleRight;
        _scaleTop = scaleTop;
        _scaleBottom = scaleBottom;
    }

    public override void AddElement(UIElement element)
    {
        base.AddElement(element);
        RecalculateBounds();
    }

    public override void RemoveElement(UIElement element)
    {
        base.RemoveElement(element);
        RecalculateBounds();
    }

    private void RecalculateBounds()
    {
        UIRect bounds = new(float.MaxValue, float.MaxValue, 0, 0);
        foreach (var element in Elements)
        {
            bounds.X = element.GetPosition().X < bounds.X ? element.GetPosition().X : bounds.X;
            bounds.Y = element.GetPosition().Y < bounds.Y ? element.GetPosition().Y : bounds.Y;
            if (element.RelativeRect.X + element.RelativeRect.Width > bounds.Width)
            {
                bounds.Width = element.RelativeRect.X + element.RelativeRect.Width;
            }
            if (element.RelativeRect.Y + element.RelativeRect.Height > bounds.Height)
            {
                bounds.Height = element.RelativeRect.Y + element.RelativeRect.Height;
            }
        }

        RelativeRect = RelativeRect with
        {
            X = _scaleLeft ? bounds.X : RelativeRect.X,
            Y = _scaleTop ? bounds.Y : RelativeRect.Y,
            Width = _scaleRight ? bounds.Width : RelativeRect.Width,
            Height = _scaleBottom ? bounds.Height : RelativeRect.Height
        };
    }
}