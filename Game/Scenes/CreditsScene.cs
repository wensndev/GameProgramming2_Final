using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;

namespace Game.Scenes;

public class CreditsScene : IScene
{
    private readonly SceneManager _sceneManager;

    public CreditsScene(SceneManager sceneManager) => _sceneManager = sceneManager;

    public void Load() { }
    public void Unload() { }

    public void Update(GameTime gameTime)
    {
        if (Game1.Input.IsPressed(Keys.Escape) || Game1.Input.IsPressed(Keys.Enter))
            _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        AssetManager.DrawRect(spriteBatch, 0, 0, GameConstants.WindowWidth, GameConstants.WindowHeight, Color.Black);
        DrawCentered(spriteBatch, "CREDITS", 160, Color.White);
        DrawCentered(spriteBatch, "Why Am I Alive", 240, Color.LightGray);
        DrawCentered(spriteBatch, "A MonoGame Project", 280, Color.Gray);
        DrawCentered(spriteBatch, "ESC / Enter: Back", 480, Color.DarkGray);
        spriteBatch.End();
    }

    private void DrawCentered(SpriteBatch sb, string text, int y, Color color)
    {
        var size = AssetManager.Font.MeasureString(text);
        sb.DrawString(AssetManager.Font, text,
            new Vector2((GameConstants.WindowWidth - size.X) / 2f, y), color);
    }
}
