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
            string textboxText =
                $"Last clicked button with: {lastMouseButton}\nSlider value: {slider.Value}";
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
            
            while (!WindowShouldClose())
            {
                BeginDrawing();
                    ClearBackground(Color.White);
                    textboxText = $"Last clicked button with: {lastMouseButton}\nSlider value: {slider.Value}";
                    manager.Update(true);
                    textbox.SetText(textboxText);
                EndDrawing();
            }

            return;
        }

        // Audio
        AudioManager.InitAudio();
        var musicGroup = new AudioGroup(0.85f);
        List<Audio.Audio> songs =
        [
            AudioManager.LoadAudioFile("clouds_may_come.mp3"),
            AudioManager.LoadAudioFile("buildings_have_eyes.mp3")
        ];
        int currentSong = 0;

        songs[currentSong].AudioGroup = musicGroup;
        songs[currentSong].Play();
        
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
        var loop = new UICheckbox(
            new UIRect(-120, 0, 100, 100)
        );
        loop.SetAnchor("top-left", pausePlay);
        loop.Checked = songs[currentSong].Clip.Looping;

        var songPosition = new UISlider(
            new UIRect(0, 120, 500, 24),
            origin: new Vector2(0.5f, 0.5f),
            anchor: ("center", new Vector2(0, 40))
        );
        var songTime = new UITextBox(
            new UIRect(0, 155, 800, 150),
            "00:00/00:00",
            origin: new Vector2(0.5f, 0),
            anchor: ("center", new Vector2(0, 40)),
            id: "time-tracker"
        );
        
        pausePlay.OnClick = (int mouseButton) =>
        {
            switch (songs[currentSong].State)
            {
                case AudioState.Playing:
                    pausePlay.SetText("Play");
                    songs[currentSong].Pause();
                    break;
                case AudioState.Stopped:
                case AudioState.Paused:
                    pausePlay.SetText("Pause");
                    songs[currentSong].Play();
                    break;
            }
        };

        stopPlay.OnClick = (int mouseButton) =>
        {
            songs[currentSong].Stop();
            pausePlay.SetText("Play");
        };

        loop.OnClick += (int mouseButton) =>
        {
            songs[currentSong].Looping = loop.Checked;
        };


        songPosition.MinValue = 0;
        songPosition.MaxValue = songs[currentSong].Length;

        var volumeSlider = new UISlider(
            new UIRect(0, 300, 32, 150),
            value: musicGroup.Volume,
            valueStep: 0.05f,
            scrollDirection: ScrollDirection.VerticalBottom
        );
        var pitchSlider = new UISlider(
            new UIRect(32, 300, 32, 150),
            minValue: -2,
            maxValue: 2,
            value: musicGroup.Pitch,
            valueStep: 0.05f,
            scrollDirection: ScrollDirection.VerticalBottom
        );
        var panSlider = new UISlider(
            new UIRect(0, 268, 100, 32),
            value: musicGroup.Pan,
            valueStep: 0.1f
        );

        float timeToSeekTo = 0f;
        songPosition.OnValueChanged = () =>
        {
            timeToSeekTo = songPosition.Value;
            
            songTime.SetText($"{songPosition.Value}/{songs[currentSong].Length}\nVolume: {volumeSlider.Value}\nPitch: {pitchSlider.Value}\nPan: {panSlider.Value}");
        };

        songPosition.OnMouseUp = (int mouseButton) =>
        {
            songs[currentSong].Seek(timeToSeekTo);
            timeToSeekTo = 0f;
        };
        
        while (!WindowShouldClose())
        {
            BeginDrawing();
                ClearBackground(Color.White);
                AudioManager.Update();
                manager.Update(true);
                musicGroup.Volume = volumeSlider.Value;
                musicGroup.Pitch = pitchSlider.Value;
                musicGroup.Pan = 1 - panSlider.Value;
                
                if (!songPosition.Focused)
                {
                    songPosition.Value = songs[currentSong].Position;
                }

                if (songs[currentSong].State == AudioState.None)
                {
                    if (currentSong + 1 >= songs.Count)
                    {
                        currentSong = 0;
                    }
                    else
                    {
                        currentSong++;
                    }
            
                    songs[currentSong].AudioGroup = musicGroup;
                    songs[currentSong].Looping = loop.Checked;
                    songs[currentSong].Play();
            
                    songPosition!.MaxValue = songs[currentSong].Length;
                }

                if (IsKeyPressed(KeyboardKey.A))
                {
                    panSlider.Value -= 0.1f;
                }
                else if (IsKeyPressed(KeyboardKey.D))
                {
                    panSlider.Value += 0.1f;
                }

                if (IsKeyPressed(KeyboardKey.W))
                {
                    volumeSlider.Value += 0.05f;
                }
                else if (IsKeyPressed(KeyboardKey.S))
                {
                    volumeSlider.Value -= 0.05f;
                }
            EndDrawing();
        }
    }
}