using Leaf.UI;

namespace Leaf.Events;

public partial class Event
{
    public Event(UIElement element, EventType type)
    {
        Element = element;
        EventType = type;
    }
    
    public UIElement? Element;
}