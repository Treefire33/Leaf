using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.Animation;

public class Animation
{
    public float FrameRate { get; }
    private int _frameCount;
    private float _cumulativeFrameTime;
    
    public Dictionary<int, List<Keyframe>> Keyframes = [];

    public Animation(int frameRate)
    {
        FrameRate = 1f/frameRate;
        AnimationManager.Animations.Add(this);
    }

    public void AddKeyframe(int frame, Keyframe keyframe)
    {
        if (!Keyframes.TryGetValue(frame, out List<Keyframe>? value))
        {
            value = [];
            Keyframes[frame] = value;
        }

        value.Add(keyframe);
    }

    public void Update()
    {
        _cumulativeFrameTime += GetFrameTime();
        if (_cumulativeFrameTime >= FrameRate)
        {
            _frameCount++;
            _cumulativeFrameTime = 0;
            if (!Keyframes.ContainsKey(_frameCount)) { return; }
            foreach (var keyframe in Keyframes[_frameCount])
            {
                keyframe.Played.Invoke();
            }
        }
    }
}