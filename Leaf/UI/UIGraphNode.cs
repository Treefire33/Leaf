using System.Numerics;
using Leaf.UI.Interfaces;

namespace Leaf.UI;

public class UIGraphNode : UIElement
{
    private IGraphData _graphData;

    public UIGraphNode(
        UIRect posScale,
        IGraphData graphData,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, @class, "graph_node", anchor, origin, tooltip)
    {
        _graphData = graphData;
    }

    public override void Update()
    {
        base.Update();
        _graphData.Display(new UIRect(GetPosition(), RelativeRect.Size));
    }
}