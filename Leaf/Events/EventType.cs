namespace Leaf.Events;

[Flags]
public enum EventType
{
	None,
	LeftMouseDown,
	LeftMouseUp,
	LeftMouseClick,
	RightMouseDown,
	RightMouseUp,
	RightMouseClick,
	MouseDown, // Applies to both left, right, middle, etc. down
	MouseUp, // Applies to both left, right, middle, etc. up
	MouseClick, // Applies to both left, right, middle, etc. click
	KeyDown,
	KeyPressed,
	KeyUp
}
