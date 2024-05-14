﻿using UnityEngine;
using UnityEngine.UI;

namespace Blasphemous.Framework.Menus.Options;

/// <summary>
/// An option composed of text
/// </summary>
public class TextOption : MonoBehaviour
{
    private Image _underline;
    private Text _text;

    private bool _numeric;
    private bool _allowZero;
    private int _maxLength;

    private string _currentValue;
    private bool _selected;

    /// <summary>
    /// The current string value of the option
    /// </summary>
    public string CurrentValue
    {
        get => _currentValue.Length > 0 ? _currentValue : string.Empty;
        set
        {
            _currentValue = value;
            UpdateStatus();
        }
    }

    /// <summary>
    /// The current int value of the option
    /// </summary>
    public int CurrentNumericValue => int.TryParse(CurrentValue, out int value) ? value : 0;

    /// <summary>
    /// Updates the selected status
    /// </summary>
    public void SetSelected(bool selected)
    {
        _selected = selected;
        UpdateStatus();
    }

    private void Update()
    {
        if (!_selected)
            return;

        foreach (char c in Input.inputString)
        {
            ProcessCharacter(c);
        }
    }

    /// <summary>
    /// Initializes the text option
    /// </summary>
    public void Initialize(Image underline, Text text, bool numeric, bool allowZero, int maxLength)
    {
        _underline = underline;
        _text = text;

        _numeric = numeric;
        _allowZero = allowZero;
        _maxLength = maxLength;

        _currentValue = string.Empty;
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        _underline.sprite = _selected
            ? Main.MenuFramework.IconLoader.TextOn
            : Main.MenuFramework.IconLoader.TextOff;
        _text.text = _currentValue.Length > 0 ? _currentValue : "---";
    }

    private void ProcessCharacter(char c)
    {
        if (c == '\r' || c == '\n')
            return;

        if (c == '\b')
        {
            HandleBackspace();
            return;
        }

        if (_currentValue.Length >= _maxLength)
            return;

        if (char.IsWhiteSpace(c))
        {
            HandleWhitespace(c);
        }
        else if (!char.IsNumber(c))
        {
            HandleNonNumeric(c);
        }
        else if (c == '0')
        {
            HandleZero();
        }
        else
        {
            HandleNumber(c);
        }
    }

    void HandleBackspace()
    {
        if (_currentValue.Length == 0) // Skip if value is empty
            return;

        CurrentValue = _currentValue.Substring(0, _currentValue.Length - 1);
        //Main.Randomizer.AudioHandler.PlayEffectUI(UISFX.ChangeSelection);
    }

    void HandleWhitespace(char c)
    {
        if (_currentValue.Length == 0 || _numeric) // Skip if value is empty or only numbers allowed
            return;

        CurrentValue += c;
        //Main.Randomizer.AudioHandler.PlayEffectUI(UISFX.ChangeSelection);
    }

    void HandleNonNumeric(char c)
    {
        if (_numeric) // Skip if only numbers allowed
            return;

        CurrentValue += c;
        //Main.Randomizer.AudioHandler.PlayEffectUI(UISFX.ChangeSelection);
    }

    void HandleZero()
    {
        if (!_allowZero && _currentValue.Length == 0) // Skip if value is empty and cant start with zero
            return;

        CurrentValue += '0';
        //Main.Randomizer.AudioHandler.PlayEffectUI(UISFX.ChangeSelection);
    }

    void HandleNumber(char c)
    {
        CurrentValue += c;
        //Main.Randomizer.AudioHandler.PlayEffectUI(UISFX.ChangeSelection);
    }
}
