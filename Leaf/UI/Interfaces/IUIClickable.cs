namespace Leaf.UI.Interfaces;

public interface IUIClickable
{
	public Action<int>? OnClick { get; set; }
	void HandleElementInteraction();
}
