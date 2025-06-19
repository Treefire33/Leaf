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
		string[]? classes = null,
		Vector2 anchor = default,
		Vector2 origin = default,
		string? tooltip = null
	) : base(posScale, visible, container, id, classes, "button", anchor, origin, tooltip)
	{
		_text = text;
		ThemeElement();
		_currentTexture = _normal;
		_currentNPatch = nPatch ?? Resources.GenerateNPatchInfoFromButton(_currentTexture);
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
			else if (IsMouseButtonDown(MouseButton.Side))
			{
				newEvent.EventType = EventType.SideMouseDown;
				_pressed = true;
			}
			else if (IsMouseButtonReleased(MouseButton.Side))
			{
				newEvent.EventType = _pressed ? EventType.SideMouseClick : EventType.SideMouseUp;
				_pressed = false;
			}
			else if (IsMouseButtonDown(MouseButton.Extra))
			{
				newEvent.EventType = EventType.ExtraMouseDown;
				_pressed = true;
			}
			else if (IsMouseButtonReleased(MouseButton.Extra))
			{
				newEvent.EventType = _pressed ? EventType.ExtraMouseClick : EventType.ExtraMouseUp;
				_pressed = false;
			}
			else if (IsMouseButtonDown(MouseButton.Forward))
			{
				newEvent.EventType = EventType.ExtraMouseDown;
				_pressed = true;
			}
			else if (IsMouseButtonReleased(MouseButton.Forward))
			{
				newEvent.EventType = _pressed ? EventType.ForwardMouseClick : EventType.ForwardMouseUp;
				_pressed = false;
			}
			else if (IsMouseButtonDown(MouseButton.Back))
			{
				newEvent.EventType = EventType.ExtraMouseDown;
				_pressed = true;
			}
			else if (IsMouseButtonReleased(MouseButton.Back))
			{
				newEvent.EventType = _pressed ? EventType.BackMouseClick : EventType.BackMouseUp;
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
		
		// All click events happen to land on a multiple of 3.
		if (evnt.Element == this && (int)evnt.EventType % 3 == 0)
		{
			OnClick?.Invoke(evnt.EventType switch
			{
				EventType.LeftMouseClick => 0,
				EventType.RightMouseClick => 1,
				EventType.MiddleMouseClick => 2,
				EventType.SideMouseClick => 3,
				EventType.ExtraMouseClick => 4,
				EventType.ForwardMouseClick => 5,
				EventType.BackMouseClick => 6,
				_ => -1
			});
		}
	}
	
	public override void SetAnchor(AnchorPosition anchorPosition, UIElement? target = null, AnchorTarget targetMode = AnchorTarget.XY)
	{
		base.SetAnchor(anchorPosition, target, targetMode);
		_textPosition = AlignText(_text);
	}
}