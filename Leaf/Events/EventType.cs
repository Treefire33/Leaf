namespace Leaf.Events;

[Flags]
public enum EventType
{
	/// <summary>Unused; represents no event.</summary>
	None,
	
	/// <summary>Left mouse button is being pressed.</summary>
	LeftMouseDown,
	/// <summary>Left mouse button has been released.</summary>
	LeftMouseUp,
	/// <summary>Left mouse button has been pressed and released.</summary>
	LeftMouseClick,
	
	/// <summary>Right mouse button is being pressed.</summary>
	RightMouseDown,
	/// <summary>Right mouse button has been released.</summary>
	RightMouseUp,
	/// <summary>Right mouse button has been pressed and released.</summary>
	RightMouseClick,
	
	/// <summary>Middle mouse button is being pressed.</summary>
	MiddleMouseDown,
	/// <summary>Middle mouse button has been released.</summary>
	MiddleMouseUp,
	/// <summary>Middle mouse button has been pressed and released.</summary>
	MiddleMouseClick,
	
	/// <summary>Side mouse button (additional/fourth mouse button) is being pressed.</summary>
	SideMouseDown,
	/// <summary>Side mouse button (additional/fourth mouse button) has been released.</summary>
	SideMouseUp,
	/// <summary>Side mouse button (additional/fourth mouse button) has been pressed and released.</summary>
	SideMouseClick,
	
	/// <summary>Extra mouse button (additional/fifth mouse button) is being pressed.</summary>
	ExtraMouseDown,
	/// <summary>Extra mouse button (additional/fifth mouse button) has been released.</summary>
	ExtraMouseUp,
	/// <summary>Extra mouse button (additional/fifth mouse button) has been pressed and released.</summary>
	ExtraMouseClick,
	
	/// <summary>Forward mouse button (additional/sixth mouse button) is being pressed.</summary>
	ForwardMouseDown,
	/// <summary>Forward mouse button (additional/sixth mouse button) has been released.</summary>
	ForwardMouseUp,
	/// <summary>Forward mouse button (additional/sixth mouse button) has been pressed and released.</summary>
	ForwardMouseClick,
	
	/// <summary>Back mouse button (additional/seventh mouse button) is being pressed.</summary>
	BackMouseDown,
	/// <summary>Back mouse button (additional/seventh mouse button) has been released.</summary>
	BackMouseUp,
	/// <summary>Back mouse button (additional/seventh mouse button) has been pressed and released.</summary>
	BackMouseClick,
	
	/// <summary>Any key is being pressed.</summary>
	KeyDown,
	/// <summary>Any key has been pressed.</summary>
	KeyPressed,
	/// <summary>Any key has been released.</summary>
	KeyUp
}
