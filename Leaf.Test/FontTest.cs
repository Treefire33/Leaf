using System.Numerics;
using Leaf.Audio;
using Leaf.UI;
using Leaf.UI.GraphData;
using Leaf.Utilities;
using Raylib_cs;
using static Raylib_cs.Raylib;
using BlendMode = Leaf.Utilities.BlendMode;

namespace Leaf.Test;

public class FontTest : ILeafTest
{
    public void Test(ref UIManager manager)
    {
        /*// Decorative Elements
        var textbox = new UITextBox(
            new UIRect(310, 10, 300, 250),
            ""
        );*/

        float yOffset = 0;
        int currentFont = 0;
        string[] fonts = [
            "clearsans",
            "m6x11plus"
        ];
        int reset = 0;
        TextureFilter filter = 0;
        SetTextureFilter(((Font)Resources.GetFont(fonts[currentFont])).Texture, filter);
        while (!WindowShouldClose())
        {
            BeginDrawing();
                ClearBackground(Color.White);
                manager.Update(true);
                
                if (IsKeyPressed(KeyboardKey.Space))
                {
                    currentFont++;
                    if (currentFont == fonts.Length)
                    {
                        currentFont = 0;
                    }
                }

                if (IsKeyPressed(KeyboardKey.Up))
                {
                    filter += 1;
                    if (filter > TextureFilter.Anisotropic16X)
                    {
                        filter = 0;
                    }
                    SetTextureFilter(((Font)Resources.GetFont(fonts[currentFont])).Texture, filter);
                }
                
                yOffset = reset;
                for (int i = 1; i <= 120; i++)
                {
                    DrawTextEx(
                        Resources.GetFont(fonts[currentFont]), 
                        $"Font size: {i}, filter: {filter}", 
                        new Vector2(0, yOffset), 
                        i,
                        1,
                        Color.Black
                    );
                    yOffset += MeasureTextEx(Resources.GetFont(fonts[currentFont]), $"Font size: {i}", i, 1).Y;
                }

                reset -= 2;
                if (-reset >= yOffset)
                {
                    reset = 0;
                }

                EndDrawing();
        }

        return;
    }
}