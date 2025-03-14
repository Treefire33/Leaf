namespace Leaf.UI;

/// <summary>
/// A group of UIElements, use in lieu of UIContainer when you want to control a lot of defined elements rather than
/// access the ContainedElements list.
/// </summary>
/// <param name="elements">A list of elements inside the group</param>
public class UIGroup(params UIElement[] elements)
{
    private List<UIElement> _elements = elements.ToList();

    public void Set(params UIElement[] elements)
    {
        _elements = elements.ToList();
    }

    public void AddElement(UIElement element)
    {
        _elements.Add(element);
    }

    public void RemoveElement(UIElement element)
    {
        _elements.Remove(element);
    }

    public void Clear()
    {
        _elements.Clear();
    }

    public void SetVisibility(bool visible)
    {
        foreach (var element in _elements)
        {
            element.SetVisibility(visible);
        }
    }

    public void Show() => SetVisibility(true);
    public void Hide() => SetVisibility(false);

    public void SetActive(bool active)
    {
        foreach (var element in _elements)
        {
            element.SetActive(active);
        }
    }
    
    public void Enable() => SetActive(true);
    public void Disable() => SetActive(false);
}