using System.Numerics;
using Leaf.Events;
using Leaf.UI.Theming;
using Raylib_cs;

#pragma warning disable CS8618, CS9264

namespace Leaf.UI;

public class UIManager
{
	public static UIManager? DefaultManager;
	/// <summary>
	/// Refers to the size of a render texture, ignore if you aren't rendering to one.
	/// If ignored, defaults to the size of the window.
	/// </summary>
	public static Vector2 GameSize;
	/// <summary>
	/// Refers to the position of a render texture, ignore if you aren't rendering to one
	/// If ignored, defaults to (0, 0).
	/// </summary>
	public static Vector2 GamePosition;
	/// <summary>
	/// When enabled, all elements will have a red outline drawn around them.
	/// </summary>
	public static bool DebugMode = false;
	
	private static void SetDefaultManager(UIManager manager)
	{
		DefaultManager = manager;
	}

	public static UIManager GetDefaultManager()
	{
		DefaultManager ??= new UIManager();
		return DefaultManager;
	}
	
	public UIContainer? Container;
	public List<Event> UIEvents = [];
	public UITheme Theme;

	public UIManager(
		Vector2 gameSize = default, 
		Vector2 gamePosition = default,
		params string[] themes
	)
	{
		if (gameSize == default)
		{
			gameSize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
		}
		if (gamePosition == default)
		{
			gamePosition = Vector2.Zero;
		}
		GameSize = gameSize;
		GamePosition = gamePosition;
		UITheme.LoadDefaultTheme(); // Load default theme
		if (themes.Length > 0)
			foreach (var theme in themes)
			{
				LoadTheme(theme);
			}
		if (DefaultManager == null)
		{
			SetDefaultManager(this);
		}	
		GetDefaultContainer();
	}
	
	/// <summary>
	/// Draws the default container, which then draws all ui elements.
	/// </summary>
	public void DrawUI()
	{
		Container!.Update();
	}
	
	/// <summary>
	/// Calls the event processor for the default container, which then calls the event processor for all ui elements.
	/// </summary>
	public void ProcessEvents()
	{
		foreach (Event evnt in UIEvents)
			Container!.ProcessEvent(evnt);
	}
	
	private int _lastKey;
	/// <summary>
	/// Processes various keyboard events and pushes them to the events list.
	/// </summary>
	public void PushInputEvents()
	{
		int keyPressed = Raylib.GetKeyPressed();
		while (keyPressed != 0)
		{
			PushEvent(new Event(keyPressed, EventType.KeyPressed));
			_lastKey = keyPressed;
			
			keyPressed = Raylib.GetKeyPressed();
		}
		
		if (_lastKey != 0 && Raylib.IsKeyDown((KeyboardKey)_lastKey))
		{
			PushEvent(new Event(_lastKey, EventType.KeyDown));
		}
		else if (_lastKey != 0 && Raylib.IsKeyReleased((KeyboardKey)_lastKey))
		{
			PushEvent(new Event(_lastKey, EventType.KeyUp));
		}
	}
	
	/// <summary>
	/// Combines DrawUI, PushKeyEvents, and ProcessEvents together.
	/// Typically used in cases where processing events is not needed.
	/// </summary>
	/// <param name="flushEvents">Should the events list be cleared at the end of the frame.</param>
	public void Update(bool flushEvents = false)
	{
		DrawUI();
		
		PushInputEvents();

		ProcessEvents();

		if (flushEvents)
		{
			ResetEvents();
		}
	}

	public void LoadTheme(string themePath = "")
	{
		Theme = UITheme.LoadTheme(themePath);
	}

	public void PushEvent(Event newEvent)
	{
		UIEvents.Add(newEvent);
	}

	public void ResetEvents()
	{
		UIEvents.Clear();
	}

	public UIContainer? GetDefaultContainer()
	{
		if (Container != null)
		{
			return Container;
		}

		Container = new UIContainer(
			new UIRect(0, 0, GameSize),
			visible: true,
			isRootContainer: true
		);

		return Container;
	}
}
