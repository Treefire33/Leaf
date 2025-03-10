using Raylib_cs;

namespace Cattail.UI.Theming;

public class UIElementAppearance
{
	public Dictionary<string, Color> Colours = [];
	public (Font, int) Font;
	public Dictionary<string, dynamic> Miscellaneous = [];

	public Color GetColour(string key)
	{
		return Colours.TryGetValue(key, out Color colour) ? colour : Color.White;
	}

	public dynamic? GetMiscellaneous(string key)
	{
		return Miscellaneous.TryGetValue(key, out dynamic? miscellaneous) ? miscellaneous : null;
	}

	public override string ToString()
	{
		string str = "";
		foreach (KeyValuePair<string, Color> pair in Colours)
		{
			str += pair.Key + ": " + pair.Value + Environment.NewLine;
		}
		foreach (KeyValuePair<string, dynamic> pair in Miscellaneous)
		{
			str += pair.Key + ": " + pair.Value + Environment.NewLine;
		}
		return $"Appearance:\n{str}";
	}
}
