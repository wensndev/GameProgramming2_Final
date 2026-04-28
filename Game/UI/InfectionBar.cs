using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;

namespace Game.UI;

public class InfectionBar
{
    private readonly Vector2 _position;
    private const int Width = 200;
    private const int Height = 24;

    public InfectionBar(Vector2 position) => _position = position;

    public void Draw(SpriteBatch spriteBatch, float level)
    {
        int x = (int)_position.X, y = (int)_position.Y;
        AssetManager.DrawRect(spriteBatch, x, y, Width, Height, Color.DarkGray);
        AssetManager.DrawRect(spriteBatch, x, y, (int)(Width * level), Height, Color.Purple);
        spriteBatch.DrawString(AssetManager.Font, $"Infection: {(int)(level * 100)}%",
            _position + new Vector2(4, 4), Color.White);
    }
}
