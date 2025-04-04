namespace Leaf.UI.Interfaces;

public interface IUIContainer : IUIElement
{
	void AddElement(UIElement element);
	void RemoveElement(UIElement element);
}
