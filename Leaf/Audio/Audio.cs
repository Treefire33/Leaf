using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Audio;

/// <summary>
/// An audio clip.
/// </summary>
/// <param name="clip">The raylib music stream to use.</param>
public class Audio(Music clip)
{
    private Music _clip = clip;
    /// <summary>Current raylib music stream.</summary>
    public Music Clip => _clip;
    
    /// <summary>Current state of the audio.</summary>
    public AudioState State = AudioState.None;

    /// <summary>The AudioGroup to adjust the audio with.</summary>
    public AudioGroup? AudioGroup;
    
    /// <summary>The length (in seconds) of the clip.</summary>
    public float Length => GetMusicTimeLength(Clip);
    
    /// <summary>The current position (in seconds) of the clip.</summary>
    public float Position => GetMusicTimePlayed(Clip);
    
    /// <summary>The looping state of the clip.</summary>
    public bool Looping
    {
        get => _clip.Looping;
        set => _clip.Looping = value;
    }
    
    /// <summary>Fires when Play() is called.</summary>
    public Action? Played;
    /// <summary>Fires when Stop() is called.</summary>
    public Action? Stopped;
    /// <summary>Fires when Pause() is called.</summary>
    public Action? Paused;
    /// <summary>
    /// Fires when Position >= Length and the clip is no longer playing but has not been stopped.
    /// </summary>
    public Action? Finished;

    public void Play()
    {
        State = AudioState.Playing;
        
        if (State == AudioState.Paused)
        {
            ResumeMusicStream(_clip);
            return;
        }

        PlayMusicStream(_clip);
        Played?.Invoke();
    }

    public void Pause()
    {
        State = AudioState.Paused;
        
        PauseMusicStream(_clip);
        Paused?.Invoke();
    }

    public void Stop()
    {
        if (State == AudioState.Paused)
        {
            ResumeMusicStream(_clip);
        }
        
        State = AudioState.Stopped;
        
        StopMusicStream(_clip);
        Stopped?.Invoke();
    }
    
    /// <summary>
    /// Changes the current position in the clip.
    /// </summary>
    /// <param name="position">The position in the clip to seek to.</param>
    public void Seek(float position)
    {
        if (position < 0) position = 0;
        if (position > Length) position = Length;
        
        SeekMusicStream(Clip, position);
    }

    public void Update()
    {
        if ((!IsMusicStreamPlaying(_clip) || Position >= Length) && State == AudioState.Playing)
        {
            State = AudioState.None;
            Finished?.Invoke();
            return;
        }
        
        SetMusicVolume(_clip, AudioGroup?.Volume ?? 1);
        SetMusicPitch(_clip, AudioGroup?.Pitch ?? 1);
        SetMusicPan(_clip, AudioGroup?.Pan ?? 0.5f);
        
        UpdateMusicStream(_clip);
    }

    public static implicit operator Music(Audio clip)
    {
        return clip.Clip;
    }
}