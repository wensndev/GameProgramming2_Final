using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;

namespace Game.Scenes;

public class GameOverScene : IScene
{
    private readonly SceneManager _sceneManager;

    public GameOverScene(SceneManager sceneManager) => _sceneManager = sceneManager;

    public void Load() { }
    public void Unload() { }

    public void Update(GameTime gameTime)
    {
        if (Game1.Input.IsPressed(Keys.Enter) || Game1.Input.IsPressed(Keys.Space))
            _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
        if (Game1.Input.IsPressed(Keys.Escape))
            System.Environment.Exit(0);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        AssetManager.DrawRect(spriteBatch, 0, 0, GameConstants.WindowWidth, GameConstants.WindowHeight, new Color(60, 0, 0, 255));
        DrawCentered(spriteBatch, "GAME OVER", 260, Color.Red);
        DrawCentered(spriteBatch, "Enter: Main Menu   ESC: Quit", 340, Color.LightGray);
        spriteBatch.End();
    }

    private void DrawCentered(SpriteBatch sb, string text, int y, Color color)
    {
        var size = AssetManager.Font.MeasureString(text);
        sb.DrawString(AssetManager.Font, text,
            new Vector2((GameConstants.WindowWidth - size.X) / 2f, y), color);
    }
}
