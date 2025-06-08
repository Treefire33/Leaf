using System.Numerics;
using System.Text.RegularExpressions;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public partial class UITextInput : UIElement
{
    private string _text = null!;
    private readonly int _maxCharacters;

    public bool Focused;

    public string Text
    {
        get => _text;
        set
        {
            if (_text != value)
            {
                _text = value;
                OnTextChanged?.Invoke();
            }
        }
    }

    private readonly Regex _inputRegex = DefaultInputRegex();
    
    public Action? OnTextChanged { get; set; }

    public UITextInput(
        UIRect posScale, 
        string defaultText, 
        int maxCharacters,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ): base(posScale, visible, container, id, classes, "text-input", anchor, origin, tooltip)
    {
        Text = defaultText;
        _maxCharacters = maxCharacters;
    }

    public override void Update()
    {
        base.Update();

        if (IsMouseButtonPressed(MouseButton.Left))
        {
            Focused = Hovered;
        }

        if (Focused && IsKeyPressed(KeyboardKey.Enter))
        {
            Focused = false;
        }

        SetMouseCursor(Hovered ? MouseCursor.IBeam : MouseCursor.Default);

        HandleElementInteraction();
        
        Utility.DrawRectangle(
            new Rectangle(GetPosition(), RelativeRect.Size),
            _borderRadius,
            _borderThickness,
            _backgroundColour,
            _borderColour
        );
        Utility.DrawTextBoxed(
            _font,
            Text,
            new Rectangle(GetPosition(), RelativeRect.Size),
            _fontSize,
            _textSpacing,
            true,
            _textColour
        );

        if (Focused)
        {
            //Draw text cursor
            var textSize = MeasureTextEx(_font, Text, _fontSize, _textSpacing);
            var textPosition = new Vector2(
                GetPosition().X + (textSize.X % RelativeRect.Size.X), 
                GetPosition().Y + (textSize.Y * (int)(textSize.X / RelativeRect.Size.X)) * 1.5f
            );
            // remove characters until it fits in box.
            // max characters is ignored.
            if (textPosition.Y > GetPosition().Y + RelativeRect.Size.Y && Text.Length - 1 >= 0)
            {
                Text = Text.Remove(Text.Length - 1, 1);
            }
            Utility.DrawTextBoxed(
                _font,
                "|",
                new Rectangle(textPosition, RelativeRect.Size),
                _fontSize,
                _textSpacing,
                true,
                _textColour
            );
        }
    }
    
    public void HandleElementInteraction()
    {
        if (Focused)
        {
            int key = GetCharPressed();

            while (key > 0)
            {
                if (Text.Length <= _maxCharacters)
                {
                    Text += (char)key;
                }

                key = GetCharPressed();
            }
            
            if (IsKeyPressed(KeyboardKey.Backspace) && Text.Length - 1 >= 0)
            {
                Text = Text.Remove(Text.Length - 1, 1);
            }
            
            Text = _inputRegex.Replace(Text, string.Empty);
        }
    }

    [GeneratedRegex(@"[^A-Za-z0-9 ]+", RegexOptions.Multiline)]
    private static partial Regex DefaultInputRegex();
}