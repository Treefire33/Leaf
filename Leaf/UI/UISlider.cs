using System.Numerics;
using System.Text.RegularExpressions;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UISlider : UIElement
{
    public float MinValue { get; set; }
    public float MaxValue { get; set; }
    
    private float _value;
    public float Value
    {
        get => _value;
        set
        {
            OnValueChanged?.Invoke(value - _value);
            _value = Math.Clamp(value, MinValue, MaxValue);
        }
    }

    private float _step;
    
    public bool Focused;
    private bool _scrollDirection;

    private UIPanel? _handle;
    
    private float _outlineThickness = 1f;
    private Color _outlineColour = Color.Black;
    private Color _fillColour = Color.White;
    private Color _backgroundColour = Color.Gray;
    
    public Action<float>? OnValueChanged;
 
    public UISlider(
        UIRect posScale, 
        float minValue = 0, 
        float maxValue = 1, 
        float value = 0, 
        float valueStep = 0.0001f,
        string scrollDirection = "horizontal",
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
        _step = valueStep;
        _scrollDirection = scrollDirection.ToLower() switch
        {
            "horizontal" => false,
            "vertical" => true,
        };
        var handleRect = new UIRect(0, 0, 32, RelativeRect.Size.Y);
        if (_scrollDirection)
        {
            handleRect = new UIRect(0, 0, RelativeRect.Size.X, 32);
        }
        _handle = new UIPanel(
            handleRect,
            origin: _scrollDirection ? new Vector2(0, 0.5f) : new Vector2(0.5f, 0)
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

    private void UpdateHandle()
    {
        
    }

    public override void Update()
    {
        base.Update();

        if (IsMouseButtonPressed(MouseButton.Left))
        {
            Focused = Hovered || _handle!.Hovered;
        }

        if (IsMouseButtonReleased(MouseButton.Left))
        {
            Focused = false;
        }

        HandleElementInteraction();
        
        UpdateHandle();
        
        Vector2 fillSize;
        if (!_scrollDirection)
        {
            _handle!.RelativeRect = _handle!.RelativeRect with
            {
                X = MathF.Abs(Value / MaxValue) * (RelativeRect.Size.X),
            };
            fillSize = RelativeRect.Size with { X = RelativeRect.Size.X * (Value / MaxValue) };
        }
        else
        {
            _handle!.RelativeRect = _handle!.RelativeRect with
            {
                Y = MathF.Abs(Value / MaxValue) * (RelativeRect.Size.Y),
            };
            fillSize = RelativeRect.Size with { Y = RelativeRect.Size.Y * (Value / MaxValue) };
        }

        DrawRectangleRec(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _backgroundColour
        );
        
        DrawRectangleRec(
            new Rectangle(GetPosition(), fillSize),
            _fillColour
        );
        DrawRectangleLinesEx(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _outlineThickness,
            _outlineColour
        );
    }

    public void HandleElementInteraction()
    {
        if (Focused && IsMouseButtonDown(MouseButton.Left))
        {
            float scaledDist;
            if (!_scrollDirection)
            {
                scaledDist = (Utility.GetVirtualMousePosition().X - GetPosition().X) / RelativeRect.Size.X;
            }
            else
            {
                scaledDist = (Utility.GetVirtualMousePosition().Y - GetPosition().Y) / RelativeRect.Size.Y;
            }

            var value = scaledDist * MaxValue + MinValue;
            Value = MathF.Floor(value/_step) * _step;
        }
    }
}