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
    
    public static void MoveDefaultAssets()
    {
        void MoveDir(string dir)
        {
            var curDir = new DirectoryInfo(dir);
            var copyTo = dir switch
            {
                ".\\Assets\\" => RootPath,
                ".\\Assets\\Audio\\" => AudioRootPath,
                ".\\Assets\\UI\\" => UIRootPath,
                ".\\Assets\\UI\\Fonts\\" => UIFontsPath,
                ".\\Assets\\UI\\Images\\" => UIImagesPath,
                ".\\Assets\\UI\\Spritesheets\\" => UISpritesheetsPath,
                ".\\Assets\\UI\\Themes\\" => UIThemesPath
            };
            foreach (var dirInfo in curDir.GetDirectories())
            {
                MoveDir(dir+dirInfo.Name+"\\");
            }

            if (!Directory.Exists(copyTo))
            {
                Directory.CreateDirectory(copyTo);
            }

            foreach (var file in curDir.GetFiles())
            {
                if (File.Exists(copyTo + file.Name))
                {
                    file.Delete();
                    continue;
                }
                file.MoveTo(copyTo+file.Name);
            }
            Directory.Delete(dir);
        }
        if (Directory.Exists(".\\Assets\\") && RootPath != ".\\Assets\\")
        {
            MoveDir(".\\Assets\\");
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