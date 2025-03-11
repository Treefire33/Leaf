using ExCSS;

namespace Cattail.UI.Theming;

public class UITheme
{
    public List<StyleRule> Rules = [];
    public static UITheme LoadTheme(string stylesheet)
    {
        var stylesheetParser = new StylesheetParser();
        var stylesheetData = stylesheetParser.Parse(File.ReadAllText(stylesheet));
        
        var theme = new UITheme();
        theme.Rules.AddRange(stylesheetData.StyleRules as IEnumerable<StyleRule> ?? []);

        return theme;
    }

    public StyleRule? GetRuleFromObject(string id, string @class, string element)
    {
        
    }
}