using System.Numerics;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

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

	//Theme Attributes
	private HorizontalTextAlignment _horizontalAlignment;
	private VerticalTextAlignment _verticalAlignment;

	private Vector2 _padding = new(0, 0);

	public UITextBox(
		UIRect posScale,
		string text,
		bool visible = true, 
		IUIContainer? container = null,
		string id = "",
		string @class = "",
		Vector2 anchor = default,
		Vector2 origin = default,
		string? tooltip = null
	) : base(posScale, visible, container, id, @class, "textbox", anchor, origin, tooltip)
	{
		_text = text;
	}

	public void SetPadding(Vector2 padding)
	{
		_padding = padding;
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
		//Vector2 textSize = MeasureTextEx(_font, _text, _fontSize, 0);
		Utility.DrawTextBoxed(
			_font,
			_text,
			new Rectangle(AlignText(_text), RelativeRect.Size),
			_fontSize,
			_textSpacing,
			true,
			_textColour
		);
	}
}
