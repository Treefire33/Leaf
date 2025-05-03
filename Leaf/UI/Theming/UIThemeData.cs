using System.Text.RegularExpressions;
using ExCSS;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Leaf.UI.Theming;

public struct UIThemeData
{
    public static StyleRule DefaultRule;
    private StyleRule? _elementRule;
    private StyleRule? _classRule;
    private StyleRule? _idRule;

    public UIThemeData(StyleRule? elementRule, StyleRule? classRule, StyleRule? idRule)
    {
        _elementRule = elementRule;
        _classRule = classRule;
        _idRule = idRule;
    }

    public ThemeProperty GetProperty(string propertyName, string defaultValue = "")
    {
        var idProperty = _idRule?.Style.GetPropertyValue(propertyName);
        if (idProperty == string.Empty) { idProperty = null; }
        var classProperty = _classRule?.Style.GetPropertyValue(propertyName);
        if (classProperty == string.Empty) { classProperty = null; }
        var elementProperty = _elementRule?.Style.GetPropertyValue(propertyName);
        if (elementProperty == string.Empty) { elementProperty = null; }
        var defaultProperty = DefaultRule.Style.GetPropertyValue(propertyName);
        if (defaultProperty == string.Empty) { defaultProperty = null; }

        var property = idProperty ?? classProperty ?? elementProperty ?? defaultProperty ?? defaultValue;
        return property;
    }

    public override string ToString()
    {
        return $"{_idRule?.ToCss()}\n{_classRule?.ToCss()}\n{_elementRule?.Style.CssText}";
    }
}

public partial struct ThemeProperty
{
    public string? Value;

    public static implicit operator ThemeProperty(string value)
    {
        return new ThemeProperty { Value = value };
    }
    
    public static implicit operator string(ThemeProperty value)
    {
        return value.Value;
    }
    
    public Color AsColor()
    {
        if (string.IsNullOrEmpty(Value)) { return Color.White; }

        var numbers = ColorRegexPattern().Matches(Value);
        List<float> colors = [];
        foreach (Match match in numbers)
        {
            colors.Add(float.Parse(match.Value));
        }
        
        var color = new Color(colors[0]/255f, colors[1]/255f, colors[2]/255f, colors.Count == 4 ? colors[3]/255f : 1);
        
        return color;
    }

    public float AsFloat()
    {
        if (string.IsNullOrEmpty(Value)) { return 1; }
        return float.Parse(ValueRegexPattern().Match(Value).Value);
    }
    
    public int AsInt()
    {
        if (string.IsNullOrEmpty(Value)) { return 1; }
        return int.Parse(ValueRegexPattern().Match(Value).Value);
    }

    public Font AsFont()
    {
        if (string.IsNullOrEmpty(Value)) { return Resources.Fonts["default"]; }
        return Resources.Fonts.TryGetValue(Value, out Font font) ? font : Resources.Fonts["default"];
    }

    public List<Texture2D> AsButtonImages()
    {
        if (string.IsNullOrEmpty(Value)) { return Resources.Buttons["default"]; }
        return Resources.Buttons.TryGetValue(Value, out List<Texture2D> images) ? images : Resources.Buttons["default"];
    }
    
    public List<Texture2D> AsCheckboxImages()
    {
        if (string.IsNullOrEmpty(Value)) { return Resources.Buttons["checkbox"]; }
        return Resources.Buttons.TryGetValue(Value, out List<Texture2D> images) ? images : Resources.Buttons["checkbox"];
    }

    [GeneratedRegex(@"[0-9]\w+|0")]
    private static partial Regex ColorRegexPattern();
    
    [GeneratedRegex(@"[0-9]\d+|[0-9]{1}")]
    private static partial Regex ValueRegexPattern();
}