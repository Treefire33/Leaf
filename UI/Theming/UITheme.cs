using System.Text;
using Newtonsoft.Json;
using Raylib_cs;

namespace Cattail.UI.Theming;

public class UITheme
{
	private Dictionary<string, UIElementTheme> _elementThemes = [];
	public Dictionary<ObjectID, UIElementAppearance> ElementThemes = [];

	public static UITheme LoadThemeFromFile(string filePath)
	{
		UITheme newTheme = new()
		{
			//Loaded theme *should* never be null
			_elementThemes = JsonConvert.DeserializeObject<
				Dictionary<string, UIElementTheme>
			>(File.ReadAllText(filePath))!
		};
		foreach (var theme in newTheme._elementThemes)
		{
			/*//@ = class or default element theme
			if (theme.Key.Contains('@') || theme.Key.Contains('#'))
			{
				newTheme._elementThemes[theme.Key] = newTheme.LoadPrototype(theme.Value);
			}*/
			newTheme._elementThemes[theme.Key] = newTheme.LoadPrototype(theme.Value);
		}
		
		return newTheme;
	}

	public (Font, int) GetElementFontEntry(UIElementTheme element, string _class)
	{
		return (
			GetFontFromName(element.Font!["name"]),
			int.Parse(element.Font!["size"])
		);
	}

	public Dictionary<string, Color> GetElementColours(UIElementTheme element, string _class)
	{
		Dictionary<string, Color> elementColours = [];
		foreach (var entry in element.Colour!)
		{
			elementColours.Add(entry.Key, GetColourFromHex(entry.Value));
		}
		if (_elementThemes.TryGetValue(_class, out UIElementTheme theme))
		{
			foreach (var miscEntry in theme.Colour!)
			{
				if (!elementColours.ContainsKey(miscEntry.Key))
				{
					elementColours.Add(miscEntry.Key, GetColourFromHex(miscEntry.Value));
				}
			}
		}
		return elementColours;
	}

	public Dictionary<string, dynamic> GetElementMiscEntries(UIElementTheme element, string _class)
	{
		Dictionary<string, dynamic> entries = [];
		foreach (var entry in element.Misc!)
		{
			entries.Add(entry.Key, entry.Value);
		}
		if (_elementThemes.TryGetValue(_class, out UIElementTheme theme))
		{
			foreach (var miscEntry in theme.Misc!)
			{
				if (!entries.ContainsKey(miscEntry.Key))
				{
					entries.Add(miscEntry.Key, miscEntry.Value);
				}
			}
		}
		return entries;
	}

	public Font GetFontFromName(string fontName)
	{
		return Resources.Fonts[fontName];
	}

	public Color GetColourFromHex(string hex)
	{
		int red = int.Parse(hex[1..3], System.Globalization.NumberStyles.HexNumber);
		int blue = int.Parse(hex[3..5], System.Globalization.NumberStyles.HexNumber);
		int green = int.Parse(hex[5..7], System.Globalization.NumberStyles.HexNumber);
		int alpha = hex.Length > 7 ? int.Parse(hex[7..9], System.Globalization.NumberStyles.HexNumber) : 255;

		return new Color(
			red,
			blue,
			green,
			alpha
		);
	}

	public UIElementTheme LoadPrototype(UIElementTheme theme)
	{
		UIElementTheme prototypeTheme = theme.Prototype != null 
			? _elementThemes[theme.Prototype] 
			: _elementThemes["default"];
		theme.Colour ??= [];
		theme.Font ??= [];
		theme.Misc ??= [];
		if (prototypeTheme.Prototype != null && theme.Prototype != null)
		{
			_elementThemes[theme.Prototype] = LoadPrototype(_elementThemes[theme.Prototype]);
		}
		foreach (KeyValuePair<string, string> colourEntry in prototypeTheme.Colour!)
		{
			if (!theme.Colour.ContainsKey(colourEntry.Key))
			{
				theme.Colour[colourEntry.Key] = colourEntry.Value;
			}
		}
		foreach (KeyValuePair<string, string> fontEntry in prototypeTheme.Font!)
		{
			if (!theme.Font.ContainsKey(fontEntry.Key))
			{
				theme.Font[fontEntry.Key] = fontEntry.Value;
			}
		}
		foreach (KeyValuePair<string, dynamic> miscEntry in prototypeTheme.Misc!)
		{
			if (!theme.Misc.ContainsKey(miscEntry.Key))
			{
				theme.Misc[miscEntry.Key] = miscEntry.Value;
			}
		}
		return theme;
	}

	public UIElementAppearance GetFromObjectID(ObjectID objectID)
	{
		if (ElementThemes.TryGetValue(objectID, out UIElementAppearance? value))
		{
			return value;
		}

		
		var theme = _elementThemes[objectID.ID];
		
		LoadPrototype(theme);
		
		UIElementAppearance newElementAppearance = new()
		{
			Font = GetElementFontEntry(theme, objectID.Class),
			Colours = GetElementColours(theme, objectID.Class),
			Miscellaneous = GetElementMiscEntries(theme, objectID.Class),
		};
		
		ElementThemes.Add(objectID, newElementAppearance);
		
		return newElementAppearance;
	}

	public override string ToString()
	{
		StringBuilder representation = new();
		foreach (var theme in _elementThemes)
		{
			representation.Append(theme.Key + "\n\t");
			representation.AppendLine(theme.Value.ToString());
		}
		return representation.ToString();
	}
}

public struct ObjectID(string id = "", string _class = "@default")
{
	public string ID = id;
	public string Class = _class;

	public static bool operator ==(ObjectID a, ObjectID b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(ObjectID a, ObjectID b)
	{
		return !a.Equals(b);
	}

	public override readonly bool Equals(object? obj)
	{
		if (obj != null && obj is ObjectID b)
		{
			return (ID == b.ID) && (Class == b.Class);
		}
		else
		{
			return false;
		}
	}

	public override readonly int GetHashCode() => base.GetHashCode();

	public override string ToString()
	{
		return $"ID: {ID}, Class: {Class}";
	}

	public static implicit operator ObjectID(string id)
	{
		return new ObjectID(id);
	}
}
