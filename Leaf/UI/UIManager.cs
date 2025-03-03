using System.Numerics;
using Cattail.UI.Events;
using Cattail.UI.Interfaces;
using Cattail.UI.Theming;
using Raylib_cs;
#pragma warning disable CS8618, CS9264

namespace Cattail.UI;

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
	
	public List<UIElement> Elements = [];
	public UIContainer? Container;
	public List<Event> UIEvents = [];
	public UITheme Theme;
	public bool IsFocused = false;

	public UIManager(Vector2 gameSize = default, string theme = "default.json")
	{
		if (gameSize == default)
		{
			gameSize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
		}
		GameSize = gameSize;
		LoadTheme(Resources.UIThemesPath + theme);
		if (DefaultManager == null)
		{
			SetDefaultManager(this);
		}	
		GetDefaultContainer();
		
		Resources.LoadAssets();
	}

	public void DrawUI()
	{
		Container!.Update();
	}

	public void LoadTheme(string themePath)
	{
		Theme = UITheme.LoadThemeFromFile(themePath);
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
