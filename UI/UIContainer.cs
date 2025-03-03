using System.Numerics;
using Cattail.UI.Interfaces;
using Cattail.UI.Theming;
using Raylib_cs;

namespace Cattail.UI;

public class UIContainer : UIElement, IUIContainer
{
    public List<UIElement> Elements = [];
    public UIContainer(
        UIRect posScale,
        bool visible = true,
        UIContainer? container = null,
        ObjectID id = default,
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        bool isRootContainer = false
    ) : base(posScale, visible, container, new ObjectID(id.ID ?? "default", id.Class ?? "@container"), anchor, origin, isRootContainer: isRootContainer)
    {
        
    }

    public override void Update()
    {
        if (Visible)
        {
            if (UIManager.DebugMode)
            {
                Raylib.DrawRectangleRec(
                    new UIRect(GetPosition(), RelativeRect.Size),
                    Color.DarkBlue
                );
                Raylib.DrawRectangleLinesEx(
                    new UIRect(GetPosition(), RelativeRect.Size),
                    10,
                    Color.Black
                );
            }

            Raylib.BeginScissorMode((int)GetPosition().X, (int)GetPosition().Y, (int)RelativeRect.Width, (int)RelativeRect.Height);
                foreach (UIElement element in Elements.Where(e => e.Visible))
                {
                    element.Update();
                }
            Raylib.EndScissorMode();
        }
    }

    public override void Kill()
    {
        foreach (UIElement element in Elements)
        {
            element.Kill();
        }
        base.Kill();
    }

    public virtual void AddElement(UIElement element)
    {
        element.Container?.RemoveElement(element);
        Elements.Add(element);
        var tempAnchor = element.Anchor;
        element.SetAnchor("top-left", Vector2.Zero);
        element.SetAnchor(tempAnchor.AnchorPosition, tempAnchor.Offset);
        element.Container = this;
        Elements = Elements.OrderBy((x) => x.Layer).ToList();
    }

    public virtual void RemoveElement(UIElement element)
    {
        Elements.Remove(element);
    }
}