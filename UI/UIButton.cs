using System.Numerics;
using System.Runtime.InteropServices;
using Cattail.UI.Events;
using Cattail.UI.Interfaces;
using Cattail.UI.Theming;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cattail.UI;

public class UIButton : UIElement, IUIClickable
{
    [MarshalAs(UnmanagedType.LPUTF8Str)] private string _text;
	private Font _font;
	private int _fontSize = 2;
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
		string style, 
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
		List<Texture2D> images = Resources.GetButtonImagesFromStyle(style);
		_normal = images[0];
		_hover = images[1];
		_disabled = images[2];
		_currentTexture = _normal;
		_currentNPatch = Resources.GenerateNPatchInfoFromButton(_currentTexture);
		ThemeElement();
		SetText(text);
	}

	public override void ThemeElement()
	{
		base.ThemeElement();
		/*_font = Theme.Font.Item1;
		_fontSize = Theme.Font.Item2 + 5;
		_textColour = Theme.GetColour("text");*/
	}

	public void SetText(string text)
	{
		_text = text;
		_textSize = MeasureTextEx(_font, _text, _fontSize, 1);
		_textPosition = new Vector2(
			GetPosition().X
			+ (RelativeRect.Size.X / 2)
			- (_textSize.X / 2),
			GetPosition().Y
			+ (RelativeRect.Size.Y / 2)
			- (_textSize.Y / 2)
		);
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
}