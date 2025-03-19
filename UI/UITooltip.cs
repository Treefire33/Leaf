using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UITooltip : UIElement
{
	private Font _font;
	private int _fontSize;
	private Color _textColour;
	private Color _backgroundColour;
	private Color _borderColour;
	private int _borderThickness;
	private int _segments;
	private string _tooltipText;
	private Vector2 _padding = new(5, 5);

	private readonly UIElement _parentElement;

	public UITooltip(
		string? text, 
		UIElement parentElement,
		bool visible = true, 
		IUIContainer? container = null,
		string id = "",
		string @class = "",
		(string, Vector2) anchor = default,
		Vector2 origin = default
	) : base(new UIRect(), visible, container, id, @class, "tooltip", anchor, origin, tooltip:null)
	{
		if (text == null)
		{
			return;
		}
		_parentElement = parentElement;
		_tooltipText = text;
		ThemeElement();
		CalculateSize();
	}

	public override void ThemeElement()
	{
		base.ThemeElement();
		_font = Theme.GetProperty("font-family").AsFont();
		_fontSize = Theme.GetProperty("font-size").AsInt();
		_textColour = Theme.GetProperty("color").AsColor();
		_backgroundColour = Theme.GetProperty("background-color").AsColor();
		_borderColour = Theme.GetProperty("border-top-color").AsColor();
		_borderThickness = Theme.GetProperty("border-top-width").AsInt();
		_segments = Theme.GetProperty("segments").AsInt();
	}

	private void CalculateSize()
	{
		Vector2 textSize = MeasureTextEx(_font, FormatTooltip(_tooltipText), _fontSize, 1);
		//_padding = textSize / 8;
		RelativeRect = new UIRect(
			GetPosition(),
			250,
			textSize.Y * 2
		);
	}

	//Returns a string that doesn't make the tooltip bigger than the screen.
	//Max tooltip width is 200, no cap on height
	private string FormatTooltip(string text)
	{
		StringBuilder currentString = new();
		int line = 0;
		for (int i = 0; i < text.Length; i++)
		{
			_ = currentString.Append(text[i]);
			string currentLine = currentString.ToString().Split('\n')[line];
			if (MeasureTextEx(_font, currentLine, 20, 0).X >= 250)
			{
				_ = currentString.Append('\n');
				line++;
			}
		}
		return currentString.ToString();
	}

	public void SetTooltipText(string text)
	{
		_tooltipText = text;
	}

	public override void Update()
	{
		if (_parentElement.Hovered)
		{
			var pos = Utility.GetVirtualMousePosition() - new Vector2(0, RelativeRect.Height + _padding.Y);
			DrawRectangleRounded(
				new Rectangle(pos, RelativeRect.Width, RelativeRect.Height),
				0.3f,
				_segments,
				_backgroundColour
			);
			DrawRectangleRoundedLinesEx(
				new Rectangle(pos, RelativeRect.Width, RelativeRect.Height),
				0.3f,
				_segments,
				_borderThickness,
				_borderColour
			);
			Utility.DrawTextBoxed(
				_font,
				_tooltipText,
				Utility.AddRectangles(
					new Rectangle(pos, RelativeRect.Width, RelativeRect.Height),
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
	}
}
