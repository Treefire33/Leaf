namespace Leaf;

public static partial class Resources
{
    public static string RootPath = @"Assets\";
    public static string UIRootPath = $@"{RootPath}UI\";
    public static string AudioRootPath = $@"{RootPath}Audio\";

    public static void LoadDefault()
    {
        LoadUIAssets();
    }

    public static void LoadAssets()
    {
        LoadUIAssets();
    }
    
    public static void SetRootPath(string rootPath)
    {
        RootPath = rootPath;
        UIRootPath = $"{RootPath}UI\\";
        AudioRootPath = $"{RootPath}Audio\\";
    }
}