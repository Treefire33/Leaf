using System.Numerics;
using Leaf.Events;
using Leaf.UI.Interfaces;

namespace Leaf.UI;

public class UIDropdown : UIElement
{
    public string SelectedOption { get; set; }
    private readonly UIButton? _selectedButton;
    private readonly UIScrollingContainer? _optionsContainer;

    /// <inheritdoc cref="UIElement"/>
    public UIDropdown(
        UIRect posScale,
        List<string> options,
        bool visible = true,
        IUIContainer? container = null,
        string id = "",
        string[]? classes = null,
        Vector2 anchor = default,
        Vector2 origin = default,
        string? tooltip = null
    ) : base(posScale, visible, container, id, classes, "dropdown", anchor, origin, tooltip)
    {
        _optionsContainer = new UIScrollingContainer(
            posScale with { Y = posScale.Height + posScale.Y, Height = posScale.Height * 2},
            visible: false
        );
        foreach (var option in options)
        {
            AddOption(option);
        }
        SelectedOption = options[0];
        _selectedButton = new UIButton(posScale, SelectedOption);
        _selectedButton.OnClick += i =>
        {
            _optionsContainer.Visible = !_optionsContainer.Visible;
        };
    }

    public void AddOption(string option)
    {
        var optionButton = new UIButton(
            posScale: new UIRect(0, 0, RelativeRect.Size),
            option
        );
        optionButton.OnClick += i =>
        {
            SelectedOption = option;
            _optionsContainer!.Visible = false;
        };
        _optionsContainer!.AddElement(optionButton);
        CalculateOptionPositions();
    }
    
    public void RemoveOption(string option)
    {
        var optionButton = _optionsContainer!.Elements.Find(element => ((UIButton)element).Text == option);
        if (optionButton != null)
            _optionsContainer!.RemoveElement(optionButton);
        CalculateOptionPositions();
    }

    private void CalculateOptionPositions()
    {
        float y = 0;
        foreach (UIElement option in _optionsContainer!.Elements)
        {
            option.RelativeRect = option.RelativeRect with
            {
                Y = y
            };
            y += option.RelativeRect.Height;
        }
        
        _optionsContainer.SetMaxScroll();
    }

    public override void Update()
    {
        _selectedButton!.SetText(SelectedOption);
        base.Update();
    }

    public override void Kill()
    {
        _optionsContainer?.Kill();
        base.Kill();
    }
}