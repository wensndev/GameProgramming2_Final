using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;
using Game.Systems;
using Game.World;

namespace Game.Entities;

public class Npc : Entity
{
    private readonly DialogueSystem _dialogue;
    private bool _talked;

    public Npc(Vector2 position, DialogueSystem dialogue)
    {
        Position = position;
        _dialogue = dialogue;
        Width = 22;
        Height = 34;
        DrawColor = Color.Gold;
    }

    public void TryTalk(Rectangle playerBounds)
    {
        if (_talked || _dialogue.IsActive) return;
        var trigger = new Rectangle(Bounds.X - 40, Bounds.Y - 10, Bounds.Width + 80, Bounds.Height + 20);
        if (!trigger.Intersects(playerBounds)) return;
        if (!(Game1.Input.IsPressed(Keys.F) || Game1.Input.IsPressed(Keys.E))) return;

        _talked = true;
        _dialogue.StartDialogue(DialogueData.NpcLines);
    }

    public void ApplyPhysics(TileMap map, float dt)
    {
        Velocity.Y += GameConstants.Gravity * dt;
        CollisionHelper.Move(ref Position, ref Velocity, Width, Height, map, dt, out _);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        base.Draw(spriteBatch);
        if (!_talked)
            spriteBatch.DrawString(AssetManager.Font, "[E]",
                new Vector2(Position.X - 5, Position.Y - 22), Color.Yellow);
    }
}
