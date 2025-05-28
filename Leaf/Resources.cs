using System.Numerics;
using System.Runtime.Loader;
using System.Text;
using System.Xml;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf;

public static partial class Resources
{
    public static string RootPath = @".\Assets\";
    
    private static string _imagesPath = @"Images\";
    public static string ImagesPath => $@"{RootPath}{_imagesPath}";
    
    private static string _fontsPath = @"Fonts\";
    public static string FontsPath => $@"{RootPath}{_fontsPath}";
    
    private static string _spritesheetsPath = @"Spritesheets\";
    public static string SpritesheetsPath => $@"{RootPath}{_spritesheetsPath}";
    
    private static string _uiRootPath = $@"UI\";
    public static string UIRootPath => $@"{RootPath}{_uiRootPath}";
    
    private static string _audioRootPath = $@"Audio\";
    public static string AudioRootPath => $@"{RootPath}{_audioRootPath}";
    
    public static readonly Dictionary<string, Font> Fonts = new()
    {
        {"default", GetFontDefault()}
    };
    
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
            case ResourcesPath.Images:
                _imagesPath = path;
                break;
            case ResourcesPath.Fonts:
                _fontsPath = path;
                break;
            case ResourcesPath.Spritesheets:
                _spritesheetsPath = path;
                break;
            case ResourcesPath.UI:
                _uiRootPath = path;
                break;
            case ResourcesPath.UIThemes:
                _uiThemesPath = path;
                break;
            case ResourcesPath.UIButtons:
                _uiButtonsPath = path;
                break;
        }
    }

    private static void MoveDefaultAssets()
    {
        void MoveDir(string dir)
        {
            var curDir = new DirectoryInfo(dir);
            var copyTo = dir switch
            {
                ".\\Assets\\" => RootPath,
                ".\\Assets\\Fonts\\" => FontsPath,
                ".\\Assets\\Images\\" => ImagesPath,
                ".\\Assets\\Spritesheets\\" => SpritesheetsPath,
                ".\\Assets\\Audio\\" => AudioRootPath,
                ".\\Assets\\UI\\" => UIRootPath,
                ".\\Assets\\UI\\Buttons\\" => UIButtonsPath,
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
                file.MoveTo(copyTo+file.Name, true);
            }
            Directory.Delete(dir);
        }
        
        if (Directory.Exists(".\\Assets\\") && RootPath != ".\\Assets\\" && Directory.Exists(RootPath))
        {
            MoveDir(".\\Assets\\");
        }
    }
    
    public static void InitResources()
    {
        MoveDefaultAssets();
        if (Directory.Exists(FontsPath))
        {
            foreach (var file in Directory.GetFiles(FontsPath))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Fonts[name] = LoadFont(file);
            }
        }
        LoadUIAssets();
    }
    
    public static Texture2D LoadSprite(string file)
    {
        return LoadTexture($"{ImagesPath}{file}");
    }

    public static Texture2D[] LoadSpritesheet(string spritesheet)
    {
        var spritesheetXml = new XmlDocument();
        using StreamReader stream = new($"{SpritesheetsPath}{spritesheet}", Encoding.UTF8);
        
        if (!File.Exists(spritesheet+".xml"))
        { throw new Exception($"Spritesheet file with name: {spritesheet} does not exist."); }
            
        spritesheetXml.Load(stream.ReadToEnd());
        stream.Close();
        
        if (spritesheetXml.DocumentElement!.Name == "SpriteAtlases")
        {
            foreach (XmlElement sprAtlas in spritesheetXml.DocumentElement!.GetElementsByTagName("SpriteAtlas"))
            {
                return LoadSpritesheetXml(sprAtlas);
            }
        }
        else
        {
            return LoadSpritesheetXml((XmlElement)spritesheetXml.GetElementsByTagName("SpriteAtlas")[0]!);
        }

        return [];
    }

    private static Texture2D[] LoadSpritesheetXml(XmlElement spritesheetXml)
    {
        var imagePath = ImagesPath + spritesheetXml.GetAttribute("image");
        var name = spritesheetXml.GetAttribute("name");
        Vector2 cellSize = new(
            float.Parse(spritesheetXml.GetAttribute("cellX")),
            float.Parse(spritesheetXml.GetAttribute("cellY"))
        );
        
        List<Texture2D> textures = [];
        Image spritesheetImg = LoadImage(imagePath);
        foreach (XmlNode subTex in spritesheetXml.GetElementsByTagName("SubTexture"))
        {
            var x = int.Parse(subTex.Attributes?["x"]?.Value ?? "0");
            var y = int.Parse(subTex.Attributes?["y"]?.Value ?? "0");
            var img = ImageFromImage(spritesheetImg, new Rectangle(
                new Vector2(x, y) * cellSize,
                cellSize
            ));
            textures.Add(LoadTextureFromImage(img));
            UnloadImage(img);
        }
        
        UnloadImage(spritesheetImg);

        return textures.ToArray();
    }
    
    private static unsafe Font LoadFont(string fontName)
    {
        Font loadedFont = LoadFontEx(fontName, 64, null, 0);
        //GenTextureMipmaps(&loadedFont.Texture);
        SetTextureFilter(loadedFont.Texture, TextureFilter.Anisotropic8X);
        return loadedFont;
    }
}