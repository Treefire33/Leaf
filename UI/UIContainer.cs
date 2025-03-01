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
        bool isRootContainer = false
    ) : base(posScale, visible, container, new ObjectID(id.ID ?? "default", id.Class ?? "@container"), isRootContainer: isRootContainer)
    {
        
    }

    public override void Update()
    {
        if (Visible)
        {
            Raylib.BeginScissorMode((int)GetPosition().X, (int)GetPosition().Y, (int)RelativeRect.Width, (int)RelativeRect.Height);
                foreach (IUIElement element in Elements.Where(e => e.Visible))
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

    public void AddElement(UIElement element)
    {
        Elements.Add(element);
        Elements = Elements.OrderBy((x) => x.Layer).ToList();
    }

    public void RemoveElement(UIElement element)
    {
        Elements.Remove(element);
    }
}