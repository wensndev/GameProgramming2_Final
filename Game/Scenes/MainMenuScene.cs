using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;
using Game.UI;

namespace Game.Scenes;

public class MainMenuScene : IScene
{
    private readonly SceneManager _sceneManager;
    private readonly Button[] _buttons;
    private int _selectedIndex;

    public MainMenuScene(SceneManager sceneManager)
    {
        _sceneManager = sceneManager;
        int lx = 160;
        int rx = GameConstants.WindowWidth - 400;
        _buttons = new[]
        {
            new Button("Start",   lx, 320),
            new Button("Quit",    lx, 390),
            new Button("Options", rx, 320),
            new Button("Credits", rx, 390),
        };
        _buttons[0].IsSelected = true;
    }

    public void Load() { }
    public void Unload() { }

    public void Update(GameTime gameTime)
    {
        if (Game1.Input.IsPressed(Keys.Up) || Game1.Input.IsPressed(Keys.W))
            MoveSelection(-1);
        if (Game1.Input.IsPressed(Keys.Down) || Game1.Input.IsPressed(Keys.S))
            MoveSelection(1);

        if (Game1.Input.IsPressed(Keys.Enter) || Game1.Input.IsPressed(Keys.Space))
        {
            switch (_selectedIndex)
            {
                case 0: _sceneManager.ChangeScene(new GameplayScene(_sceneManager)); break;
                case 1: System.Environment.Exit(0); break;
                case 2: _sceneManager.ChangeScene(new OptionsScene(_sceneManager)); break;
                case 3: _sceneManager.ChangeScene(new CreditsScene(_sceneManager)); break;
            }
        }
    }

    private void MoveSelection(int delta)
    {
        _buttons[_selectedIndex].IsSelected = false;
        _selectedIndex = (_selectedIndex + delta + _buttons.Length) % _buttons.Length;
        _buttons[_selectedIndex].IsSelected = true;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        var title = "WHY AM I ALIVE?";
        var titleSize = AssetManager.Font.MeasureString(title);
        spriteBatch.DrawString(AssetManager.Font, title,
            new Vector2((GameConstants.WindowWidth - titleSize.X) / 2f, 180), Color.White);

        var sub = "Up/Down: Select   Enter: Confirm";
        var subSize = AssetManager.Font.MeasureString(sub);
        spriteBatch.DrawString(AssetManager.Font, sub,
            new Vector2((GameConstants.WindowWidth - subSize.X) / 2f, 255), Color.Gray);

        foreach (var b in _buttons) b.Draw(spriteBatch);

        spriteBatch.End();
    }
}
