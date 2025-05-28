using System.Numerics;
using Raylib_cs;

namespace Leaf.UI;

/// <summary>
/// A custom rectangle struct used to better position and size UI elements.
/// </summary>
/// <param name="position">The x and y of the rect.</param>
/// <param name="scale">The width and height of the rect.</param>
public struct UIRect(Vector2 position, Vector2 scale)
{
	public UIRect(float x, float y, float width, float height) : this(new(x, y), new(width, height)) { }
	public UIRect(Vector2 position, float width, float height) : this(position, new(width, height)) { }
	public UIRect(float x, float y, Vector2 scale) : this(new(x, y), scale) { }

	private Rectangle RaylibRect => new(X, Y, Width, Height);
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
		get => new(X, Y);
		set { X = value.X; Y = value.Y; }
	}
	public Vector2 TopCenter
	{
		get => new(X / 2 + Width / 2, Y);
		set 
		{ 
			X = value.X * 2 - Width;
			Y = value.Y;
		}
	}
	public Vector2 TopRight
	{
		get => new(X + Width, Y);
		set
		{
			X = value.X - Width;
			Y = value.Y;
		}
	}

	public Vector2 CenterLeft
	{
		get => new(X, Y / 2 + Height / 2);
		set
		{
			X = value.X;
			Y = value.Y / 2 - Height;
		}
	}
	public Vector2 Center
	{
		get => (Position / 2) + (Size / 2);
		set
		{
			X = value.X * 2 - Width;
			Y = value.Y * 2 - Height;
		}
	}
	public Vector2 CenterRight
	{
		get => new(X + Width, Y / 2 + Height / 2);
		set
		{
			X = value.X - Width;
			Y = value.Y * 2 - Height;
		}
	}

	public Vector2 BottomLeft
	{
		get => new(X, Y + Height);
		set
		{
			X = value.X;
			Y = value.Y - Height;
		}
	}
	public Vector2 BottomCenter
	{
		get => new(X / 2 + Width / 2, Y + Height);
		set
		{
			X = value.X * 2 - Width;
			Y = value.Y - Height;
		}
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

	public override string ToString() => $"UIRect: <{Position}>, <{Size}>";

	public readonly UIRect Scale(float scaleFactor) =>
		new(
			X,
			Y,
			Width * scaleFactor,
			Height * scaleFactor
		);

	public readonly UIRect Scale(Vector2 scaleFactor) =>
		new(
			X,
			Y,
			Width * scaleFactor.X,
			Height * scaleFactor.Y
		);

	public readonly UIRect Scale(UIRect scaleRect) =>
		new(
			X,
			Y,
			Width * scaleRect.Width,
			Height * scaleRect.Height
		);

	public static UIRect operator +(UIRect a, UIRect b) => 
		new(a.X + b.X, a.Y + b.Y, a.Width + b.Width, a.Height + b.Height);

	public static UIRect operator -(UIRect a, UIRect b) => 
		new(a.X - b.X, a.Y - b.Y, a.Width - b.Width, a.Height - b.Height);

	public static UIRect operator *(UIRect a, float scale) =>
		a with
		{
			X = a.X * scale,
			Y = a.Y * scale,
			Width = a.Width * scale,
			Height = a.Height * scale
		};

	public static UIRect operator *(UIRect a, Vector2 scale) =>
		a with
		{
			X = a.X * scale.X,
			Y = a.Y * scale.Y,
			Width = a.Width * scale.X,
			Height = a.Height * scale.Y
		};

	public static UIRect operator *(UIRect a, UIRect b) =>
		a with
		{
			X = a.X * b.X,
			Y = a.Y * b.Y,
			Width = a.Width * b.Width,
			Height = a.Height * b.Height
		};

	public static UIRect operator /(UIRect a, float scale) =>
		a with
		{
			X = a.X / scale,
			Y = a.Y / scale,
			Width = a.Width / scale,
			Height = a.Height / scale
		};

	public static UIRect operator /(UIRect a, Vector2 scale) =>
		a with
		{
			X = a.X / scale.X,
			Y = a.Y / scale.Y,
			Width = a.Width / scale.X,
			Height = a.Height / scale.Y
		};

	public static UIRect operator /(UIRect a, UIRect b) =>
		a with
		{
			X = a.X / b.X,
			Y = a.Y / b.Y,
			Width = a.Width / b.Width,
			Height = a.Height / b.Height
		};

	public static implicit operator Rectangle(UIRect rect)
	{
		return rect.RaylibRect;
	}
	
	public static implicit operator UIRect(Rectangle rect)
	{
		return new UIRect(
			rect.Position,
			rect.Size
		);
	}
}