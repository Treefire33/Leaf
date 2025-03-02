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
            Raylib.DrawRectangleRec(
                new UIRect(GetPosition(), RelativeRect.Size),
                Color.DarkBlue
            );
            Raylib.BeginScissorMode((int)GetPosition().X, (int)GetPosition().Y, (int)RelativeRect.Width, (int)RelativeRect.Height);
                foreach (UIElement element in Elements.Where(e => e.Visible))
                {
                    element.Anchor.Offset = GetPosition();
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

    public void AddElement(UIElement element)
    {
        element.Container?.RemoveElement(element);
        element.RelativeRect.Position = GetPosition();
        Elements.Add(element);
        Elements = Elements.OrderBy((x) => x.Layer).ToList();
    }

    public void RemoveElement(UIElement element)
    {
        Elements.Remove(element);
    }
}