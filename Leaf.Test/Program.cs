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
        InitWindow(800, 400, "CattailUI");

        var manager = new UIManager(theme: "style.css", uiRootPath:".\\Resources\\");
        
        UIManager.DebugMode = true;

        var slider = new UISlider(
            new UIRect(10, 10, 150, 50),
            0, 100
        );
        
        var textboxSlider = new UITextBox(
            new UIRect(10, 60, 150, 100),
            "Slider Val: 0"
        );

        slider.OnValueChanged += delegate
        {
            textboxSlider.SetText($"Current Value: {slider.Value}");
        };

        var button = new UIButton(
            new UIRect(0, 100, 150, 50),
            "Button",
            @class: "left-button",
            tooltip: "button tooltip!"
        );
        
        button.SetAnchor("top-left", slider);
        
        var textbox = new UITextBox(
            new UIRect(300, 10, 200, 300),
            "Lorem ipsum dolor sit amet"
        );
        
        button.OnClick += delegate(int mouseButton) 
        { 
            Console.WriteLine($"Subscribed click {
                    mouseButton switch {
                        0 => "Left Click",
                        1 => "Right Click",
                        2 => "Middle Click"
                    }
                }."
            );
            
            textbox.SetText($"{
                mouseButton switch {
                0 => "Left Click",
                1 => "Right Click",
                2 => "Middle Click"
            }}");
        };

        button.OnHover += delegate
        {
            textbox.SetText("Button hovered");
        };

        var autoContainer = new UIAutoResizableContainer(
            new UIRect(300, 400, 0, 0)
        );
        
        Console.WriteLine(autoContainer.RelativeRect.Size);

        var tempElement = new UIElement(new UIRect(100, 100, 200, 200), container: autoContainer);
        
        Console.WriteLine(autoContainer.RelativeRect.Size);

        var checkbox = new UICheckbox(
            new UIRect(200, 10, 50, 50)
        );

        checkbox.OnClick += i =>
        {
            Console.WriteLine($"Checked: {checkbox.Checked}");
        };

        while (!WindowShouldClose())
        {
            BeginDrawing();
            ClearBackground(Color.White);
                manager.Update();
                
                slider.Value += 15f * GetFrameTime();
                if (slider.Value >= slider.MaxValue)
                {
                    slider.Value = slider.MinValue;
                }
                
                manager.ResetEvents();
            EndDrawing();
        }
    }
}