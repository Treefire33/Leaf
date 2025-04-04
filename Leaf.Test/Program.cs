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
        
        //UIManager.DebugMode = true;

        /*var tempElement = new UIScrollingContainer(new UIRect(20, 20, 400, 200), true, true);
        var containedElement = new UIElement(new UIRect(10, 10, 100, 450), container: tempElement);
        var contained2Element = new UIElement(new UIRect(150, 10, 100, 450), container: tempElement);
        contained2Element.SetAnchor("top-left", containedElement);
        /*var container = new UIContainer(new UIRect(0, 0, 300, 300), origin:Vector2.One/2, anchor:("center", Vector2.Zero));
        var temp2Element = new UIElement(new UIRect(0, 0, 100, 100), origin:Vector2.One/2, anchor:("center", Vector2.Zero));
        Console.WriteLine(container.GetPosition());
        container.AddElement(temp2Element);
        Console.WriteLine(temp2Element.GetPosition());#1#
        
        Console.WriteLine(containedElement.GetPosition());*/
        
        /*var _effectivenessContainer = new UIScrollingContainer(
            new UIRect(30, 60, 740, 240),
            verticalScroll: true
        );
        var _interactions = new UITextBox(
            new UIRect(0, 0, 740, 900),
            "qwertyuiopefwguhfebwhfehbwfewhjfkewhjfew",
            container: _effectivenessContainer
        );*/

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