using System.Numerics;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UIImage : UIElement
{
    private Texture2D _image;
    private UIRect _imageRect;
    private bool _ninePatch;
    private NPatchInfo _patchInfo;

    public UIImage(
        UIRect posScale, 
        Texture2D image, 
        bool ninePatch = false, 
        NPatchInfo info = default,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, classes, "image", anchor, origin, tooltip)
    {
        _image = image;
        _ninePatch = ninePatch;
        _imageRect = new UIRect(0, 0, image.Width, image.Height);
        _patchInfo = info.Top == default(NPatchInfo).Top ? Resources.GenerateNPatchInfoFromButton(image) : info;
    }

    public void SetImage(Texture2D image, bool ninePatch = false, NPatchInfo info = default)
    {
        _image = image;
        _ninePatch = ninePatch;
        _imageRect = new UIRect(0, 0, image.Width, image.Height);
        _patchInfo = info.Top == default(NPatchInfo).Top ? Resources.GenerateNPatchInfoFromButton(image) : info;
    }

    public override void Update()
    {
        base.Update();

        if (_ninePatch)
        {
            DrawTextureNPatch(
                _image,
                _patchInfo,
                new Rectangle(GetPosition(), RelativeRect.Size),
                Vector2.Zero,
                0,
                Color.White
            );
        }
        else
        {
            DrawTexturePro(_image, _imageRect, new Rectangle(GetPosition(), RelativeRect.Size), Vector2.Zero, 0, Color.White);
        }
    }
}