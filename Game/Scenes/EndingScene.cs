using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;

namespace Game.Scenes;

public class EndingScene : IScene
{
    private readonly SceneManager _sceneManager;
    private readonly int _killed;
    private readonly int _cured;
    private readonly bool _goodEnding;

    public EndingScene(SceneManager sceneManager, int killed, int cured)
    {
        _sceneManager = sceneManager;
        _killed = killed;
        _cured = cured;
        _goodEnding = cured > killed;
    }

    public void Load() { }
    public void Unload() { }

    public void Update(GameTime gameTime)
    {
        if (Game1.Input.IsPressed(Keys.Enter) || Game1.Input.IsPressed(Keys.Escape))
            _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        var bgColor = _goodEnding ? new Color(0, 20, 60) : new Color(30, 0, 0);
        AssetManager.DrawRect(spriteBatch, 0, 0, GameConstants.WindowWidth, GameConstants.WindowHeight, bgColor);

        var title = _goodEnding ? "GOOD ENDING: You Survived" : "BAD ENDING: It Is Over";
        DrawCentered(spriteBatch, title, 220, _goodEnding ? Color.Cyan : Color.OrangeRed);
        DrawCentered(spriteBatch, $"Killed: {_killed}    Cured: {_cured}", 290, Color.White);

        var msg = _goodEnding
            ? "You found the antidote. There is still hope."
            : "You killed them all. The infection won.";
        DrawCentered(spriteBatch, msg, 340, Color.LightGray);
        DrawCentered(spriteBatch, "Enter / ESC: Main Menu", 420, Color.Gray);

        spriteBatch.End();
    }

    private void DrawCentered(SpriteBatch sb, string text, int y, Color color)
    {
        var size = AssetManager.Font.MeasureString(text);
        sb.DrawString(AssetManager.Font, text,
            new Vector2((GameConstants.WindowWidth - size.X) / 2f, y), color);
    }
}
