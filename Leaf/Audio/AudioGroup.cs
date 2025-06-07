namespace Leaf.Audio;

/// <summary>
/// Stores values for volume, pitch, and pan that is then used by any Audio assigned to it.
/// </summary>
/// <param name="volume">The volume of the group.</param>
/// <param name="pitch">The pitch of the group.</param>
/// <param name="pan">The pan of the group.</param>
public class AudioGroup(float volume = 1f, float pitch = 1f, float pan = 0.5f)
{
    public float Volume = volume;
    public float Pitch = pitch;
    public float Pan = pan;
}