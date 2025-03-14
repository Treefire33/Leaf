using ExCSS;

namespace Cattail.UI.Theming;

public class UITheme
{
    public Stylesheet Stylesheet { get; init; }
    public static UITheme LoadTheme(string stylesheet)
    {
        var stylesheetParser = new StylesheetParser(true, true, tolerateInvalidValues: true);
        var stylesheetData = stylesheetParser.Parse(File.ReadAllText(stylesheet));
        
        var theme = new UITheme
        {
            Stylesheet = stylesheetData
        };
        
        var defaultRule = stylesheetData.StyleRules.Where(x => x.SelectorText == "*");
        IEnumerable<IStyleRule> styleRules = defaultRule.ToList();
        if (styleRules.Any())
        {
            UIThemeData.DefaultRule = (styleRules.First() as StyleRule)!;
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