using System.Numerics;
using Leaf.Events;
using Leaf.UI.Interfaces;
using Leaf.UI.Theming;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace Leaf.UI;

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

	public bool Visible;
	public bool Active = true;

	private bool _hovered;
	public bool Hovered
	{
		get => _hovered;
		protected set
		{
			if (!_hovered && value)
				OnHover?.Invoke();
			_hovered = value;
		}
	}
	
	// Theming parameters
	protected Font _font;
	protected int _fontSize = 5;
	protected int _textSpacing = 1;
	protected Color _textColour = Color.White;
	protected HorizontalTextAlignment _horizontalTextAlignment = HorizontalTextAlignment.Left;
	protected VerticalTextAlignment _verticalTextAlignment = VerticalTextAlignment.Top;
	protected Color _backgroundColour = Color.White;
	protected Color _borderColour = Color.White;
	protected float _borderThickness;
	protected float _borderRadius;
	
	/// <summary>
	/// An action fired *once* when the element is hovered.
	/// Does not fire when the element is unhovered.
	/// </summary>
	public Action? OnHover { get; set;}
	
	/// <summary>Base UI element.</summary>
	/// <param name="posScale">The relative position and scale of the element.</param>
	/// <param name="visible">The visibility of the element.</param>
	/// <param name="container">The parent container of the element, defaults to the default container.</param>
	/// <param name="id">The style ID of the element.</param>
	/// <param name="class">The style class of the element.</param>
	/// <param name="element">The base style for an element.</param>
	/// <param name="anchor">The anchor point for an element.</param>
	/// <param name="origin">A point in the element where positioning is based on.</param>
	/// <param name="tooltip">A string that creates a tooltip.</param>
	/// <param name="isRootContainer">Is the element the root container?</param>
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
		ThemeElement();
	}

	public virtual void DebugDraw()
	{
		DrawRectangleLinesEx(
			new Rectangle(GetPosition(), RelativeRect.Size),
			1f,
			Color.Red
		);
	}	
	
	public virtual void Update()
	{
		Hovered = CheckCollisionPointRec(Utility.GetVirtualMousePosition(), new Rectangle(GetPosition(), RelativeRect.Size));
	}

	public virtual void ThemeElement()
	{
		_font = Theme.GetProperty("font-family").AsFont();
		_fontSize = Theme.GetProperty("font-size").AsInt();
		_textSpacing = Theme.GetProperty("spacing").AsInt();
		_textColour = Theme.GetProperty("color").AsColor();
		_horizontalTextAlignment = Utility.GetHorizontalAlignmentFromString(Theme.GetProperty("text-align", "left"));
		_verticalTextAlignment = Utility.GetVerticalAlignmentFromString(Theme.GetProperty("text-align-vertical", "top"));
		_backgroundColour = Theme.GetProperty("background-color").AsColor();
		_borderColour = Theme.GetProperty("border-top-color").AsColor();
		_borderThickness = Theme.GetProperty("border-top-width").AsFloat();
		_borderRadius = Theme.GetProperty("border-top-left-radius").AsFloat()/100;
	}
	
	public Vector2 AlignText(string text)
	{
		var textSize = MeasureTextEx(_font, text, _fontSize, _textSpacing);
		float textX = _horizontalTextAlignment switch
		{
			HorizontalTextAlignment.Left => GetPosition().X,
			HorizontalTextAlignment.Center => GetPosition().X + (RelativeRect.Size.X / 2) - (textSize.X / 2),
			HorizontalTextAlignment.Right => GetPosition().X + (RelativeRect.Size.X) - (textSize.X),
			_ => GetPosition().X
		};
		float textY = _verticalTextAlignment switch
		{
			VerticalTextAlignment.Top => GetPosition().Y,
			VerticalTextAlignment.Center => GetPosition().Y + (RelativeRect.Size.Y / 2) - (textSize.Y / 2),
			VerticalTextAlignment.Bottom => GetPosition().Y + (RelativeRect.Size.Y) - (textSize.Y),
			_ => GetPosition().Y
		};
		return new Vector2(textX, textY);
	}

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
