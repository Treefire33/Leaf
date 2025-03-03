using System.Numerics;
using Cattail.UI.Interfaces;
using Cattail.UI.Theming;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cattail.UI;

public enum HorizontalTextAlignment
{
	Left,
	Center,
	Right
}

public enum VerticalTextAlignment
{
	Top,
	Center,
	Bottom,
}

public class UITextBox : UIElement
{
	private string _text;
	private readonly bool _isMultiline;

	//Theme Attributes
	private Color _textColour;
	private Font _font;
	private float _fontSize;
	private HorizontalTextAlignment _horizontalAlignment;
	private VerticalTextAlignment _verticalAlignment;

	private Vector2 _padding = new(0, 0);

	public UITextBox(
		UIRect posScale,
		string text,
		bool isMultiline = false,
		bool visible = true, 
		IUIContainer? container = null,
		ObjectID id = default,
		(string, Vector2) anchor = default,
		Vector2 origin = default
	) : base(posScale, visible, container, new ObjectID(id.ID ?? "default", id.Class ?? "@textbox"), anchor, origin)
	{
		_text = text;
		_isMultiline = isMultiline;
		ThemeElement();
	}

	public HorizontalTextAlignment GetHorizontalAlignmentFromString(string alignment)
	{
		return alignment switch
		{
			"left" => HorizontalTextAlignment.Left,
			"center" => HorizontalTextAlignment.Center,
			"right" => HorizontalTextAlignment.Right,
			_ => HorizontalTextAlignment.Left
		};
	}

	public VerticalTextAlignment GetVerticalAlignmentFromString(string alignment)
	{
		return alignment switch
		{
			"top" => VerticalTextAlignment.Top,
			"center" => VerticalTextAlignment.Center,
			"bottom" => VerticalTextAlignment.Bottom,
			_ => VerticalTextAlignment.Top
		};
	}

	public override void ThemeElement()
	{
		base.ThemeElement();
		_font = Theme.Font.Item1; //Font resource
		_fontSize = Theme.Font.Item2 + 5; //Font size
		_textColour = Theme.Colours["text"];
		if (Theme.Miscellaneous.TryGetValue("text_horiz_alignment", out dynamic? horizAlignment))
		{
			_horizontalAlignment = GetHorizontalAlignmentFromString(horizAlignment!);
		}
		if (Theme.Miscellaneous.TryGetValue("text_vert_alignment", out dynamic? vertAlignment))
		{
			_verticalAlignment = GetVerticalAlignmentFromString(vertAlignment!);
		}
		/*if (Theme.Miscellaneous.TryGetValue("text_horiz_alignment_padding", out dynamic? horizPadding))
		{
			_horizontalAlignment = GetHorizontalAlignmentFromString(horizAlignment!);
		}
		if (Theme.Miscellaneous.TryGetValue("text_vert_alignment_padding", out dynamic? vertPadding))
		{
			_horizontalAlignment = GetHorizontalAlignmentFromString(horizAlignment!);
		}*/
	}
  
	public void SetPadding(Vector2 padding)
	{
		_padding = padding;
	}

	private Rectangle AlignTextRec(
		UIRect original,
		HorizontalTextAlignment horizontalAlignment = HorizontalTextAlignment.Left,
		VerticalTextAlignment verticalAlignment = VerticalTextAlignment.Top
	)
	{
		return AlignTextRec(original, _text, horizontalAlignment, verticalAlignment);
	}

	private Rectangle AlignTextRec(
		UIRect original,
		string text,
		HorizontalTextAlignment horizontalAlignment = HorizontalTextAlignment.Left,
		VerticalTextAlignment verticalAlignment = VerticalTextAlignment.Top
	)
	{
		Vector2 textSize = MeasureTextEx(_font, text, _fontSize, 0);
		UIRect newRect = original;
		switch (horizontalAlignment)
		{
			default:
			case HorizontalTextAlignment.Left:
				break;

			case HorizontalTextAlignment.Center:
				newRect.X = GetPosition().X
					+ (RelativeRect.Size.X / 2)
					- (textSize.X / 2);
				break;

			case HorizontalTextAlignment.Right:
				//Not implemented
				break;
		}

		switch (verticalAlignment)
		{
			default:
			case VerticalTextAlignment.Top:
				break;

			case VerticalTextAlignment.Center:
				newRect.Y = GetPosition().Y
					+ (RelativeRect.Size.Y / 2)
					- (textSize.Y / 2);
				break;

			case VerticalTextAlignment.Bottom:
				//Not implemented
				break;
		}
    
		return newRect;
	}
	/// <summary>
	/// Sets the text of the button.
	/// </summary>
	/// <param name="text">The text to set the button text to.</param>
	public void SetText(string text)
	{
		_text = text;
	}

	public override void Update()
	{
		base.Update();
		Vector2 textSize = MeasureTextEx(_font, _text, _fontSize, 0);
		if (_isMultiline)
		{
			float positionOffset = 0f;
			foreach (string line in _text.Split('\n'))
			{
				textSize = MeasureTextEx(_font, line, _fontSize, 0);
				var addedRectangles = Utility.AddRectangles(
					AlignTextRec(RelativeRect, line, _horizontalAlignment, _verticalAlignment),
					new Rectangle(
						_padding.X / 2,
						_padding.Y / 2,
						-_padding.X,
						-_padding.Y
					)
				);
				DrawTextPro(
					_font,
					line,
					new Vector2(addedRectangles.X, addedRectangles.Y) + new Vector2(0, (int)positionOffset),
					Vector2.Zero,
					0,
					_fontSize,
					1,
					_textColour
				);
				positionOffset += textSize.Y;
			}
		}
		else if (RelativeRect is { Height: > 0, Width: > 0 })
		{
			Utility.DrawTextBoxed(
				_font,
				_text,
				Utility.AddRectangles(
					AlignTextRec(RelativeRect, _horizontalAlignment, _verticalAlignment),
					new Rectangle(
						_padding.X / 2,
						_padding.Y / 2,
						-_padding.X,
						-_padding.Y
					)
				),
				_fontSize,
				1,
				true,
				_textColour
			);
		}
		else
		{
			var addedRectangles = Utility.AddRectangles(
				AlignTextRec(RelativeRect, _horizontalAlignment, _verticalAlignment),
				new Rectangle(
					_padding.X / 2,
					_padding.Y / 2,
					-_padding.X,
					-_padding.Y
				)
			);
			DrawTextPro(
				_font,
				_text,
				new Vector2(addedRectangles.X, addedRectangles.Y),
				Vector2.Zero,
				0,
				_fontSize,
				1,
				Color.White
			);
			RelativeRect = RelativeRect with { Height = textSize.Y };
		}
	}
}
