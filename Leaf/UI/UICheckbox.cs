using System.Numerics;
using Leaf.Events;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UICheckbox : UIElement, IUIClickable
{
    private bool _value;

    public bool Checked
    {
        get => _value;
        set => _value = value;
    }
    
    private Texture2D _currentTexture;
    private NPatchInfo _currentNPatch;
    private Texture2D _normal;
    private Texture2D _hover;
    private Texture2D _disabled;
    private Texture2D[] _checkmarks;
    
    public Action<int>? OnClick { get; set; }

    public UICheckbox(
        UIRect posScale,
        bool visible = true, 
        NPatchInfo? nPatch = null,
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, classes, "checkbox", anchor, origin, tooltip)
    {
        ThemeElement();
        _currentTexture = _normal;
        if (nPatch != null)
			_currentNPatch = nPatch.Value;
    }

    public override void ThemeElement()
    {
	    List<Texture2D> images = Theme.GetProperty("button-style").AsButtonImages("checkbox");
	    _normal = images[0];
	    _hover = images[1];
	    _disabled = images[2];
	    _currentNPatch = Theme.GetProperty("nine-patch").AsNPatch(_normal);
	    List<Texture2D> checkmarks = Theme.GetProperty("checkmark-style").AsCheckmarks();
	    _checkmarks = checkmarks.ToArray();
    }

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
	    DrawTexturePro(
		    _checkmarks[Checked ? 0 : 1],
		    new Rectangle(
			    Vector2.Zero,
			    new Vector2(_checkmarks[Checked ? 0 : 1].Width, _checkmarks[Checked ? 0 : 1].Height)
		    ),
		    new Rectangle(
			    GetPosition(),
			    RelativeRect.Size
		    ),
		    Vector2.Zero,
		    0,
		    Color.White
	    );
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

    private bool _pressed;
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
				if (newEvent.EventType == EventType.LeftMouseClick)
				{
					Checked = !Checked;
				}
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

    private void ChangeTexture()
    {
	    _currentTexture = !Active ? _disabled : Hovered ? _hover  : _normal;
    }
}