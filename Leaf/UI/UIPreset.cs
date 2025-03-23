namespace Leaf.UI;

/// <summary>
/// A collection of elements in a dictionary that can easily be created and destroyed.
/// </summary>
public class UIPreset
{
    private Dictionary<string, UIElement> Elements { get; } = [];

    public UIElement this[string name] => Elements[name];

    public UIPreset(params UIElement[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Elements.Add($"element_{i}", elements[i]);
        }
    }
    
    public UIPreset(params (string, UIElement)[] elements)
    {
        foreach (var pair in elements)
        {
            Elements.Add(pair.Item1, pair.Item2);
        }
    }

    public void Load()
    {
        foreach (var element in Elements)
        {
            element.Value.SetVisibility(true);
            element.Value.SetActive(true);
        }
    }
    
    public void Unload()
    {
        foreach (var element in Elements)
        {
            element.Value.SetVisibility(false);
            element.Value.SetActive(false);
        }
    }
}