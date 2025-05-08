using System.Numerics;
using System.Xml;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf;

public static partial class Resources
{
    private static string _uiImagesPath = @"Images\";
    private static string _uiSpritesheetsPath = @"Spritesheets\";
    private static string _uiThemesPath = @"Themes\";
    private static string _uiFontsPath = @"Fonts\";
    public static string UIImagesPath => $@"{UIRootPath}{_uiImagesPath}";
    public static string UISpritesheetsPath => $@"{UIRootPath}{_uiSpritesheetsPath}";
    public static string UIThemesPath => $@"{UIRootPath}{_uiThemesPath}";
    public static string UIFontsPath => $@"{UIRootPath}{_uiFontsPath}";
    
    public static readonly Dictionary<string, Font> Fonts = new()
    {
        {"default", GetFontDefault()}
    };

    public static readonly Dictionary<string, List<Texture2D>> Buttons = [];
    
    public static NPatchInfo GenerateNPatchInfoFromButton(Texture2D button)
    {
        return new NPatchInfo
        {
            Source = new Rectangle(0, 0, button.Width, button.Height),
            Left = button.Width / 3,
            Right = button.Width / 3,
            Top = button.Height / 3,
            Bottom = button.Height / 3,
            Layout = NPatchLayout.NinePatch
        };
    }
    
    public static void LoadDefaultUIAssets()
    {
        if (Directory.Exists(".\\Assets\\UI\\Fonts\\"))
        {
            foreach (var file in Directory.GetFiles(".\\Assets\\UI\\Fonts\\"))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Fonts.Add(name, LoadFontUI(file));
            }
        }
        if (Directory.Exists(".\\Assets\\UI\\Spritesheets\\") && Directory.Exists(".\\Assets\\UI\\Images\\"))
        {
            var spritesheetXml = new XmlDocument();
            Span<string> tempPaths = [RootPath,_uiRootPath,_uiImagesPath];
            RootPath = ".\\Assets\\";
            _uiRootPath = "UI\\";
            _uiImagesPath = "Images\\";
            foreach (var file in Directory.GetFiles(".\\Assets\\UI\\Spritesheets\\"))
            {
                spritesheetXml.Load(file);
                if (spritesheetXml.DocumentElement!.Name == "SpriteAtlases")
                {
                    foreach (XmlElement sprAtlas in spritesheetXml.DocumentElement!.GetElementsByTagName("SpriteAtlas"))
                    {
                        LoadSpritesheetXml(sprAtlas);
                    }
                }
                else
                {
                    LoadSpritesheetXml((XmlElement)spritesheetXml.GetElementsByTagName("SpriteAtlas")[0]!);
                }
            }

            RootPath = tempPaths[0];
            _uiRootPath = tempPaths[1];
            _uiImagesPath = tempPaths[2];
        }
    }

    public static void LoadUIAssets()
    {
        MoveDefaultAssets();
        if (Directory.Exists(UIFontsPath))
        {
            foreach (var file in Directory.GetFiles(UIFontsPath))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Fonts[name] = LoadFontUI(file);
            }
        }
        if (Directory.Exists(UISpritesheetsPath) && Directory.Exists(UIImagesPath))
        {
            var spritesheetXml = new XmlDocument();
            foreach (var file in Directory.GetFiles(UISpritesheetsPath))
            {
                spritesheetXml.Load(file);
                if (spritesheetXml.DocumentElement!.Name == "SpriteAtlases")
                {
                    foreach (XmlElement sprAtlas in spritesheetXml.DocumentElement!.GetElementsByTagName("SpriteAtlas"))
                    {
                        LoadSpritesheetXml(sprAtlas);
                    }
                }
                else
                {
                    LoadSpritesheetXml((XmlElement)spritesheetXml.GetElementsByTagName("SpriteAtlas")[0]!);
                }
            }
        }
    }

    private static void LoadSpritesheetXml(XmlElement spritesheetXml)
    {
        var imagePath = UIImagesPath + spritesheetXml.GetAttribute("image");
        var name = spritesheetXml.GetAttribute("name");
        Vector2 cellSize = new(
            float.Parse(spritesheetXml.GetAttribute("cellX")),
            float.Parse(spritesheetXml.GetAttribute("cellY"))
        );
        
        List<Texture2D> buttonTextures = [];
        Image spritesheet = LoadImage(imagePath);
        foreach (XmlNode subTex in spritesheetXml.GetElementsByTagName("SubTexture"))
        {
            var x = int.Parse(subTex.Attributes?["x"]?.Value ?? "0");
            var y = int.Parse(subTex.Attributes?["y"]?.Value ?? "0");
            var img = ImageFromImage(spritesheet, new Rectangle(
                new Vector2(x, y) * cellSize,
                cellSize
            ));
            buttonTextures.Add(LoadTextureFromImage(img));
            UnloadImage(img);
        }
        
        UnloadImage(spritesheet);

        Buttons[name] = buttonTextures;
    }
    public static Texture2D LoadSprite(string file)
    {
        return LoadTexture($"{UIImagesPath}{file}");
    }

    private static unsafe Font LoadFontUI(string fontName)
    {
        Font loadedFont = LoadFontEx(fontName, 64, null, 0);
        //GenTextureMipmaps(&loadedFont.Texture);
        SetTextureFilter(loadedFont.Texture, TextureFilter.Anisotropic8X);
        return loadedFont;
    }
}