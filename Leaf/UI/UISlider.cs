using System.Numerics;
using System.Text.RegularExpressions;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UISlider : UIElement
{
    public float MinValue { get; private set; } = 0;
    public float MaxValue { get; private set; } = 1;
    
    private float _value = 0;
    public float Value
    {
        get => _value;
        set
        {
            OnValueChanged?.Invoke(value - _value);
            _value = Math.Clamp(value, MinValue, MaxValue);
        }
    }
    
    public bool Focused;

    private UIPanel? _handle;
    
    private float _outlineThickness = 1f;
    private Color _outlineColour = Color.Black;
    private Color _fillColour = Color.White;
    private Color _backgroundColour = Color.Gray;
    
    public Action<float>? OnValueChanged;
 
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
        _handle = new UIPanel(
            new UIRect(0, 0, 32, RelativeRect.Size.Y),
            origin: new Vector2(0.5f, 0)
        );
        _handle.SetAnchor("top-left", this);
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
        
        if (IsMouseButtonPressed(MouseButton.Left))
        {
            Focused = Hovered || _handle!.Hovered;
        }
        
        HandleElementInteraction();

        _handle!.RelativeRect = _handle!.RelativeRect with
        {
            X = (Value / MaxValue) * (GetPosition().X + RelativeRect.Size.X),
        };
        
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
        if (Focused && Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            var scaledDist = (Utility.GetVirtualMousePosition().X - GetPosition().X) / RelativeRect.Size.X;
            var value = scaledDist * MaxValue + MinValue;
            Value = value;
        }
    }
}