using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Leaf.Events;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

/// <summary>
/// A button.
/// </summary>
public class UIButton : UIElement, IUIClickable
{
    [MarshalAs(UnmanagedType.LPUTF8Str)] private string _text;
    public string Text => _text;

	private Texture2D _currentTexture;
	private NPatchInfo _currentNPatch;

	//Button Textures and Pressed State
	private Texture2D _normal;
	private Texture2D _hover;
	private Texture2D _disabled;
	private bool _pressed;

	private Vector2 _textSize = Vector2.Zero;
	private Vector2 _textPosition = Vector2.Zero;
	
	/// <summary>
	/// An action fired when the button is clicked in *any* way.
	///	Provides an integer representing the button clicked.
	/// </summary>
	public Action<int>? OnClick { get; set; }

	/// <inheritdoc cref="UIElement"/>
	public UIButton(
		UIRect posScale, 
		string text, 
		NPatchInfo? nPatch = null,
		bool visible = true, 
		IUIContainer? container = null,
		string id = "",
		string @class = "",
		Vector2 anchor = default,
		Vector2 origin = default,
		string? tooltip = null
	) : base(posScale, visible, container, id, @class, "button", anchor, origin, tooltip)
	{
		_text = text;
		_currentTexture = _normal;
		_currentNPatch = nPatch ?? Resources.GenerateNPatchInfoFromButton(_currentTexture);
		ThemeElement();
		SetText(_text);
	}

	public override void ThemeElement()
	{
		base.ThemeElement();
		List<Texture2D> images = Theme.GetProperty("button-style").AsButtonImages();
		_normal = images[0];
		_hover = images[1];
		_disabled = images[2];
		_currentNPatch = Theme.GetProperty("nine-patch").AsNPatch(_normal);
	}

	public void SetText(string text)
	{
		_text = text;
		_textPosition = AlignText(_text);
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
		_textPosition = AlignText(_text);
		
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
			_textSpacing,
			_textColour
		);
		/*Utility.DrawTextStyled(
			_font,
			_text,
			new Rectangle(
				_textPosition,
				RelativeRect.Size
			),
			_fontSize,
			_textSpacing,
			true,
			_textColour
		);*/
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
			else if (IsMouseButtonDown(MouseButton.Middle))
			{
				newEvent.EventType = EventType.MiddleMouseDown;
				_pressed = true;
			}
			else if (IsMouseButtonReleased(MouseButton.Middle))
			{
				newEvent.EventType = _pressed ? EventType.MiddleMouseClick : EventType.MiddleMouseUp;
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

	public override void ProcessEvent(Event evnt)
	{
		base.ProcessEvent(evnt);
		if (evnt.Element == this && evnt.EventType == EventType.LeftMouseClick)
		{
			OnClick?.Invoke(0);
		}
		if (evnt.Element == this && evnt.EventType == EventType.RightMouseClick)
		{
			OnClick?.Invoke(1);
		}
		if (evnt.Element == this && evnt.EventType == EventType.MiddleMouseClick)
		{
			OnClick?.Invoke(2);
		}
	}
	
	public override void SetAnchor(AnchorPosition anchorPosition, UIElement? target = null)
	{
		base.SetAnchor(anchorPosition, target);
		_textPosition = AlignText(_text);
	}
}