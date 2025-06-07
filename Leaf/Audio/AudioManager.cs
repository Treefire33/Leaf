using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Audio;

/// <summary>
/// A manager for all audios loaded.
/// Must call InitAudio() to play audio.
/// Must be updated for loaded audios to play.
/// </summary>
public static class AudioManager
{
    private static readonly List<Audio> _audios = [];
    
    /// <summary>
    /// Initializes audio devices, must be called to play audio.
    /// </summary>
    public static void InitAudio()
    {
        InitAudioDevice();
    }
    
    /// <summary>
    /// Loads an audio clip from the audio resource path.
    /// </summary>
    /// <param name="fileName">The name of the audio to load.</param>
    /// <returns>A new audio.</returns>
    public static Audio LoadAudioFile(string fileName)
    {
        Music music = LoadMusicStream(Resources.AudioRootPath + fileName);

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