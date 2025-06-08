using System.Numerics;
using Leaf.Audio;
using Leaf.UI;
using Leaf.UI.GraphData;
using Leaf.Utilities;
using Raylib_cs;
using static Raylib_cs.Raylib;
using BlendMode = Leaf.Utilities.BlendMode;

namespace Leaf.Test;

public class UIElementTest : ILeafTest
{
    public void Test(ref UIManager manager)
    {
        // Interactable Elements
        var button = new UIButton(
            new UIRect(10, 10, 150, 50),
            "Test Button"
        );
        var checkbox = new UICheckbox(
            new UIRect(160, 0, 50, 50)
        );
        checkbox.SetAnchor(AnchorPosition.TopLeft, button);
        var textInput = new UITextInput(
            new UIRect(0, 60, 200, 24),
            "",
            255
        );
        textInput.SetAnchor(AnchorPosition.TopLeft, button);
        var slider = new UISlider(
            new UIRect(0, 60, 200, 50),
            0, 100,
            valueStep: 25
        );
        slider.SetAnchor(AnchorPosition.TopLeft, textInput);

        // Decorative Elements
        var textbox = new UITextBox(
            new UIRect(310, 10, 300, 250),
            ""
        );
        var panel = new UIPanel(
            new UIRect(0, 0, 800, 400)
        );
        panel.Layer = -1;
        var imageWithTooltip = new UIImage(
            new UIRect(10, 410, 200, 200),
            Resources.LoadSprite("weird_cat_i_found_on_the_sidewalk.png"),
            tooltip: "weird cat I found on the sidewalk"
        );

        // Containers
        var scrollContainer = new UIScrollingContainer(
            new UIRect(210, 410, 100, 300),
            enableScrollbars: true
        );
        var dropdown = new UIDropdown(
            new UIRect(310, 410, 200, 80),
            ["option 1", "option 2", "option 3", "option 4"]
        );

        // Graph Data
        /*var scatterPlot = new ScatterPlot("x axis", "y axis", [new Vector2(10, 25), new Vector2(25, 30)]);
        var graphNode = new UIGraphNode(
            new UIRect(310, 260, 300, 200),
            scatterPlot
        );*/

        // Actions
        int lastMouseButton = -1;
        string textboxText =
            $"Last clicked button with: {lastMouseButton}\nSlider value: {slider.Value}";
        button.OnClick += (int mouseButton) => { lastMouseButton = mouseButton; };
        checkbox.OnClick += (int mouseButton) => { textbox.SetVisibility(!checkbox.Checked); };
        textInput.OnTextChanged += () =>
        {
            scrollContainer.ClearElements();

            foreach (char c in textInput.Text)
            {
                _ = new UITextBox(
                    new UIRect(0, 20 * scrollContainer.Elements.Count - 1, 100, 20),
                    c.ToString(),
                    container: scrollContainer
                );
            }
        };
        
        while (!WindowShouldClose())
        {
            BeginDrawing();
                ClearBackground(Color.White);
                textboxText = $"Last clicked button with: {lastMouseButton}\nSlider value: {slider.Value}";
                manager.Update(true);
                textbox.SetText(textboxText);
            EndDrawing();
        }
    }
}