using System.Numerics;
using Leaf.Events;
using Leaf.UI.Interfaces;
using Leaf.UI.Theming;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Leaf.UI;

/// <summary>
/// Base UI element.
/// </summary>
public class UIElement : IUIElement
{
	protected readonly UIManager Manager;
	protected UIThemeData Theme;
	//all positions are, by default, based on the top left
	public Anchor Anchor;
	public Vector2 Origin = Vector2.Zero;
	public UIRect RelativeRect { get; set; }
	public int Layer = 0;

	public IUIContainer? Container;
	private UITooltip? _tooltip;

	public bool Visible = true;
	public bool Active = true;

	private bool _hovered;
	public bool Hovered
	{
		get => _hovered;
		private set
		{
			if (!_hovered && value)
				OnHover?.Invoke();
			_hovered = value;
		}
	}
	
	/// <summary>
	/// An action fired *once* when the element is hovered.
	/// Does not fire when the element is unhovered.
	/// </summary>
	public Action? OnHover { get; set;}

	public UIElement(
		UIRect posScale, 
		bool visible = true, 
		IUIContainer? container = null,
		string id = "",
		string @class = "",
		string element = "",
		(string, Vector2) anchor = default,
		Vector2 origin = default,
		string? tooltip = null,
		bool isRootContainer = false
	)
	{
		Manager = UIManager.GetDefaultManager();
		RelativeRect = posScale;
		Theme = Manager.Theme.GetThemeDataFromObject($"#{id}", $".{@class}", element);
		if (!isRootContainer)
		{
			Container = container ?? Manager.GetDefaultContainer();
		}
		else
		{
			Container = null;
		}
		Container?.AddElement(this);
		if (anchor == default)
		{
			anchor = ("top-left", Vector2.Zero);
		}
		Anchor = new Anchor();
		if (origin == default)
		{
			origin = Vector2.Zero;
		}
		Origin = origin;
		SetAnchor(anchor.Item1!, anchor.Item2);
		Visible = visible;
		if (tooltip != null)
		{
			_tooltip = new UITooltip(
				tooltip,
				this
			);
		}
	}

	public virtual void Update()
	{
		if (UIManager.DebugMode)
			DrawRectangleRec(new UIRect(GetPosition(), RelativeRect.Size), Color.Red);
		Hovered = CheckCollisionPointRec(Utility.GetVirtualMousePosition(), new Rectangle(GetPosition(), RelativeRect.Size));
	}

	public virtual void ThemeElement() { }

	public virtual void Kill()
	{
		Container?.RemoveElement(this);
		_tooltip?.Kill();
	}

	public virtual void ProcessEvent(Event evnt) { }

	public virtual void SetAnchor(string anchorPosition, Vector2 anchorOffset)
	{
		Anchor.AnchorPosition = anchorPosition;
		Anchor.Offset = anchorOffset;
	}
	
	public virtual void SetAnchor(string anchorPosition, UIElement anchorElement)
	{
		Anchor.AnchorPosition = anchorPosition;
		Anchor.Offset = anchorElement.GetPosition();
	}

	public Vector2 GetPosition()
	{
		Vector2 containerOffset = Vector2.Zero;
		Vector2 containerSize = UIManager.GameSize;
		if (Container != null)
		{
			containerOffset = Container.GetPosition();
			containerSize = Container.RelativeRect.Size;
		}
		return ((RelativeRect.Position + Anchor.GetAnchored(containerSize)) - RelativeRect.Size * Origin) + containerOffset;
	}

	public void SetVisibility(bool visibilityState)
	{
		Visible = visibilityState;
	}

	public void Show() => SetVisibility(true);
	public void Hide() => SetVisibility(false);

	public void SetActive(bool activeState)
	{
		Active = activeState;
	}

	public void Enable() => SetActive(true);
	public void Disable() => SetActive(false);
}
