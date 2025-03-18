using System.Numerics;
using System.Runtime.InteropServices;
using Leaf.UI.Events;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UIButton : UIElement, IUIClickable
{
    [MarshalAs(UnmanagedType.LPUTF8Str)] private string _text;
	private Font _font;
	private int _fontSize = 2;
	private string _textAlignmentHorizontal = "center";
	private string _textAlignmentVertical = "center";
	private Color _textColour = Color.White;

	private Texture2D _currentTexture;
	private NPatchInfo _currentNPatch;

	//Button Textures and Pressed State
	private Texture2D _normal;
	private Texture2D _hover;
	private Texture2D _disabled;
	private bool _pressed;

	private Vector2 _textSize = Vector2.Zero;
	private Vector2 _textPosition = Vector2.Zero;

	public UIButton(
		UIRect posScale, 
		string text, 
		bool visible = true, 
		IUIContainer? container = null,
		string id = "",
		string @class = "",
		(string, Vector2) anchor = default,
		Vector2 origin = default
	) : base(posScale, visible, container, id, @class, "button", anchor, origin)
	{
		_text = text;
		ThemeElement();
		_currentTexture = _normal;
		_currentNPatch = Resources.GenerateNPatchInfoFromButton(_currentTexture);
		SetText(_text);
	}

	public override void ThemeElement()
	{
		base.ThemeElement();
		_font = Theme.GetProperty("font-family").AsFont();
		_fontSize = Theme.GetProperty("font-size").AsInt();
		_textColour = Theme.GetProperty("color").AsColor();
		_textAlignmentHorizontal = Theme.GetProperty("text-align");
		_textAlignmentVertical = Theme.GetProperty("text-align-vertical");
		List<Texture2D> images = Theme.GetProperty("button-style").AsButtonImages();
		_normal = images[0];
		_hover = images[1];
		_disabled = images[2];
	}

	public void SetText(string text)
	{
		_text = text;
		AlignButtonText();
	}

	private void AlignButtonText()
	{
		_textSize = MeasureTextEx(_font, _text, _fontSize, 1);
		float textX = _textAlignmentHorizontal switch
		{
			"left" => GetPosition().X,
			"center" => GetPosition().X + (RelativeRect.Size.X / 2) - (_textSize.X / 2),
			"right" => GetPosition().X + (RelativeRect.Size.X) - (_textSize.X),
			_ => GetPosition().X
		};
		float textY = _textAlignmentVertical switch
		{
			"top" => GetPosition().Y,
			"center" => GetPosition().Y + (RelativeRect.Size.Y / 2) - (_textSize.Y / 2),
			"bottom" => GetPosition().Y + (RelativeRect.Size.Y) - (_textSize.Y),
			_ => GetPosition().Y
		};
		_textPosition = new Vector2(textX, textY);
	}

	[DllImport("raylib", CallingConvention = CallingConvention.Cdecl)]
	public static extern void DrawTextEx(
		Font font,
		[MarshalAs(UnmanagedType.LPUTF8Str)]string text,
		Vector2 position,
		float fontSize,
		float spacing,
		Color tint
	);

	public override void Update()
	{
		base.Update();
		
		HandleElementInteraction();
		
		DrawTextureNPatch(
			_currentTexture,
			_currentNPatch,
			new Rectangle(GetPosition(), RelativeRect.Size),
			Vector2.Zero,
			0,
			Color.White
		);
		DrawTextEx(
			_font,
			_text,
			_textPosition,
			_fontSize,
			1,
			_textColour
		);
	}

	public void ChangeTexture()
	{
		_currentTexture = !Active ? _disabled : Hovered ? _hover  : _normal;
	}

	public void HandleElementInteraction()
	{
		ChangeTexture();
		if (!Active) { return; }

		if (Hovered)
		{
			Event newEvent = new(this, EventType.None);

			if (IsMouseButtonDown(MouseButton.Left))
			{
				newEvent.EventType = EventType.LeftMouseDown;
				_pressed = true;
			}
			else if (IsMouseButtonReleased(MouseButton.Left))
			{
				newEvent.EventType = _pressed ? EventType.LeftMouseClick : EventType.LeftMouseUp;
				_pressed = false;
			}
			else if (IsMouseButtonDown(MouseButton.Right))
			{
				newEvent.EventType = EventType.RightMouseDown;
				_pressed = true;
			}
			else if (IsMouseButtonReleased(MouseButton.Right))
			{
				newEvent.EventType = _pressed ? EventType.RightMouseClick : EventType.RightMouseUp;
				_pressed = false;
			}
			else
			{
				_pressed = false;
			}
			
			if (newEvent.EventType != EventType.None)
			{
				Manager.PushEvent(newEvent);
			}
		}
	}

	public override void SetAnchor(string anchorPosition, UIElement anchorElement)
	{
		base.SetAnchor(anchorPosition, anchorElement);
		AlignButtonText();
	}

	public override void SetAnchor(string anchorPosition, Vector2 anchorOffset)
	{
		base.SetAnchor(anchorPosition, anchorOffset);
		AlignButtonText();
	}
}