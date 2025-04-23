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
    
    public UIScrollingContainer(
        UIRect posScale,
        bool verticalScroll = true,
        bool horizontalScroll = false,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, @class, anchor, origin, tooltip)
    {
        _allowVerticalScroll = verticalScroll;
        _allowHorizontalScroll = horizontalScroll;
    }
    
    public void SetScrollSpeed(float scrollX = 12.0f, float scrollY = 12.0f)
    {
        _scrollSpeedX = scrollX;
        _scrollSpeedY = scrollY;
    }
    
    private void SetMaxScroll()
    {
        foreach (var element in Elements)
        {
            if (element.GetPosition().Y + element.RelativeRect.Height > _maxScroll.Y)
            {
                // Should this somehow not be negative, just set the max scroll to 0.
                _maxScroll.Y = MathF.Min((element.GetPosition().Y + element.RelativeRect.Height - RelativeRect.Height) * -1.5f, 0);
            }
            if (element.GetPosition().X + element.RelativeRect.Width > _maxScroll.X)
            {
                _maxScroll.X = element.GetPosition().X + element.RelativeRect.Width + RelativeRect.Width;
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

    public override void Update()
    {
        Hovered = CheckCollisionPointRec(Utility.GetVirtualMousePosition(), new Rectangle(GetPosition(), RelativeRect.Size));
        HandleScroll();
        if (Visible)
        {
            if (UIManager.DebugMode)
                DrawRectangleRec(
                    new UIRect(GetPosition(), RelativeRect.Size),
                    Color.SkyBlue
                );
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
        if (Hovered)
        {
            if ((IsKeyDown(KeyboardKey.LeftShift) || IsKeyDown(KeyboardKey.RightShift)) && _allowHorizontalScroll)
            {
                if (_allowHorizontalScroll)
                {
                    _scrollOffset.X -= (int)GetMouseWheelMove() * _scrollSpeedX;
                    _scrollOffset.X = Math.Clamp(_scrollOffset.X, 0, _maxScroll.X);
                }
            }
            else if (_allowVerticalScroll)
            {
                _scrollOffset.Y += (int)GetMouseWheelMove() * _scrollSpeedY;
                _scrollOffset.Y = Math.Clamp(_scrollOffset.Y, _maxScroll.Y, 0);
            }
        }
        //_scrollOffset = Vector2.Clamp(_scrollOffset, _maxScroll, Vector2.Zero);
        //Console.WriteLine(_scrollOffset);
    }
}