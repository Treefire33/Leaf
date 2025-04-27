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
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null,
        bool isRootContainer = false
    ) : base(posScale, visible, container, id, @class, "container", anchor, origin, tooltip:tooltip, isRootContainer: isRootContainer)
    {
        
    }

    public override void Update()
    {
        Elements = Elements.OrderBy(x => x.Layer).ToList();
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
        var tempAnchor = element.Anchor;
        element.SetAnchor("top-left", Vector2.Zero);
        element.SetAnchor(tempAnchor.AnchorPosition, tempAnchor.Offset);
        element.Container = this;
        Elements = Elements.OrderBy(x => x.Layer).ToList();
    }

    public virtual void RemoveElement(UIElement element)
    {
        Elements.Remove(element);
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