using System.Numerics;
using Leaf.UI.Interfaces;
using Raylib_cs;

namespace Leaf.UI;

public class UIPanel : UIElement
{
    public UIPanel(
        UIRect posScale,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, @class, "panel", anchor, origin, tooltip)
    { }

    public override void Update()
    {
        base.Update();
        Utility.DrawRectangle(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _borderRadius,
            _borderThickness,
            _backgroundColour,
            _borderColour
        );
    }
}