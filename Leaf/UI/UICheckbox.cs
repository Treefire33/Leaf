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
    
    public Action<int>? OnClick { get; set; }

    public UICheckbox(
        UIRect posScale,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, @class, "checkbox", anchor, origin, tooltip)
    {
        OnClick += i =>
        {
            if (i == 0)
            {
                Checked = !Checked;
            }
        };
        
        ThemeElement();
        _currentTexture = _normal;
        _currentNPatch = Resources.GenerateNPatchInfoFromButton(_normal);
    }

    public override void ThemeElement()
    {
	    List<Texture2D> images = Theme.GetProperty("checkbox-style").AsCheckboxImages();
	    _normal = images[0];
	    _hover = images[1];
	    _disabled = images[2];
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
	    if (Checked)
	    {
		    DrawCircleV(
			    (GetPosition() + RelativeRect.Size / 2),
			    RelativeRect.Size.Y / 4,
			    Color.White
			);
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

    private void ChangeTexture()
    {
	    
    }
}