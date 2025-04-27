using System.Numerics;
using Leaf.UI;
using Leaf.UI.GraphData;
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
        
        // Decorative Elements
        var textbox = new UITextBox(
            new UIRect(310, 10, 300, 250),
            ""
        );
        var panel= new UIPanel(
            new UIRect(0, 0, 800, 400)
        );
        panel.Layer = -1;
        var imageWithTooltip = new UIImage(
            new UIRect(10, 410, 200, 200),
            LoadTexture(".\\Resources\\Sprites\\weird_cat_i_found_on_the_sidewalk.png"),
            tooltip: "weird cat I found on the sidewalk"
        );
        
        // Containers
        var scrollContainer = new UIScrollingContainer(
            new UIRect(210, 410, 300, 300)
        );
        
        // Graph Data
        var scatterPlot = new ScatterPlot("x axis", "y axis", [new Vector2(10, 25), new Vector2(25, 30)]);
        var graphNode = new UIGraphNode(
            new UIRect(310, 260, 300, 200),
            scatterPlot
        );
        
        // Actions
        int lastMouseButton = -1;
        float sliderDelta = 0;
        string textboxText = $"Last clicked button with: {lastMouseButton}\nSlider delta: {sliderDelta}\nSlider value: {slider.Value}";
        button.OnClick += (int mouseButton) =>
        {  
            lastMouseButton = mouseButton;
        };
        checkbox.OnClick += (int mouseButton) =>
        {
            textbox.SetVisibility(!checkbox.Checked);
        };
        textInput.OnTextChanged += () =>
        {
            for (int i = scrollContainer.Elements.Count - 1; i >= 0; i--)
            {
                scrollContainer.Elements.ElementAt(i).Kill();
            }
            scrollContainer.Elements.Clear();

            foreach (char c in textInput.Text)
            {
                _ = new UITextBox(
                    new UIRect(0, 60 * scrollContainer.Elements.Count - 1, 300, 50),
                    c.ToString(),
                    container: scrollContainer
                );
            }
        };
        slider.OnValueChanged += (float delta) =>
        {
            sliderDelta = delta;
        };

        while (!WindowShouldClose())
        {
            BeginDrawing();
            ClearBackground(Color.White);
                textboxText = $"Last clicked button with: {lastMouseButton}\nSlider delta: {sliderDelta}\nSlider value: {slider.Value}";
                manager.Update(true);
                textbox.SetText(textboxText);
            EndDrawing();
        }
    }
}