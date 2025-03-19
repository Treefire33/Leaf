using System.Numerics;
using Leaf.UI.Interfaces;
using Raylib_cs;

namespace Leaf.UI;

public class UISlider : UIElement, IUIClickable
{
    public float MinValue { get; private set; } = 0;
    public float MaxValue { get; private set; } = 1;
    public float Value { get; set; } = 0;
    
    //private UIButton _handle;
    
    private float _outlineThickness = 1f;
    private Color _outlineColour = Color.Black;
    private Color _fillColour = Color.White;
    private Color _backgroundColour = Color.Gray;

    public UISlider(
        UIRect posScale, 
        float minValue, 
        float maxValue, 
        float value = 0, 
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, @class, "slider", anchor, origin, tooltip)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        Value = value;
        /*_handle = new UIButton(
            new UIRect(0, 0, 32, 32),
            "default",
            "",
            origin: new Vector2(0.5f, 0.5f)
        );
        _handle.SetAnchor("left", this);*/
        ThemeElement();
    }

    public override void ThemeElement()
    {
        base.ThemeElement();
        _fillColour = Theme.GetProperty("fill").AsColor();
        _backgroundColour = Theme.GetProperty("background-color").AsColor();
        _outlineColour = Theme.GetProperty("border-top-color").AsColor();
        _outlineThickness = Theme.GetProperty("border-top-width").AsFloat();
    }

    public override void Update()
    {
        base.Update();
        HandleElementInteraction();
        
        Raylib.DrawRectangleRec(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _backgroundColour
        );
        Raylib.DrawRectangleRec(
            new Rectangle(GetPosition(), RelativeRect.Size with { X = RelativeRect.Size.X * (Value / MaxValue) }),
            _fillColour
        );
        Raylib.DrawRectangleLinesEx(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _outlineThickness,
            _outlineColour
        );
    }

    public void HandleElementInteraction()
    {
        
    }
}