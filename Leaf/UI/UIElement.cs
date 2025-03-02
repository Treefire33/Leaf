using System.Numerics;
using Cattail.UI.Interfaces;
using Cattail.UI.Theming;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cattail.UI;

public class UIElement : IUIElement
{
	protected readonly UIManager Manager;
	protected UIElementAppearance Theme;
	//all positions are, by default, based on the top left
	public Anchor Anchor;
	public Vector2 Origin = Vector2.Zero;
	public UIRect RelativeRect;
	public int Layer = 0;

	public UIContainer? Container;

	public bool Visible = true;
	public bool Active = true;
	public bool Hovered;

	public UIElement(
		UIRect posScale, 
		bool visible = true, 
		UIContainer? container = null,
		ObjectID objectID = default,
		(string, Vector2) anchor = default,
		Vector2 origin = default,
		bool isRootContainer = false
	)
	{
		Manager = UIManager.GetDefaultManager();
		RelativeRect = posScale;
		if (anchor == default)
		{
			anchor = ("top-left", Vector2.Zero);
		}
		Anchor = new Anchor();
		SetAnchor(anchor.Item1!, anchor.Item2);
		if (origin == default)
		{
			origin = Vector2.Zero;
		}
		Origin = origin;
		if (container == null && !isRootContainer)
		{
			Container = Manager.GetDefaultContainer()!;
		}
		/*Manager.Elements.Add(this);
		Manager.Elements = [.. Manager.Elements.OrderBy(element => element.Layer)];*/
		Container?.AddElement(this);
		if (objectID == default)
		{
			objectID.ID = "default";
			objectID.Class = "";
		}
		Theme = Manager.Theme.GetFromObjectID(objectID);
		Visible = visible;
	}

	public virtual void Update()
	{
		DrawRectangleRec(new UIRect(GetPosition(), RelativeRect.Size), Color.Red);
		Hovered = CheckCollisionPointRec(Utility.GetVirtualMousePosition(), RelativeRect);
	}

	public virtual void ThemeElement() { }

	public virtual void Kill()
	{
		Container?.RemoveElement(this);
	}

	public void SetAnchor(string anchorPosition, Vector2 anchorOffset)
	{
		Vector2 containerSize;
		containerSize = Container != null ? Container.RelativeRect.Size : UIManager.GameSize;

		Anchor.AnchorPoint = anchorPosition.ToLower() switch
		{
			"top-left" => Vector2.Zero,
			"top-center" => containerSize with { X = containerSize.X / 2 },
			"top-right" => containerSize with { X = containerSize.X },
			"left" => containerSize with { Y = containerSize.Y / 2 },
			"center" => containerSize with { X = containerSize.X / 2, Y = containerSize.Y / 2 },
			"right" => containerSize with { X = containerSize.X, Y = containerSize.Y / 2 },
			"bottom-left" => containerSize with { Y = containerSize.Y },
			"bottom-center" => containerSize with { X = containerSize.X / 2, Y = containerSize.Y },
			"bottom-right" => containerSize with { X = containerSize.X, Y = containerSize.Y },
			_ => Anchor.AnchorPoint
		};

		Anchor.Offset = anchorOffset;
	}

	public Vector2 GetPosition()
	{
		return (RelativeRect.Position + Anchor.GetAnchored()) - RelativeRect.Size * Origin;
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
