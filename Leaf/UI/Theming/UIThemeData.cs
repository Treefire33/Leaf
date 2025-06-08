using System.Text.RegularExpressions;
using ExCSS;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Leaf.UI.Theming;

public struct UIThemeData
{
    public static StyleRule DefaultRule { get; set; } = null!;
    private StyleRule? _elementRule;
    private StyleRule? _classRule;
    private StyleRule? _idRule;
    
    public UIThemeData(StyleRule? elementRule, StyleRule[]? classRule, StyleRule? idRule)
    {
        _elementRule = elementRule;
        if (classRule is { Length: > 0 })
        {
            _classRule = classRule[0];
            foreach (StyleRule styleRule in classRule)
            {
                foreach (IProperty prop in styleRule.Style.ToArray())
                {
                    _classRule.Style.SetProperty(prop.Name, prop.Value);
                }
            }
        }
        _idRule = idRule;
    }

    public ThemeProperty GetProperty(string propertyName, string defaultValue = "")
    {
        string? idProperty = _idRule?.Style.GetPropertyValue(propertyName);
        if (idProperty == string.Empty) { idProperty = null; }
        string? classProperty = _classRule?.Style.GetPropertyValue(propertyName);
        if (classProperty == string.Empty) { classProperty = null; }
        string? elementProperty = _elementRule?.Style.GetPropertyValue(propertyName);
        if (elementProperty == string.Empty) { elementProperty = null; }
        string? defaultProperty = DefaultRule.Style.GetPropertyValue(propertyName);
        if (defaultProperty == string.Empty) { defaultProperty = null; }

        string property = idProperty ?? classProperty ?? elementProperty ?? defaultProperty ?? defaultValue;
        return property;
    }

    public override string ToString()
    {
        return $"{_idRule?.ToCss()}\n{_classRule?.ToCss()}\n{_elementRule?.Style.CssText}";
    }
}

public partial struct ThemeProperty
{
    private string _value;

    public static implicit operator ThemeProperty(string value)
    {
        return new ThemeProperty { _value = value };
    }
    
    public static implicit operator string(ThemeProperty value)
    {
        return value._value;
    }
    
    public Color AsColor()
    {
        if (string.IsNullOrEmpty(_value)) { return Color.White; }

        MatchCollection numbers = ColorRegexPattern().Matches(_value);
        List<float> colors = [];
        foreach (Match match in numbers)
        {
            colors.Add(float.Parse(match.Value));
        }
        
        Color color = new(
            colors[0]/255f, 
            colors[1]/255f, 
            colors[2]/255f, 
            colors.Count == 4 ? colors[3]/255f : 1
        );
        
        return color;
    }
    
    private static float AsFloat(string value)
    {
        return string.IsNullOrEmpty(value) ?
            1f
            : float.Parse(ValueRegexPattern().Match(value).Value);
    }
    
    private static int AsInt(string value)
    {
        return string.IsNullOrEmpty(value) ?
            1
            : int.Parse(ValueRegexPattern().Match(value).Value);
    }
    
    public float AsFloat() => AsFloat(_value);
    
    public float[] AsFloats()
    {
        string[] splits = _value.Split(' ');
        float[] finalFloats = new float[splits.Length];
        
        for (int i = 0; i < splits.Length; i++)
        {
            finalFloats[i] = AsFloat(splits[i]);
        }

        return finalFloats;
    }
    
    public int AsInt() => AsInt(_value);
    
    public int[] AsInts()
    {
        string[] splits = _value.Split(' ');
        int[] finalFloats = new int[splits.Length];
        
        for (int i = 0; i < splits.Length; i++)
        {
            finalFloats[i] = AsInt(splits[i]);
        }

        return finalFloats;
    }

    public LeafFont AsFont()
    {
        return Resources.GetFont(_value);
    }

    public List<Texture2D> AsButtonImages(string fallback = "default")
    {
        if (string.IsNullOrEmpty(_value)) { return Resources.Buttons[fallback]; }
        return Resources.Buttons.TryGetValue(_value, out List<Texture2D>? images) ? images : Resources.Buttons[fallback];
    }
    
    public List<Texture2D> AsCheckmarks(string fallback = "checkmarks")
    {
        if (string.IsNullOrEmpty(_value)) { return Resources.CheckmarkStyles[fallback]; }
        return Resources.CheckmarkStyles.TryGetValue(_value, out List<Texture2D>? images) ? images : Resources.CheckmarkStyles[fallback];
    }

    public NPatchInfo AsNPatch(Texture2D button)
    {
        if (string.IsNullOrEmpty(_value))
        {
            return Resources.GenerateNPatchInfoFromButton(button);
        }

        string[] npatchArgs = _value.Split(' ');
        NPatchLayout layout = NPatchLayout.NinePatch;
        int leftOffset = 0;
        int topOffset = 0;
        int rightOffset = 0;
        int bottomOffset = 0;

        int ConvertPercentage(string value, bool percentHeight = false)
        {
            float val = float.Parse(ValueRegexPattern().Match(value).Value);
            if (value.Contains('%'))
            {
                return (int)((percentHeight ? button.Height : button.Width) * (val/100));
            }
            
            return (int)val;
        }

        for (int i = 0; i < npatchArgs.Length; i++)
        {
            if (i == 0)
            {
                layout = npatchArgs[i] switch
                {
                    "nine-patch" => NPatchLayout.NinePatch,
                    "three-patch-horizontal" =>  NPatchLayout.ThreePatchHorizontal,
                    "three-patch-vertical" =>  NPatchLayout.ThreePatchVertical,
                    _ =>  NPatchLayout.NinePatch
                };
                continue;
            }

            if (npatchArgs.Length <= 2)
            {
                leftOffset = ConvertPercentage(_value);
                topOffset = ConvertPercentage(_value, true);
                rightOffset = ConvertPercentage(_value);
                bottomOffset = ConvertPercentage(_value, true);
                break;
            }

            switch (i)
            {
                case 1:
                    leftOffset = ConvertPercentage(_value);
                    break;
                case 2:
                    topOffset = ConvertPercentage(_value, true);
                    break;
                case 3:
                    rightOffset = ConvertPercentage(_value);
                    break;
                case 4:
                    bottomOffset = ConvertPercentage(_value, true);
                    break;
            }
        }

        NPatchInfo nInfo = new()
        {
            Source = new Rectangle(0, 0, button.Width, button.Height),
            Left = leftOffset,
            Top = topOffset,
            Right = rightOffset,
            Bottom = bottomOffset,
            Layout = layout
        };
        
        return nInfo;
    }
    
    // Implicit conversions
    public static implicit operator Color(ThemeProperty property) => property.AsColor();
    public static implicit operator int(ThemeProperty property) => property.AsInt();
    public static implicit operator float(ThemeProperty property) => property.AsFloat();
    public static implicit operator LeafFont(ThemeProperty property) => property.AsFont();
    public static implicit operator List<Texture2D>(ThemeProperty property) => property.AsButtonImages();

    [GeneratedRegex(@"[0-9]\w+|0")]
    private static partial Regex ColorRegexPattern();
    
    [GeneratedRegex(@"[0-9]\d+|[0-9]{1}")]
    private static partial Regex ValueRegexPattern();
}