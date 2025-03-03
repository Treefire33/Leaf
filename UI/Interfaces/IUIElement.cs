using System.Numerics;

namespace Cattail.UI.Interfaces;

public interface IUIElement
{
    UIRect RelativeRect { get; set; }
    
    Vector2 GetPosition();
    void Update();
}
    
