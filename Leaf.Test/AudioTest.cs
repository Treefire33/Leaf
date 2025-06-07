using System.Numerics;
using Leaf.Audio;
using Leaf.UI;
using Leaf.UI.GraphData;
using Leaf.Utilities;
using Raylib_cs;
using static Raylib_cs.Raylib;
using BlendMode = Leaf.Utilities.BlendMode;

namespace Leaf.Test;

public class AudioTest : ILeafTest
{
    public void Test(ref UIManager manager)
    {
        // Audio
        AudioManager.InitAudio();
        var musicGroup = new AudioGroup(0.85f);
        List<Audio.Audio> songs = [];
        foreach (var song in Directory.GetFiles(Resources.AudioRootPath))
        {
            songs.Add(AudioManager.LoadAudioFile(Path.GetFileName(song)));
        }
        int currentSong = 0;

        songs[currentSong].AudioGroup = musicGroup;
        songs[currentSong].Play();
        
        // Audio Controls
        var pausePlay = new UIButton(
            new UIRect(0, 40, 100, 100),
            "Pause",
            origin: new Vector2(0.5f, 0.5f)
        );
        pausePlay.SetAnchor(AnchorPosition.Center);
        var stopPlay = new UIButton(
            new UIRect(120, 0, 100, 100),
            "Stop"
        );
        stopPlay.SetAnchor(AnchorPosition.TopLeft, pausePlay);
        var loop = new UICheckbox(
            new UIRect(-120, 0, 100, 100)
        );
        loop.SetAnchor(AnchorPosition.TopLeft, pausePlay);
        loop.Checked = songs[currentSong].Clip.Looping;

        var songPosition = new UISlider(
            new UIRect(0, 160, 500, 24),
            origin: new Vector2(0.5f, 0.5f)
        );
        songPosition.SetAnchor(AnchorPosition.Center);
        var songTime = new UITextBox(
            new UIRect(0, 195, 800, 150),
            "00:00/00:00",
            origin: new Vector2(0.5f, 0),
            id: "time-tracker"
        );
        songTime.SetAnchor(AnchorPosition.Center);
        
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

        var skipSong = new UIButton(
            new UIRect(128, 128, 100, 100),
            "Skip"
        );

        skipSong.OnClick = (int mouseButton) =>
        {
            songs[currentSong].State = AudioState.None;
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

        return;
    }
}