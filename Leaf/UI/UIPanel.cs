using System.Numerics;
using Leaf.UI.Interfaces;
using Raylib_cs;

namespace Leaf.UI;

public class UIPanel : UIElement
{
    private Color _backgroundColor;
    private Color _outlineColour;
    private float _outlineThickness;
    
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
    {
        ThemeElement();
    }

    public override void ThemeElement()
    {
        base.ThemeElement();
        _backgroundColor = Theme.GetProperty("background-color").AsColor();
        _outlineColour = Theme.GetProperty("border-top-color").AsColor();
        _outlineThickness = Theme.GetProperty("border-top-width").AsFloat();
    }

    public override void Update()
    {
        base.Update();
        Raylib.DrawRectangleRec(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _backgroundColor
        );
        Raylib.DrawRectangleLinesEx(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _outlineThickness,
            _outlineColour
        );
    }
}