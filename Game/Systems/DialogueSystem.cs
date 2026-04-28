using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Game.Core;

namespace Game.Systems;

public class DialogueSystem
{
    public bool IsActive { get; private set; }
    private string[] _lines = Array.Empty<string>();
    private int _lineIndex;

    public string CurrentLine => _lineIndex < _lines.Length ? _lines[_lineIndex] : string.Empty;

    public event Action OnComplete;

    public void StartDialogue(string[] lines)
    {
        _lines = lines;
        _lineIndex = 0;
        IsActive = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;
        if (Game1.Input.IsPressed(Keys.Enter) || Game1.Input.IsPressed(Keys.Space))
        {
            _lineIndex++;
            if (_lineIndex >= _lines.Length)
            {
                IsActive = false;
                OnComplete?.Invoke();
            }
        }
    }
}
