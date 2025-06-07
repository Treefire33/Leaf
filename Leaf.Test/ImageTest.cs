using System.Numerics;
using Leaf.Audio;
using Leaf.UI;
using Leaf.UI.GraphData;
using Leaf.Utilities;
using Raylib_cs;
using static Raylib_cs.Raylib;
using BlendMode = Leaf.Utilities.BlendMode;
using static Leaf.Utilities.Utilities;

namespace Leaf.Test;

public class ImageTest : ILeafTest
{
    public void Test(ref UIManager manager)
    {
        var strangeCatBase = Resources.LoadSprite("copyright_infringement.png");
        var blendBase = CreateEmptyImage(
            new Vector2(strangeCatBase.Width, strangeCatBase.Height), 
            new Color(127, 127, 127, 255)
        );

        bool currentAlphaMode = false;

        List<Texture2D> blendedImagesRgba = [];
        List<Texture2D> blendedImagesRgb = [];
        for (int i = 0; i < 12; i++)
        {
            blendedImagesRgba.Add(BlendImage(strangeCatBase, blendBase, BlendMode.Multiply + i));
            blendedImagesRgb.Add(BlendImage(strangeCatBase, blendBase, BlendMode.RgbMultiply + i));
        }

        int offsetX = 0;
        int offsetY = 0;
        string[] blendModes = [
            "Multiply",
            "Add",
            "Subtract",
            "Divide",
            "Min",
            "Max",
            "Screen",
            "Overlay",
            "Colour Dodge",
            "Colour Burn",
            "Difference",
            "Negation"
        ];
        List<UIImage> rgbaImages = [];
        List<UIImage> rgbImages = [];
        for (int i = 0; i < 12; i++)
        {
            var currentImageRgba = blendedImagesRgba[i];
            var currentImageRgb = blendedImagesRgb[i];
            DrawTextOnTexture(ref currentImageRgba, new(0, 0), blendModes[i], 20, Color.Black);
            DrawTextOnTexture(ref currentImageRgb, new(0, 0), blendModes[i], 20, Color.Black);
            
            rgbaImages.Add(new UIImage(
                new UIRect(offsetX, offsetY, 200, 200),
                blendedImagesRgba[i]
            ));
            rgbImages.Add(new UIImage(
                new UIRect(offsetX, offsetY, 200, 200),
                blendedImagesRgb[i]
            ));
            
            offsetX += 200;
            if (offsetX == 800)
            {
                offsetX = 0;
                offsetY += 200;
            }
        }

        _ = new UIImage(
            new UIRect(0, 600, 200, 200),
            strangeCatBase
        );
        
        while (!WindowShouldClose())
        {
            BeginDrawing();
                ClearBackground(Color.White);
                manager.Update(true);

                if (IsKeyPressed(KeyboardKey.Space))
                {
                    currentAlphaMode = !currentAlphaMode;
                    foreach (var img in rgbaImages)
                    {
                        img.SetVisibility(currentAlphaMode);
                    }

                    foreach (var img in rgbImages)
                    {
                        img.SetVisibility(!currentAlphaMode);
                    }
                }
                DrawText($"Blending Alpha: {currentAlphaMode}", 200, 600, 12, Color.Black);
            EndDrawing();
        }

        return;
    }
}