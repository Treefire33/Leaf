using System.Numerics;
using Leaf.Events;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UIScrollingContainer : UIContainer
{
    private float _scrollSpeedX = 12.0f;
    private float _scrollSpeedY = 12.0f;
    private Vector2 _maxScroll = Vector2.Zero;
    private Vector2 _scrollOffset = Vector2.Zero;
    
    private bool _allowVerticalScroll;
    private bool _allowHorizontalScroll;

    private UISlider? _scrollBarX;
    private UISlider? _scrollBarY;
    
    public UIScrollingContainer(
        UIRect posScale,
        bool verticalScroll = true,
        bool horizontalScroll = false,
        bool enableScrollbars = false,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, classes, anchor, origin, tooltip)
    {
        _allowVerticalScroll = verticalScroll;
        if (verticalScroll && enableScrollbars)
        {
            _scrollBarY = new UISlider(
                new UIRect(posScale.X+posScale.Width, posScale.Y, 32, posScale.Height),
                scrollDirection: ScrollDirection.VerticalTop,
                origin: new Vector2(1, 0)
            );
        }
        _allowHorizontalScroll = horizontalScroll;
        if (horizontalScroll && enableScrollbars)
        {
            _scrollBarX = new UISlider(
                new UIRect(posScale.X, posScale.Y + posScale.Height, posScale.Width, 32),
                scrollDirection: ScrollDirection.HorizontalLeft,
                origin: new Vector2(0, 1)
            );
        }
        SetMaxScroll();
    }
    
    public void SetScrollSpeed(float scrollX = 12.0f, float scrollY = 12.0f)
    {
        _scrollSpeedX = scrollX;
        _scrollSpeedY = scrollY;
    }
    
    public void SetMaxScroll()
    {
        _maxScroll = Vector2.Zero;
        foreach (var element in Elements)
        {
            if (element.GetPosition().Y + element.RelativeRect.Height > _maxScroll.Y)
            {
                // Should this somehow not be negative, just set the max scroll to 0.
                _maxScroll.Y = MathF.Min(-element.RelativeRect.BottomLeft.Y + RelativeRect.Height, 0);
                if (_scrollBarY != null) _scrollBarY.MinValue = 0;
                if (_scrollBarY != null) _scrollBarY.MaxValue = -_maxScroll.Y;
            }
            if (element.GetPosition().X + element.RelativeRect.Width > _maxScroll.X)
            {
                _maxScroll.X = MathF.Max(element.RelativeRect.X - RelativeRect.Width, 0);
                if (_scrollBarX != null) _scrollBarX.MinValue = 0;
                if (_scrollBarX != null) _scrollBarX.MaxValue = _maxScroll.X;
            }
        }
    }

    public override void AddElement(UIElement element)
    {
        base.AddElement(element);
        SetMaxScroll();
    }

    public override void RemoveElement(UIElement element)
    {
        base.RemoveElement(element);
        SetMaxScroll();
    }

    public override void ClearElements()
    {
        base.ClearElements();
        SetMaxScroll();
    }

    public override void Update()
    {
        Hovered = CheckCollisionPointRec(Utility.GetVirtualMousePosition(), new Rectangle(GetPosition(), RelativeRect.Size));
        HandleScroll();
        if (Visible)
        {
            BeginScissorMode((int)GetPosition().X, (int)GetPosition().Y, (int)RelativeRect.Width, (int)RelativeRect.Height);
                foreach (UIElement element in Elements.Where(e => e.Visible))
                {
                    element.Anchor.Offset = _scrollOffset;
                    element.Update();
                }
            EndScissorMode();
        }
    }
    
    private void HandleScroll()
    {
        if ((_scrollBarX?.Focused ?? false) || (_scrollBarY?.Focused ?? false))
        {
            if (_scrollBarX != null)
            {
                _scrollOffset.X = _scrollBarX.Value;
            }
            
            if (_scrollBarY != null)
            {
                _scrollOffset.Y = -_scrollBarY.Value;
            }
        }
        else if (Hovered)
        {
            if ((IsKeyDown(KeyboardKey.LeftShift) || IsKeyDown(KeyboardKey.RightShift)) && _allowHorizontalScroll)
            {
                if (_allowHorizontalScroll)
                {
                    _scrollOffset.X -= (int)GetMouseWheelMove() * _scrollSpeedX;
                    if (_scrollBarX != null) _scrollBarX.Value = _scrollOffset.X;
                }
            }
            else if (_allowVerticalScroll)
            {
                _scrollOffset.Y += (int)GetMouseWheelMove() * _scrollSpeedY;
                if (_scrollBarY != null) _scrollBarY.Value = -_scrollOffset.Y;
            }
        }
        _scrollOffset.X = Math.Clamp(_scrollOffset.X, 0, _maxScroll.X);
        _scrollOffset.Y = Math.Clamp(_scrollOffset.Y, _maxScroll.Y, 0);
        //_scrollOffset = Vector2.Clamp(_scrollOffset, _maxScroll, Vector2.Zero);
        //Console.WriteLine(_scrollOffset);
    }
}