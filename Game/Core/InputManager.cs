using Microsoft.Xna.Framework.Input;

namespace Game.Core;

public class InputManager
{
    private KeyboardState _current;
    private KeyboardState _previous;

    public void Update()
    {
        _previous = _current;
        _current = Keyboard.GetState();
    }

    public bool IsDown(Keys key) => _current.IsKeyDown(key);
    public bool IsPressed(Keys key) => _current.IsKeyDown(key) && !_previous.IsKeyDown(key);
    public bool IsReleased(Keys key) => !_current.IsKeyDown(key) && _previous.IsKeyDown(key);
}
