using System.Numerics;
using Leaf.Events;
using Leaf.UI.Theming;
using Raylib_cs;

#pragma warning disable CS8618, CS9264

namespace Leaf.UI;

public class UIManager
{
	public static UIManager? DefaultManager;
	public static Vector2 GameSize;
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

	public UIManager(Vector2 gameSize = default, string theme = "", string uiRootPath = "")
	{
		if (gameSize == default)
		{
			gameSize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
		}
		GameSize = gameSize;
		if (uiRootPath != "") Resources.SetResourcesPath(ResourcesPath.UI, uiRootPath);
		Resources.LoadUIAssets();
		UITheme.LoadDefaultTheme(); // Load default theme
		LoadTheme(theme);
		if (DefaultManager == null)
		{
			SetDefaultManager(this);
		}	
		GetDefaultContainer();
	}

	public void DrawUI()
	{
		Container!.Update();
	}

	private void ProcessEvents()
	{
		foreach (Event evnt in UIEvents)
			Container!.ProcessEvent(evnt);
	}
	
	private int _lastKey = 0;
	public void Update(bool flushEvents = false)
	{
		DrawUI();
		
		int keyPressed = Raylib.GetKeyPressed();
		if (keyPressed != 0 && !Raylib.IsKeyDown((KeyboardKey)_lastKey))
		{
			PushEvent(new Event(keyPressed, EventType.KeyPressed));
			_lastKey = keyPressed;
		}
		else
		{
			PushEvent(new Event(_lastKey, EventType.KeyDown));
		}

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
