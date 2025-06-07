using ExCSS;

namespace Leaf.UI.Theming;

public class UITheme
{
    private Stylesheet Stylesheet { get; init; } = null!;
    
    public UIThemeData GetThemeDataFromObject(string id, string @class, string element)
    {
        IEnumerable<IStyleRule> idRules = Stylesheet.StyleRules.Where(x => x.SelectorText == id);
        IEnumerable<IStyleRule> classRules = Stylesheet.StyleRules.Where(x => x.SelectorText == @class);
        IEnumerable<IStyleRule> elementRules = Stylesheet.StyleRules.Where(x => x.SelectorText == element);
        StyleRule? idRule = idRules.FirstOrDefault() as StyleRule;
        StyleRule? classRule = classRules.FirstOrDefault() as StyleRule;
        StyleRule? elementRule = elementRules.FirstOrDefault() as StyleRule;
        UIThemeData themeData = new(elementRule, classRule, idRule);
        return themeData;
    }
    
    private static readonly StylesheetParser StylesheetParser = new(true, true, tolerateInvalidValues: true);
    private static Stylesheet _defaultStylesheet = null!;

    public static void LoadDefaultTheme()
    {
        Stylesheet stylesheetData = StylesheetParser.Parse(File.ReadAllText($"{Resources.UIThemesPath}default.css"));

        List<IStyleRule> defaultRule = stylesheetData.StyleRules.Where(x => x.SelectorText == "*").ToList();
        if (defaultRule.Count > 0)
        {
            UIThemeData.DefaultRule = (defaultRule.First() as StyleRule)!;
        }
        else
        {
            throw new Exception("Default rule of default theme not found.");
        }
        
        _defaultStylesheet = stylesheetData;
    }
    
    public static UITheme LoadTheme(string stylesheet = "")
    {
        // Create new theme with default stylesheet loaded in.
        UITheme theme = new()
        {
            Stylesheet = _defaultStylesheet
        };
        if (stylesheet == "") { return theme; }
        
        // Load new stylesheet.
        Stylesheet stylesheetData = StylesheetParser.Parse(File.ReadAllText(Resources.UIThemesPath + stylesheet));
        // Get new style rules.
        IEnumerable<IStyleRule> currentStyles = theme.Stylesheet.StyleRules;
    
        // Iterate over default style rules. Used to overwrite and add to defaults.
        foreach (IStyleRule? styleRule in stylesheetData.StyleRules)
        {
            // Get properties of current rule.
            List<IProperty> properties = styleRule.Style.ToList();
            // Attempt to match new rule with existing one.
            IStyleRule? matchedRule = currentStyles.FirstOrDefault(x => x.SelectorText == styleRule.SelectorText);
            
            if (matchedRule is not null)
            {
                // There is an existing rule, append new properties to rule.
                foreach (IProperty prop in properties)
                {
                    matchedRule.Style.SetProperty(prop.Name, prop.Value);
                }
            }
            else
            {
                // New rule, append to stylesheet.
                theme.Stylesheet.AppendChild(styleRule);
            }
        }

        return theme;
    }
}