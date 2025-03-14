using System.Numerics;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public class UIImage : UIElement
{
    public Texture2D Image;
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
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default
    ) : base(posScale, visible, container, id, @class, "image", anchor, origin)
    {
        Image = image;
        _ninePatch = ninePatch;
        _imageRect = new(0, 0, new Vector2(image.Width, image.Height));
        _patchInfo = info.Top == default(NPatchInfo).Top ? Resources.GenerateNPatchInfoFromButton(image) : info;
    }

    public override void Update()
    {
        base.Update();

        if (_ninePatch)
        {
            DrawTextureNPatch(
                Image,
                _patchInfo,
                new Rectangle(GetPosition(), RelativeRect.Size),
                Vector2.Zero,
                0,
                Color.White
            );
        }
        else
        {
            DrawTexturePro(Image, _imageRect, new Rectangle(GetPosition(), RelativeRect.Size), Vector2.Zero, 0, Color.White);
        }
    }
}