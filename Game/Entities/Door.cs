using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;

namespace Game.Entities;

public class Door : Entity
{
    public bool IsUnlocked { get; private set; }

    public Door(Vector2 position)
    {
        Position = position;
        Width = 32;
        Height = 64;
        DrawColor = Color.DarkRed;
    }

    public void Unlock()
    {
        IsUnlocked = true;
        DrawColor = Color.LimeGreen;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        base.Draw(spriteBatch);
        string label = IsUnlocked ? "EXIT" : "LOCK";
        Color c = IsUnlocked ? Color.White : Color.Gray;
        spriteBatch.DrawString(AssetManager.Font, label,
            new Vector2(Position.X, Position.Y + Height / 2f - 8), c);
    }
}
