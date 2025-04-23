using System.Numerics;
using Leaf.UI;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Test;

class Program
{
    static void Main(string[] args)
    {
        SetTraceLogLevel(TraceLogLevel.Error);
        InitWindow(800, 800, "Leaf UI Testing");
        
        SetTargetFPS(60);

        var manager = new UIManager(theme: "style.css", uiRootPath:".\\Resources\\");
        
        // Interactable Elements
        var button = new UIButton(
            new UIRect(10, 10, 150, 50),
            "Test Button"
        );
        var checkbox = new UICheckbox(
            new UIRect(160, 0, 50, 50),
            anchor: ("top-left", button.GetPosition())
        );
        var textInput = new UITextInput(
            new UIRect(0, 60, 200, 24),
            "",
            255,
            anchor: ("top-left", button.GetPosition())
        );
        var slider = new UISlider(
            new UIRect(0, 60, 200, 50),
            0, 100,
            anchor: ("top-left", textInput.GetPosition())
        );

        while (!WindowShouldClose())
        {
            BeginDrawing();
            ClearBackground(Color.White);
                manager.Update(true);
            EndDrawing();
        }
    }
}