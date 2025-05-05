using System.Numerics;
using System.Xml;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf;

public static partial class Resources
{
    public static string UIImagesPath => $@"{UIRootPath}Images\";
    public static string UISpritesheetsPath => $@"{UIRootPath}Spritesheets\";
    public static string UIThemesPath => $@"{UIRootPath}Themes\";
    public static string UIFontsPath => $@"{UIRootPath}Fonts\";
    
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

    public static void SetRoot(string rootPath)
    {
        UIRootPath = rootPath;
    }

    public static void LoadUIAssets()
    {
        if (Directory.Exists(UIFontsPath))
        {
            foreach (var file in Directory.GetFiles(UIFontsPath))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Fonts.Add(name, LoadFontUI(file));
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
            buttonTextures.Add(LoadTextureFromImage(ImageFromImage(spritesheet, new Rectangle(
                new Vector2(x, y) * cellSize,
                cellSize
            ))));
        }
        
        Buttons.Add(name, buttonTextures);
    }

    private static unsafe Font LoadFontUI(string fontName)
    {
        Font loadedFont = LoadFontEx(fontName, 64, null, 0);
        GenTextureMipmaps(&loadedFont.Texture);
        SetTextureFilter(loadedFont.Texture, TextureFilter.Anisotropic8X);
        return loadedFont;
    }
}