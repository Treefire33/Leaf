using System.Numerics;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI.GraphData;

public class ScatterPlot(string xLabel, string yLabel, params Vector2[] entries) : IGraphData
{
    public string XLabel = xLabel;
    private Vector2 _xLabelSize = MeasureTextEx(GetFontDefault(), xLabel, 18f, 1f);
    public string YLabel = yLabel;
    private Vector2 _yLabelSize = MeasureTextEx(GetFontDefault(), yLabel, 18f, 1f);
    public List<Vector2> Entries = entries.ToList();

    public void Display(UIRect bounds)
    {
        DrawLineEx(bounds.Position, bounds.BottomLeft, 2f, Color.Black);
        DrawTextPro(GetFontDefault(), YLabel, 
            bounds.BottomLeft - _yLabelSize with { X = _yLabelSize.X / 2, Y = 0 }, 
            Vector2.Zero,
            -90,
            18f,
            1f,
            Color.Black
        );
        DrawLineEx(bounds.BottomLeft, bounds.BottomRight, 2f, Color.Black);
        DrawTextPro(GetFontDefault(), XLabel, 
            bounds.BottomLeft, 
            Vector2.Zero,
            0,
            18f,
            1f,
            Color.Black
        );
        foreach (var entry in Entries)
        {
            var point = new Vector2(
                entry.X + bounds.X,
                bounds.BottomRight.Y - entry.Y
            );
            DrawCircleV(point, 2f, Color.Black);
        }
    }
}