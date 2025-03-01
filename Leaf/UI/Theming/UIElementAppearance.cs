using Raylib_cs;

namespace Cattail.UI.Theming;

public class UIElementAppearance
{
	public Dictionary<string, Color> Colours = [];
	public (Font, int) Font;
	public Dictionary<string, dynamic> Miscellaneous = [];
}
