namespace Leaf.Animation;

public static class AnimationManager
{
    public static List<Animation> Animations = [];
    
    public static void Update()
    {
        foreach (Animation animation in Animations)
        {
            animation.Update();
        }
    }
}