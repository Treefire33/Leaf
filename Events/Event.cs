using Leaf.UI;
using Raylib_cs;

namespace Leaf.Events;

public partial class Event
{
	public Event(int keyCode, EventType type)
	{
		KeyCode = (KeyboardKey)keyCode;
		EventType = type;
	}

	public override string ToString()
	{
		return $"UIEvent of type: {EventType}";
	}
	
	public KeyboardKey KeyCode = 0;
	public EventType EventType;
}
