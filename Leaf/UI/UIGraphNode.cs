using System.Numerics;
using Leaf.UI.Interfaces;

namespace Leaf.UI;

/// <summary>
/// Unfinished implementation of a UIElement that displays
/// graphs through a custom defined class that implements IGraphData.
/// </summary>
public class UIGraphNode : UIElement
{
    private IGraphData _graphData;

    public UIGraphNode(
        UIRect posScale,
        IGraphData graphData,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, classes, "graph-node", anchor, origin, tooltip)
    {
        _graphData = graphData;
    }

    public override void Update()
    {
        base.Update();
        _graphData.Display(new UIRect(GetPosition(), RelativeRect.Size));
    }
}