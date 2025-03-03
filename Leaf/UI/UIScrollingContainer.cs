using System.Numerics;
using System.Runtime.InteropServices;
using Cattail.UI.Events;
using Cattail.UI.Interfaces;
using Cattail.UI.Theming;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cattail.UI;

public class UIScrollingContainer : UIElement, IUIContainer
{
    public List<UIElement> Elements = [];
    
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
        ObjectID id = default,
        (string, Vector2) anchor = default,
        Vector2 origin = default
    ) : base(posScale, visible, container, new ObjectID(id.ID ?? "default", id.Class ?? "@scroll_container"), anchor, origin)
    {
        _allowVerticalScroll = verticalScroll;
        _allowHorizontalScroll = horizontalScroll;
    }

    public void AddElement(UIElement element)   
    {
        element.Container?.RemoveElement(element);
        Elements.Add(element);
        var tempAnchor = element.Anchor;
        element.SetAnchor("top-left", Vector2.Zero);
        element.SetAnchor(tempAnchor.AnchorPosition, tempAnchor.Offset);
        element.Container = this;
        Elements = Elements.OrderBy((x) => x.Layer).ToList();
        SetMaxScroll();
    }

    private void SetMaxScroll()
    {
        foreach (var element in Elements)
        {
            if (element.GetPosition().Y + element.RelativeRect.Height > _maxScroll.Y)
            {
                _maxScroll.Y = (element.GetPosition().Y + element.RelativeRect.Height - RelativeRect.Height) * -1.5f;
            }
            if (element.GetPosition().X + element.RelativeRect.Width > _maxScroll.X)
            {
                _maxScroll.X = element.GetPosition().X + element.RelativeRect.Width + RelativeRect.Width;
            }
        }
    }

    public void RemoveElement(UIElement element)
    {
        Elements.Remove(element);
    }

    public override void Update()
    {
        base.Update();
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
        //_scrollOffset = Vector2.Clamp(_scrollOffset, _maxScroll, Vector2.Zero);
        //Console.WriteLine(_scrollOffset);
    }

    public override void Kill()
    {
        foreach (UIElement element in Elements)
        {
            element.Kill();
        }
        base.Kill();
    }
}