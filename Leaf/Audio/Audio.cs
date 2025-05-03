using Leaf.UI;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Audio;

public class Audio(Music clip)
{
    private Music _clip = clip;
    public Music Clip => _clip;
    public AudioState State = AudioState.None;

    public AudioGroup AudioGroup;
    
    public float Length => GetMusicTimeLength(Clip);
    public float Position => GetMusicTimePlayed(Clip);

    public bool Looping
    {
        get => _clip.Looping;
        set => _clip.Looping = value;
    }

    public Action? Played;
    public Action? Stopped;
    public Action? Paused;
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
        
        SetMusicVolume(_clip, AudioGroup.Volume);
        SetMusicPitch(_clip, AudioGroup.Pitch);
        SetMusicPan(_clip, AudioGroup.Pan);
        
        UpdateMusicStream(_clip);
    }

    public static implicit operator Music(Audio clip)
    {
        return clip.Clip;
    }
}

public enum AudioState
{
    None,
    Stopped,
    Paused,
    Playing
}