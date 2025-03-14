using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public static class Resources
{
    public static string UIRootPath = @".\Assets\UI\";
    public static string UIImagesPath = $@"{UIRootPath}Images\";
    public static string UIThemesPath = $@"{UIRootPath}Themes\";
    public static string UIFontsPath = $@"{UIRootPath}Fonts\";
    
    public static readonly Dictionary<string, Font> Fonts = new()
    {
        {"default", GetFontDefault()}
    };

    public static readonly Dictionary<string, List<Texture2D>> Buttons = new()
    {
        {"default", GenButtonsFromName("default", true)}
    };
    
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

    public static List<Texture2D> GetButtonImagesFromStyle(string style)
    {
        return Buttons[style];
    }
    
    public static List<Texture2D> GenButtonsFromName(string name, bool isSpritesheet)
    {
        if (isSpritesheet)
        {
            List<Texture2D> buttonTextures = [];
            Image spritesheet = LoadImage($@"{UIImagesPath}{name}_images.png");
            Vector2 spritesheetSize = new(96, 96);
            int rows = (int)(spritesheet.Height / spritesheetSize.Y);
            int columns = (int)(spritesheet.Width / spritesheetSize.X);
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    buttonTextures.Add(LoadTextureFromImage(ImageFromImage(spritesheet, new Rectangle(
                        new Vector2(x, y) * spritesheetSize,
                        spritesheetSize
                    ))));
                }
            }
            
            return buttonTextures;
        }

        Texture2D loadedNormal = LoadTexture($@"{UIImagesPath}{name}.png");
        Texture2D loadedHover = LoadTexture($@"{UIImagesPath}{name}_hovered.png");
        Texture2D loadedDisabled = LoadTexture($@"{UIImagesPath}{name}_disabled.png");
            
        return [
            loadedNormal,
            loadedHover,
            loadedDisabled
        ];
    }

    public static void LoadAssets()
    {
        if (Directory.Exists(UIFontsPath))
        {
            foreach (var file in Directory.GetFiles(UIFontsPath))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                Fonts.Add(name, LoadFontUI(file));
            }
        }
        if (Directory.Exists(UIImagesPath))
        {
            foreach (var file in Directory.GetFiles(UIImagesPath))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                if (!Buttons.ContainsKey(name.Replace("_images", "")))
                {
                    Buttons.Add(name.Replace("_images", ""), GenButtonsFromName(name.Replace("_images", ""), isSpritesheet: true));
                }
            }
        }
    }

    private static unsafe Font LoadFontUI(string fontName)
    {
        Font loadedFont = LoadFontEx(fontName, 36, null, 0);
        GenTextureMipmaps(&loadedFont.Texture);
        SetTextureFilter(loadedFont.Texture, TextureFilter.Anisotropic8X);
        return loadedFont;
    }
}