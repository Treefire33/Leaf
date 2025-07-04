using Raylib_cs;

namespace Leaf;

public class LeafFont
{
    public readonly string Name;
    private Font _regular;
    private Font? _italic;
    private Font? _bold;
    private Font? _boldItalic;

    public LeafFont(
       string name,
       int fontSize = 16,
       string regularFontPath = "",
       string italicFontPath = "",
       string boldFontPath = "",
       string boldItalicFontPath = "",
       int[]? extraCodepoints = null
    )
    { 
        Name = name;
        if (!string.IsNullOrEmpty(regularFontPath))
            _regular = Resources.LoadFont(Resources.FontsPath+regularFontPath, fontSize, extraCodepoints);
        if (!string.IsNullOrEmpty(italicFontPath))
            _italic = Resources.LoadFont(Resources.FontsPath+italicFontPath, fontSize, extraCodepoints);
        if (!string.IsNullOrEmpty(boldFontPath))
            _bold = Resources.LoadFont(Resources.FontsPath+boldFontPath, fontSize, extraCodepoints);
        if (!string.IsNullOrEmpty(boldItalicFontPath))
            _boldItalic = Resources.LoadFont(Resources.FontsPath+boldItalicFontPath, fontSize, extraCodepoints);
    }

    public LeafFont(string name, Font font)
    {
        Name = name;
        _regular = font;
    }

    public Font this[FontStyle index]
    {
        get
        {
            return index switch
            {
                FontStyle.Regular => _regular,
                FontStyle.Italic => _italic ??  _regular,
                FontStyle.Bold => _bold ?? _regular,
                FontStyle.BoldItalic => _boldItalic ??  _regular,
                _ => _regular,
            };
        }
    }

    public static implicit operator Font(LeafFont font)
    {
        return font[FontStyle.Regular];
    }
}

public enum FontStyle
{
    Regular,
    Italic,
    Bold,
    BoldItalic
}