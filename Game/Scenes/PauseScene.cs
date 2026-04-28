using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;

namespace Game.Scenes;

public class PauseScene : IScene
{
    private readonly SceneManager _sceneManager;

    public PauseScene(SceneManager sceneManager) => _sceneManager = sceneManager;

    public void Load() { }
    public void Unload() { }

    public void Update(GameTime gameTime)
    {
        if (Game1.Input.IsPressed(Keys.Escape) || Game1.Input.IsPressed(Keys.P))
            _sceneManager.PopOverlay();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        AssetManager.DrawRect(spriteBatch, 0, 0, GameConstants.WindowWidth, GameConstants.WindowHeight, new Color(0, 0, 0, 150));
        DrawCentered(spriteBatch, "PAUSED", GameConstants.WindowHeight / 2 - 30, Color.White);
        DrawCentered(spriteBatch, "Press ESC or P to resume", GameConstants.WindowHeight / 2 + 10, Color.LightGray);
        spriteBatch.End();
    }

    private void DrawCentered(SpriteBatch sb, string text, int y, Color color)
    {
        var size = AssetManager.Font.MeasureString(text);
        sb.DrawString(AssetManager.Font, text,
            new Vector2((GameConstants.WindowWidth - size.X) / 2f, y), color);
    }
}
