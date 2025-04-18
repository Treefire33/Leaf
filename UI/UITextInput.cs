using System.Numerics;
using System.Text.RegularExpressions;
using Leaf.UI.Interfaces;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Leaf.UI;

public partial class UITextInput : UIElement
{
    private string _text;
    private Font _font;
    private int _fontSize;
    private Color _textColour;
    private readonly int _maxCharacters;
    private int _currentCharacters = 0;

    public bool Focused;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            _currentCharacters = _text.Length;
            OnTextChanged?.Invoke();
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
        string @class = "",
        (string, Vector2) anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ): base(posScale, visible, container, id, @class, "text-input", anchor, origin, tooltip)
    {
        _text = defaultText;
        _maxCharacters = maxCharacters;
        ThemeElement();
    }

    public override void ThemeElement()
    {
        _font = Theme.GetProperty("font-family").AsFont();
        _fontSize = Theme.GetProperty("font-size").AsInt();
        _textColour = Theme.GetProperty("color").AsColor();
    }

    public override void Update()
    {
        base.Update();
        
        HandleElementInteraction();
        
        DrawRectangleRec(new Rectangle(GetPosition(), RelativeRect.Size), Color.White);
        DrawTextPro(
            _font,
            _text,
            GetPosition(),
            new Vector2(0),
            0,
            _fontSize,
            0,
            _textColour
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

    private int _framesCount = 0;
    public void HandleElementInteraction()
    {
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