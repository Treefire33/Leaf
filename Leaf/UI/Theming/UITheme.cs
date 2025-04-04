using ExCSS;

namespace Leaf.UI.Theming;

public class UITheme
{
    private static readonly StylesheetParser _stylesheetParser = new(true, true, tolerateInvalidValues: true);
    private static Stylesheet _defaultStylesheet;
    public Stylesheet Stylesheet { get; init; }

    public static void LoadDefaultTheme()
    {
        var stylesheetData = _stylesheetParser.Parse(File.ReadAllText($"{Resources.UIThemesPath}default.css"));

        var defaultRule = stylesheetData.StyleRules.Where(x => x.SelectorText == "*");
        IEnumerable<IStyleRule> styleRules = defaultRule.ToList();
        if (styleRules.Any())
        {
            UIThemeData.DefaultRule = (styleRules.First() as StyleRule)!;
        }
        
        _defaultStylesheet = stylesheetData;
    }
    
    public static UITheme LoadTheme(string stylesheet = "")
    {
        var theme = new UITheme
        {
            Stylesheet = _defaultStylesheet
        };
        
        // todo: not this
        if (stylesheet != "")
        {
            var stylesheetData = _stylesheetParser.Parse(File.ReadAllText(Resources.UIThemesPath + stylesheet));
            var currentStyles = theme.Stylesheet.StyleRules;

            foreach (var styleRule in stylesheetData.StyleRules)
            {
                var newRules = styleRule.Style.ToList();
                var matchedRule = currentStyles.FirstOrDefault(x => x.SelectorText == styleRule.SelectorText);
                if (matchedRule is not null)
                {
                    foreach (var prop in newRules)
                    {
                        matchedRule.Style.SetProperty(prop.Name, prop.Value);
                    }
                }
                else
                {
                    theme.Stylesheet.AppendChild(styleRule);
                }
            }
        }

        return theme;
    }

    public UIThemeData GetThemeDataFromObject(string id, string @class, string element)
    {
        var idRules = Stylesheet.StyleRules.Where(x => x.SelectorText == id).ToList();
        var classRules = Stylesheet.StyleRules.Where(x => x.SelectorText == @class).ToList();
        var elementRules = Stylesheet.StyleRules.Where(x => x.SelectorText == element).ToList();
        StyleRule? idRule = idRules.FirstOrDefault() as StyleRule;
        StyleRule? classRule = classRules.FirstOrDefault() as StyleRule;
        StyleRule? elementRule = elementRules.FirstOrDefault() as StyleRule;
        var themeData = new UIThemeData(elementRule, classRule, idRule);
        return themeData;
    }
}