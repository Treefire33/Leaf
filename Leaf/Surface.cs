using System.Numerics;
using Leaf.UI;
using Leaf.UI.Theming;
using Raylib_cs;

namespace Leaf;

/// <summary>
/// Either a solid colour surface or a texture
/// </summary>
public class Surface
{
    public Color BackgroundColour { get; set; }
    public float BorderWidth { get; set; }
    public float BorderRadius { get; set; }
    public Color BorderColour { get; set; }
    public NPatchInfo NPatchInfo { get; set; }
    public Texture2D Texture { get; set; }

    public Surface(Texture2D texture, string id = "", string[]? classes = null, string element = "")
        : this(id, classes, element)
    {
        Texture = texture;
    }
    
    public Surface(string id = "", string[]? classes = null, string element = "")
    {
        ThemeSurface(id, classes, element);
    }

    public void ThemeSurface(string id = "", string[]? classes = null, string element = "")
    {
        classes ??= [];
        for (int i = 0; i < classes.Length; i++)
        {
            classes[i] = $".{classes[i]}";
        }
        UIThemeData themeData = UIManager.DefaultManager!.Theme.GetThemeDataFromObject(id, classes, element);
        BackgroundColour = themeData.GetProperty("background-color");
        BorderColour = themeData.GetProperty("border-top-color");
        BorderWidth = themeData.GetProperty("border-top-width");
        BorderRadius = themeData.GetProperty("border-top-left-radius") / 100f;
        NPatchInfo = themeData.GetProperty("nine-patch").AsNPatch(Texture);
    }
}