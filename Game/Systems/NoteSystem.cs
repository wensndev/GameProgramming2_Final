using System.Collections.Generic;

namespace Game.Systems;

public class NoteSystem
{
    private readonly List<string> _collected = new();
    public bool IsShowing { get; private set; }
    public IReadOnlyList<string> Collected => _collected;

    public void Collect(string note)
    {
        if (!_collected.Contains(note))
            _collected.Add(note);
    }

    public void Toggle() => IsShowing = !IsShowing;
}
