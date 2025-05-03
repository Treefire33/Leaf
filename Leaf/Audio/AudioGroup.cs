using Leaf.UI;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Audio;

public class AudioGroup(float volume = 1f, float pitch = 1f, float pan = 0.5f)
{
    public float Volume = volume;
    public float Pitch = pitch;
    public float Pan = pan;
}