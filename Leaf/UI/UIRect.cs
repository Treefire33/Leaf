using System.Numerics;
using Raylib_cs;

namespace Leaf.UI;

/// <summary>
/// Creates a new UIRect, which is similar to pygame's Rect.
/// This was originally created for ClanGenDotNet, so that's why it's like pygame's Rect.
/// Has 9 coordinates that correspond to the top, center and bottom left, center, and right.
/// </summary>
/// <param name="position">The x and y of the rect.</param>
/// <param name="scale">The width and height of the rect.</param>
public struct UIRect(Vector2 position, Vector2 scale)
{
	public UIRect(float x, float y, float width, float height) : this(new(x, y), new(width, height)) { }
	public UIRect(Vector2 position, float width, float height) : this(position, new(width, height)) { }
	public UIRect(float x, float y, Vector2 scale) : this(new(x, y), scale) { }

	//RelativeRect
	public Rectangle RelativeRect => new(X, Y, Width, Height);
	public float X = position.X;
	public float Y = position.Y;
	public float Width = scale.X;
	public float Height = scale.Y;

	public Vector2 Position
	{
		get => new(X, Y);

		set
		{
			X = value.X;
			Y = value.Y;
		}
	}
	public Vector2 Size
	{
		get => new(Width, Height);

		set
		{
			Width = value.X;
			Height = value.Y;
		}
	}

	//Points in RelativeRect
	public Vector2 TopLeft
	{
		get { return new(X, Y); }
	}
	public Vector2 TopCenter
	{
		get { return new(X / 2 + Width / 2, Y); }
	}
	public Vector2 TopRight
	{
		get { return new(X + Width, Y); }
	}

	public Vector2 CenterLeft
	{
		get { return new(X, Y / 2 + Height / 2); }
	}
	public Vector2 Center
	{
		get { return (Position / 2) + (Size / 2); }
	}
	public Vector2 CenterRight
	{
		get { return new(X + Width, Y / 2 + Height / 2); }
	}

	public Vector2 BottomLeft
	{
		get { return new(X, Y + Height); }
		set
		{
			X = value.X;
			Y = value.Y - Height;
		}
	}
	public Vector2 BottomCenter
	{
		get { return new(X / 2 + Width / 2, Y + Height); }
	}
	public Vector2 BottomRight
	{
		get => Position + Size;
		set
		{
			X = value.X - Width;
			Y = value.Y - Height;
		}
	}

	public override string ToString()
	{
		return $"UIRect: <{Position}>, <{Size}>";
	}

	public readonly UIRect Scale(float scaleFactor)
	{
		return new UIRect(
			X * scaleFactor,
			Y * scaleFactor,
			Width * scaleFactor,
			Height * scaleFactor
		);
	}

	public readonly UIRect Scale(Vector2 scaleFactor)
	{
		return new UIRect(
			X * scaleFactor.X,
			Y * scaleFactor.Y,
			Width * scaleFactor.X,
			Height * scaleFactor.Y
		);
	}

	public readonly UIRect Scale(UIRect scaleRect)
	{
		return new UIRect(
			X * scaleRect.X,
			Y * scaleRect.Y,
			Width * scaleRect.Width,
			Height * scaleRect.Height
		);
	}

	public static UIRect operator +(UIRect a, UIRect b)
	{
		return new UIRect(a.X + b.X, a.Y + b.Y, a.Width + b.Width, a.Height + b.Height);
	}
	
	public static UIRect operator -(UIRect a, UIRect b)
	{
		return new UIRect(a.X - b.X, a.Y - b.Y, a.Width - b.Width, a.Height - b.Height);
	}

	public static implicit operator Rectangle(UIRect rect)
	{
		return rect.RelativeRect;
	}
	
	public static implicit operator UIRect(Rectangle rect)
	{
		return new UIRect(
			rect.Position,
			rect.Size
		);
	}
}