using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Core;

public static class AssetManager
{
    public static Texture2D Pixel { get; private set; }
    public static SpriteFont Font { get; private set; }

    public static void Load(GraphicsDevice graphicsDevice, ContentManager content)
    {
        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });
        Font = content.Load<SpriteFont>("Fonts/DefaultFont");
    }

    public static void DrawRect(SpriteBatch sb, Rectangle rect, Color color)
        => sb.Draw(Pixel, rect, color);

    public static void DrawRect(SpriteBatch sb, int x, int y, int w, int h, Color color)
        => sb.Draw(Pixel, new Rectangle(x, y, w, h), color);
}
