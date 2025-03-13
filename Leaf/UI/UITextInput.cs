using System.Text.RegularExpressions;
using System.Numerics;
using System.Runtime.InteropServices;
using Cattail.UI.Events;
using Cattail.UI.Interfaces;
using Cattail.UI.Theming;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Cattail.UI;

public partial class UITextInput : UIElement
{
    private string _text;
    private Font _font;
    private int _fontSize;
    private readonly int _maxCharacters;
    private int _currentCharacters = 0;

    public bool Focused = false;

    private readonly Regex _inputRegex = DefaultInputRegex();

    public UITextInput(
        UIRect posScale, 
        string defaultText, 
        int maxCharacters,
        bool visible = true, 
        IUIContainer? container = null,
        string id = "",
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default
    ): base(posScale, visible, container, id, @class, "text_input", anchor, origin)
    {
        _text = defaultText;
        _maxCharacters = maxCharacters;
    }

    public override void ThemeElement()
    {
        /*_font = Theme.Font.Item1;
        _fontSize = Theme.Font.Item2;*/
    }

    public override void Update()
    {
        base.Update();
        
        HandleElementInteraction();
        
        DrawRectangleRec(RelativeRect.RelativeRect, Color.White);
        DrawTextPro(
            _font,
            _text,
            RelativeRect.Position,
            new Vector2(0),
            0,
            _fontSize,
            0,
            Color.Black
        );

        if (Hovered && IsMouseButtonPressed(MouseButton.Left))
        {
            Focused = true;
        }
        else if (IsMouseButtonPressed(MouseButton.Right))
        {
            Focused = false;
        }

        if (Focused)
        {
            SetMouseCursor(MouseCursor.IBeam);

            int key = GetCharPressed();

            while (key > 0)
            {
                if (key >= 32 && key <= 125 && _currentCharacters <= _maxCharacters)
                {
                    _text += (char)key;
                    _currentCharacters++;
                }

                key = GetCharPressed();
            }

            if (IsKeyDown(KeyboardKey.Backspace) && _currentCharacters >= 0)
            {
                _currentCharacters--;
                if (_currentCharacters < 0) { _currentCharacters = 0; }
                _text = _text.Remove(_currentCharacters);
            }
        }
        else
        {
            SetMouseCursor(MouseCursor.Default);
        }
    }

    public void SetText(string text)
    {
        _text = text;
        _currentCharacters = _text.Length;
    }

    public string GetText()
    {
        return _text;
    }

    public void ChangeTexture() { } //UITextInput shouldn't change texture.

    private int _framesCount = 0;
    public void HandleElementInteraction()
    {
        ChangeTexture();
        if (Focused)
        {
            _framesCount++;
            if (_currentCharacters < _maxCharacters)
            {
                if (_framesCount / 20 % 2 == 0)
                {
                    DrawTextPro(
                        _font,
                        "|",
                        new Vector2(RelativeRect.X + MeasureTextEx(_font, _text, _fontSize, 0).X, RelativeRect.Y),
                        new Vector2(0),
                        0,
                        25,
                        0,
                        Color.Black
                    );
                }
            }
        }
    }

    [GeneratedRegex(@"[^A-Za-z0-9 ]+", RegexOptions.Multiline)]
    private static partial Regex DefaultInputRegex();
}