namespace Leaf;

public static partial class Resources
{
    public static string RootPath = @".\Assets\";
    private static string _uiRootPath = $@"UI\";
    private static string _audioRootPath = $@"Audio\";
    public static string UIRootPath => $@"{RootPath}{_uiRootPath}";
    public static string AudioRootPath => $@"{RootPath}{_audioRootPath}";
    
    public static void SetResourcesPath(ResourcesPath pathToSet, string path)
    {
        switch (pathToSet)
        {
            case ResourcesPath.Root:
                RootPath = path;
                break;
            case ResourcesPath.Audio:
                _audioRootPath = path;
                break;
            case ResourcesPath.UI:
                _uiRootPath = path;
                break;
            case ResourcesPath.UIImages:
                _uiImagesPath = path;
                break;
            case ResourcesPath.UIFonts:
                _uiFontsPath = path;
                break;
            case ResourcesPath.UIThemes:
                _uiThemesPath = path;
                break;
            case ResourcesPath.UISpritesheets:
                _uiSpritesheetsPath = path;
                break;
        }
    }
}

public enum ResourcesPath
{
    Root,
    UI,
    UIFonts,
    UIImages,
    UISpritesheets,
    UIThemes,
    Audio
}