using Leaf.UI;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Audio;

public static class AudioManager
{
    public static string AudioPath => "Resources\\Audio\\";
    
    private static readonly List<Audio> _audios = [];
    
    public static void InitAudio()
    {
        InitAudioDevice();
    }

    public static Audio LoadAudioFile(string fileName)
    {
        Music music = LoadMusicStream(AudioPath + fileName);

        Audio audio = new(music);
        _audios.Add(audio);
        return audio;
    }

    public static void Update()
    {
        foreach (Audio audio in _audios.Where(audio => audio.State == AudioState.Playing))
        {
            audio.Update();
        }
    }
}