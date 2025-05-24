using System.Numerics;
using System.Xml;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf;

public static partial class Resources
{
    
    private static string _uiButtonsPath = @"Spritesheets\";
    public static string UIButtonsPath => $@"{UIRootPath}{_uiButtonsPath}";
    
    private static string _uiThemesPath = @"Themes\";
    public static string UIThemesPath => $@"{UIRootPath}{_uiThemesPath}";
    

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

    public static void LoadUIAssets()
    {
        if (Directory.Exists(UIButtonsPath))
        {
            var spritesheetXml = new XmlDocument();
            foreach (var file in Directory.GetFiles(UIButtonsPath))
            {
                if (!file.EndsWith(".xml")) { continue; }
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
        var imagePath = UIButtonsPath + spritesheetXml.GetAttribute("image");
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
}