using System.Numerics;
using Leaf.UI.Interfaces;
using Leaf.Utilities;
using Raylib_cs;

namespace Leaf.UI;

public class UIPanel : UIElement
{
    private Surface _surface;

    public UIPanel(
        UIRect posScale,
        bool visible = true,
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, classes, "panel", anchor, origin, tooltip)
    {
        _surface = new Surface($"#{id}", classes, "panel");
    }

    public override void Update()
    {
        base.Update();
        SurfaceUtility.DrawSurface(_surface, GetPosition(), RelativeRect.Size);
    }
}