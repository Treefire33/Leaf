﻿using System.Numerics;

namespace Leaf.UI.Interfaces;

public interface IUIElement
{
    UIRect RelativeRect { get; set; }
    public Action? OnHover { get; set; }
    
    Vector2 GetPosition();
    void Update();
}
    
