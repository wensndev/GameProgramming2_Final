using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;
using Game.Systems;

namespace Game.Entities;

public class NotePickup : Entity
{
    public string NoteText { get; }

    public NotePickup(Vector2 position, string noteText)
    {
        Position = position;
        NoteText = noteText;
        Width = 14;
        Height = 18;
        DrawColor = Color.Gold;
    }

    public void TryCollect(Rectangle playerBounds, NoteSystem notes)
    {
        if (!IsActive) return;
        if (!Bounds.Intersects(playerBounds)) return;
        IsActive = false;
        notes.Collect(NoteText);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        base.Draw(spriteBatch);
        spriteBatch.DrawString(AssetManager.Font, "!",
            new Vector2(Position.X + 3, Position.Y - 16), Color.Yellow);
    }
}
