namespace Leaf.Audio;

/// <summary>
/// The current playing state of an audio.
/// </summary>
public enum AudioState
{
    /// <summary>
    /// The audio is finished, or hasn't been started.
    /// </summary>
    None,
    
    /// <summary>
    /// The audio has been stopped, and its position has been reset.
    /// </summary>
    Stopped,
    
    /// <summary>
    /// The audio has been paused, and can be resumed from its current position.
    /// </summary>
    Paused,
    
    /// <summary>
    /// The audio is currently playing.
    /// </summary>
    Playing
}