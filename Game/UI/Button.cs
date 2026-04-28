using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;

namespace Game.UI;

public class Button
{
    public string Text { get; }
    public Rectangle Bounds { get; }
    public bool IsSelected { get; set; }

    public Button(string text, int x, int y, int width = 240, int height = 50)
    {
        Text = text;
        Bounds = new Rectangle(x, y, width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        AssetManager.DrawRect(spriteBatch, Bounds, IsSelected ? Color.Yellow : Color.DarkSlateGray);
        var textSize = AssetManager.Font.MeasureString(Text);
        var textPos = new Vector2(
            Bounds.X + (Bounds.Width - textSize.X) / 2f,
            Bounds.Y + (Bounds.Height - textSize.Y) / 2f
        );
        spriteBatch.DrawString(AssetManager.Font, Text, textPos, IsSelected ? Color.Black : Color.White);
    }
}
