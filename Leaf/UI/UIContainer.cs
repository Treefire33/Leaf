using System.Numerics;
using Leaf.Events;
using Leaf.UI.Interfaces;
using Raylib_cs;

namespace Leaf.UI;

public class UIContainer : UIElement, IUIContainer
{
    public List<UIElement> Elements = [];
    public UIContainer(
        UIRect posScale,
        bool visible = true,
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null,
        bool isRootContainer = false
    ) : base(posScale, visible, container, id, classes, "container", anchor, origin, tooltip:tooltip, isRootContainer: isRootContainer)
    {
        
    }

    public override void Update()
    {
        Elements = Elements.OrderBy(x => x.Layer).ToList();
        if (Visible)
        {
            Raylib.BeginScissorMode((int)GetPosition().X, (int)GetPosition().Y, (int)RelativeRect.Width, (int)RelativeRect.Height);
                foreach (UIElement element in Elements.Where(e => e.Visible))
                {
                    if (UIManager.DebugMode)
                        element.DebugDraw();
                    element.Update();
                }
            Raylib.EndScissorMode();
        }
    }

    public override void Kill()
    {
        for (int i = 0; i < Elements.Count; i++)
        {
            Elements[i].Kill();
        }
        base.Kill();
    }

    public virtual void AddElement(UIElement element)
    {
        element.Container?.RemoveElement(element);
        Elements.Add(element);
        /*var tempAnchor = element.Anchor;
        element.SetAnchor(AnchorPosition.TopLeft, this);
        element.SetAnchor(tempAnchor.AnchorPoint);*/
        element.Container = this;
        Elements = Elements.OrderBy(x => x.Layer).ToList();
    }

    public virtual void RemoveElement(UIElement element)
    {
        Elements.Remove(element);
    }

    public virtual void ClearElements()
    {
        for (int i = Elements.Count - 1; i >= 0; i--)
        {
            Elements[i].Kill();
        }
    }

    public override void ProcessEvent(Event evnt)
    {
        base.ProcessEvent(evnt);
        
        // Uses a for loop because elements can be added through processed events.
        for (int i = 0; i < Elements.Count; i++)
        {
            Elements[i].ProcessEvent(evnt);
        }
    }
}