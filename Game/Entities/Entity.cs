using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;

namespace Game.Entities;

public abstract class Entity
{
    public Vector2 Position;
    public Vector2 Velocity;
    public int Width;
    public int Height;
    public bool IsActive = true;
    public Color DrawColor = Color.White;

    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
            AssetManager.DrawRect(spriteBatch, Bounds, DrawColor);
    }
}
