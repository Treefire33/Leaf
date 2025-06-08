using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UITooltip : UIElement
{
	private string _tooltipText = null!;
	private Vector2 _padding = new(5, 5);

	private readonly UIElement _parentElement = null!;

	public UITooltip(
		string? text, 
		UIElement parentElement,
		bool visible = true, 
		IUIContainer? container = null,
		string id = "",
		string[]? classes = null,
		Vector2 anchor = default,
		Vector2 origin = default
	) : base(new UIRect(), visible, container, id, classes, "tooltip", anchor, origin, tooltip:null)
	{
		if (text == null)
		{
			return;
		}
		_parentElement = parentElement;
		_tooltipText = text;
		CalculateSize();

		Layer = 999;
	}

	private void CalculateSize()
	{
		Vector2 textSize = MeasureTextEx(_font, FormatTooltip(_tooltipText), _fontSize, _textSpacing);
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
			Vector2 pos = Utility.GetVirtualMousePosition() - new Vector2(0, RelativeRect.Height + _padding.Y);
			Utility.DrawRectangle(
				new Rectangle(pos, RelativeRect.Width, RelativeRect.Height),
				_borderRadius,
				_borderThickness,
				_backgroundColour,
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
