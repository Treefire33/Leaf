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
            OnValueChanged?.Invoke();
            _value = Math.Clamp(value, MinValue, MaxValue);
        }
    }
    
    private float _step;

    public float Step
    {
        get => _step;
        set => _step = Math.Clamp(value, MinValue, MaxValue);
    }
    
    public bool Focused;
    private ScrollDirection _scrollDirection;

    private readonly UIPanel? _handle;
    
    private Color _fillColour = Color.White;
    
    public Action? OnValueChanged;
    public Action<int>? OnMouseDown;
    public Action<int>? OnMouseUp;
 
    public UISlider(
        UIRect posScale, 
        float minValue = 0, 
        float maxValue = 1, 
        float value = 0, 
        float valueStep = 0.0001f,
        ScrollDirection scrollDirection = ScrollDirection.HorizontalLeft,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, @class, "slider", anchor, origin, tooltip)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        Value = value;
        _step = valueStep;
        _scrollDirection = scrollDirection;
        var handleRect = new UIRect(0, 0, 32, RelativeRect.Size.Y);
        if (_scrollDirection is ScrollDirection.VerticalTop or ScrollDirection.VerticalBottom)
        {
            handleRect = new UIRect(0, 0, RelativeRect.Size.X, 32);
        }
        _handle = new UIPanel(
            handleRect,
            origin: new Vector2(0.5f)
        );
        _handle.SetAnchor(
            _scrollDirection is ScrollDirection.VerticalTop or ScrollDirection.VerticalBottom
                ? AnchorPosition.TopCenter
                : AnchorPosition.Left, this);
        ThemeElement();
    }

    public override void ThemeElement()
    {
        base.ThemeElement();
        _fillColour = Theme.GetProperty("fill").AsColor();
    }
    
    public override void Update()
    {
        base.Update();

        if (IsMouseButtonPressed(MouseButton.Left))
        {
            Focused = Hovered || _handle!.Hovered;
            if (Hovered) OnMouseDown?.Invoke(0);
        }

        if (IsMouseButtonReleased(MouseButton.Left))
        {
            if (Focused) OnMouseUp?.Invoke(0);
            Focused = false;
        }

        HandleElementInteraction();
        
        Rectangle fillRect = new(GetPosition(), RelativeRect.Size);
        switch (_scrollDirection)
        {
            case ScrollDirection.HorizontalLeft:
                _handle!.RelativeRect = _handle!.RelativeRect with
                {
                    X = MathF.Abs(Value / MaxValue) * (RelativeRect.Size.X)
                };
                fillRect.Width = RelativeRect.Size.X * (Value / MaxValue);
                break;
            case ScrollDirection.HorizontalRight:
                _handle!.RelativeRect = _handle!.RelativeRect with
                {
                    X = (RelativeRect.Size.X) - MathF.Abs((Value * RelativeRect.Size.X) / MaxValue)
                };
                fillRect = fillRect with
                { 
                    X = fillRect.X + fillRect.Width - RelativeRect.Size.X * (Value / MaxValue),
                    Width = RelativeRect.Size.X * (Value / MaxValue)
                };
                break;
            case ScrollDirection.VerticalTop:
                _handle!.RelativeRect = _handle!.RelativeRect with
                {
                    Y = MathF.Abs(Value / MaxValue) * (RelativeRect.Size.Y)
                };
                fillRect.Height = RelativeRect.Size.Y * (Value / MaxValue);
                break;
            case ScrollDirection.VerticalBottom:
                _handle!.RelativeRect = _handle!.RelativeRect with
                {
                    Y = (RelativeRect.Size.Y) - MathF.Abs((Value * RelativeRect.Size.Y) / MaxValue)
                };
                fillRect = fillRect with
                { 
                    Y = fillRect.Y + fillRect.Height - RelativeRect.Size.Y * (Value / MaxValue),
                    Height = RelativeRect.Size.Y * (Value / MaxValue)
                };
                break;
        }

        Utility.DrawRectangle(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _borderRadius,
            _borderThickness,
            _backgroundColour,
            _borderColour
        );

        DrawRectangleRounded(fillRect, _borderRadius, 0, _fillColour);
    }

    public void HandleElementInteraction()
    {
        if (Focused && IsMouseButtonDown(MouseButton.Left))
        {
            float scaledDist;
            bool direction = _scrollDirection is ScrollDirection.VerticalBottom or ScrollDirection.HorizontalRight;
            if (_scrollDirection is ScrollDirection.HorizontalLeft or ScrollDirection.HorizontalRight)
            {
                scaledDist = (Utility.GetVirtualMousePosition().X - GetPosition().X) / RelativeRect.Size.X;
            }
            else
            {
                scaledDist = (Utility.GetVirtualMousePosition().Y - GetPosition().Y) / RelativeRect.Size.Y;
            }

            scaledDist = Math.Clamp(scaledDist, 0, 1);

            float value;
            
            if (!direction)
            {
                value = scaledDist * MaxValue + MinValue;
            }
            else
            {
                value = MaxValue - scaledDist * MaxValue;
            }

            Value = MathF.Floor(value / _step) * _step;
        }
    }

    public override void SetAnchor(AnchorPosition anchorPos, UIElement? target = null)
    {
        base.SetAnchor(anchorPos, target);
        _handle!.SetAnchor(
            _scrollDirection is ScrollDirection.VerticalTop or ScrollDirection.VerticalBottom
                ? AnchorPosition.TopCenter
                : AnchorPosition.Left, this);
    }
}

public enum ScrollDirection
{
    HorizontalLeft,
    HorizontalRight,
    VerticalTop,
    VerticalBottom
}