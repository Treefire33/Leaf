using System.Numerics;
using Leaf.Animation;
using Leaf.Audio;
using Leaf.UI;
using Leaf.UI.GraphData;
using Leaf.Utilities;
using Raylib_cs;
using static Raylib_cs.Raylib;
using BlendMode = Leaf.Utilities.BlendMode;

namespace Leaf.Test;

public class AnimationTest : ILeafTest
{
    public void Test(ref UIManager manager)
    {
        var animation = new Animation.Animation(30);
        for (int i = 0; i < 30 * 20; i += 30)
        {
            int frame = i;
            animation.AddKeyframe(i, new Keyframe()
            {
                Played = delegate
                {
                    Console.WriteLine($"{frame/30}s");
                }
            });
        }
        while (!WindowShouldClose())
        {
            BeginDrawing();
                ClearBackground(Color.Black);
                AnimationManager.Update();
            EndDrawing();
        }
    }
}