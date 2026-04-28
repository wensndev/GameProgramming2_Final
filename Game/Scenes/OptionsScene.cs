using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;

namespace Game.Scenes;

public class OptionsScene : IScene
{
    private readonly SceneManager _sceneManager;

    public OptionsScene(SceneManager sceneManager) => _sceneManager = sceneManager;

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
        DrawCentered(spriteBatch, "CONTROLS", 160, Color.White);

        string[] lines =
        {
            "Move:        Arrow Keys / WASD",
            "Jump:        Space / W / Up",
            "Shoot:       Z / Left Ctrl",
            "Switch Mode: Q  (unlocked after NPC dialogue)",
            "Talk / Pick: E / F",
            "Notes:       Tab",
            "Pause:       ESC / P",
        };

        for (int i = 0; i < lines.Length; i++)
            spriteBatch.DrawString(AssetManager.Font, lines[i], new Vector2(380, 220 + i * 30), Color.LightGray);

        DrawCentered(spriteBatch, "ESC / Enter: Back", 560, Color.Gray);
        spriteBatch.End();
    }

    private void DrawCentered(SpriteBatch sb, string text, int y, Color color)
    {
        var size = AssetManager.Font.MeasureString(text);
        sb.DrawString(AssetManager.Font, text,
            new Vector2((GameConstants.WindowWidth - size.X) / 2f, y), color);
    }
}
