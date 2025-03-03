namespace Cattail.UI.Interfaces;

public interface IUIContainer : IUIElement
{
	void AddElement(UIElement element);
	void RemoveElement(UIElement element);
}
