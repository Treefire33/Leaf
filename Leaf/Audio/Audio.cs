using Leaf.UI;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Audio;

public class Audio(Music clip)
{
    private Music _clip = clip;
    public Music Clip => _clip;
    public AudioState State = AudioState.None;
    
    public float ClipLength => GetMusicTimeLength(Clip);
    public float Position => GetMusicTimePlayed(Clip);

    public void Play(bool loops = false)
    {
        State = AudioState.Playing;
        
        _clip.Looping = loops;
        
        if (State == AudioState.Paused)
        {
            ResumeMusicStream(Clip);
            return;
        }

        PlayMusicStream(Clip);
    }

    public void Pause()
    {
        State = AudioState.Paused;
        
        PauseMusicStream(Clip);
    }

    public void Stop()
    {
        State = AudioState.Stopped;
        
        StopMusicStream(Clip);
    }

    public void Seek(float position)
    {
        if (position < 0) position = 0;
        if (position > ClipLength) position = ClipLength;
        
        SeekMusicStream(Clip, position);
    }
    
    public void SetLooping(bool loop)
    {
        _clip.Looping = loop;
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