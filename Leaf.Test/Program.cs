using System.Numerics;
using Leaf.Audio;
using Leaf.UI;
using Leaf.UI.GraphData;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Test;

class Program
{
    private static int _testType = 1;
    static void Main(string[] args)
    {
        SetTraceLogLevel(TraceLogLevel.Error);
        InitWindow(800, 800, "Leaf UI Testing");
        
        SetTargetFPS(60);

        var manager = new UIManager(theme: "style.css", uiRootPath:".\\Resources\\UI\\");
        
        // Tests for UIElements
        if (_testType == 0)
        {
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
                valueStep: 25,
                anchor: ("top-left", textInput.GetPosition())
            );

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
                LoadTexture(".\\Resources\\Sprites\\weird_cat_i_found_on_the_sidewalk.png"),
                tooltip: "weird cat I found on the sidewalk"
            );

            // Containers
            var scrollContainer = new UIScrollingContainer(
                new UIRect(210, 410, 300, 300),
                enableScrollbars: true
            );

            // Graph Data
            /*var scatterPlot = new ScatterPlot("x axis", "y axis", [new Vector2(10, 25), new Vector2(25, 30)]);
            var graphNode = new UIGraphNode(
                new UIRect(310, 260, 300, 200),
                scatterPlot
            );*/

            // Actions
            int lastMouseButton = -1;
            float sliderDelta = 0;
            string textboxText =
                $"Last clicked button with: {lastMouseButton}\nSlider delta: {sliderDelta}\nSlider value: {slider.Value}";
            button.OnClick += (int mouseButton) => { lastMouseButton = mouseButton; };
            checkbox.OnClick += (int mouseButton) => { textbox.SetVisibility(!checkbox.Checked); };
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
                        new UIRect(0, 20 * scrollContainer.Elements.Count - 1, 300, 20),
                        c.ToString(),
                        container: scrollContainer
                    );
                }
            };
            slider.OnValueChanged += (float delta) => { sliderDelta = delta; };
            
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

        // Audio
        AudioManager.InitAudio();
        var audio = AudioManager.LoadAudioFile("Clouds May Come - The Weather Channel.mp3");
        audio.Play(true);
        
        // Audio Controls
        var pausePlay = new UIButton(
            new UIRect(0, 0, 100, 100),
            "Pause",
            origin: new Vector2(0.5f, 0.5f),
            anchor: ("center", new Vector2(0, 40))
        );
        var stopPlay = new UIButton(
            new UIRect(120, 0, 100, 100),
            "Stop"
        );
        stopPlay.SetAnchor("top-left", pausePlay);

        var songPosition = new UISlider(
            new UIRect(0, 120, 500, 24),
            origin: new Vector2(0.5f, 0.5f),
            anchor: ("center", new Vector2(0, 40))
        );
        
        pausePlay.OnClick = (int mouseButton) =>
        {
            switch (audio.State)
            {
                case AudioState.Playing:
                    pausePlay.SetText("Play");
                    audio.Pause();
                    break;
                case AudioState.Stopped:
                case AudioState.Paused:
                    pausePlay.SetText("Pause");
                    audio.Play();
                    break;
            }
        };

        stopPlay.OnClick = (int mouseButton) =>
        {
            audio.Stop();
            pausePlay.SetText("Play");
        };


        songPosition.MinValue = 0;
        songPosition.MaxValue = audio.ClipLength;
        songPosition.OnValueChanged = delta =>
        {
            if (songPosition.Focused)
            {
                audio.Seek(songPosition.Value);
            }
        };
        
        while (!WindowShouldClose())
        {
            BeginDrawing();
                ClearBackground(Color.White);
                AudioManager.Update();
                manager.Update(true);
                songPosition.Value = audio.Position;
            EndDrawing();
        }
    }
}