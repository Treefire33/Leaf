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
	MiddleMouseDown,
	MiddleMouseUp,
	MiddleMouseClick,
	KeyDown,
	KeyPressed,
	KeyUp
}
