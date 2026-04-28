using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;

namespace Game.UI;

public class HealthBar
{
    private readonly Vector2 _position;
    private const int Width = 200;
    private const int Height = 24;

    public HealthBar(Vector2 position) => _position = position;

    public void Draw(SpriteBatch spriteBatch, int current, int max)
    {
        int x = (int)_position.X, y = (int)_position.Y;
        AssetManager.DrawRect(spriteBatch, x, y, Width, Height, Color.DarkRed);
        int fill = (int)(Width * ((float)current / max));
        AssetManager.DrawRect(spriteBatch, x, y, fill, Height, Color.LimeGreen);
        spriteBatch.DrawString(AssetManager.Font, $"HP: {current}/{max}",
            _position + new Vector2(4, 4), Color.White);
    }
}
