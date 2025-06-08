using System.Numerics;
using Leaf.Audio;
using Leaf.UI;
using Leaf.UI.GraphData;
using Leaf.Utilities;
using Raylib_cs;
using static Raylib_cs.Raylib;
using BlendMode = Leaf.Utilities.BlendMode;

namespace Leaf.Test;

class Program
{
    private static int _testType = 3;
    static void Main(string[] args)
    {
        SetTraceLogLevel(TraceLogLevel.Error);
        InitWindow(800, 800, "Leaf UI Testing");
        
        SetTargetFPS(60);
        
        Resources.SetResourcesPath(ResourcesPath.Root, ".\\Resources\\");
        Resources.SetResourcesPath(ResourcesPath.Images, "Sprites\\");
        Resources.InitResources();
        var manager = new UIManager(themes: "style.css");

        var uiTest = new UIElementTest();
        var audioTest = new AudioTest();
        var imageTest = new ImageTest();
        var fontTest = new FontTest();
        
        // Tests for UIElements
        switch (_testType)
        {
            case 0:
                uiTest.Test(ref manager);
                break;
            case 1:
                audioTest.Test(ref manager);
                break;
            case 2:
                imageTest.Test(ref manager);
                break;
            case 3:
                fontTest.Test(ref manager);
                break;
        }
    }
}