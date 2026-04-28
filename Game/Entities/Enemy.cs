using Microsoft.Xna.Framework;
using Game.Data;
using Game.World;

namespace Game.Entities;

public class Enemy : Entity
{
    public int Health { get; protected set; } = 40;
    public bool IsCured { get; private set; }
    protected float PatrolSpeed = 55f;
    protected float PatrolRange = 96f;
    private Vector2 _startPosition;

    public Enemy(Vector2 position)
    {
        Position = position;
        _startPosition = position;
        Width = 24;
        Height = 32;
        DrawColor = Microsoft.Xna.Framework.Color.IndianRed;
        Velocity.X = PatrolSpeed;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) IsActive = false;
    }

    public void Cure()
    {
        IsCured = true;
        Velocity.X = 0;
        DrawColor = Color.LightGreen;
    }

    public override void Update(GameTime gameTime)
    {
        if (!IsActive || IsCured) return;
        if (Position.X > _startPosition.X + PatrolRange) Velocity.X = -PatrolSpeed;
        if (Position.X < _startPosition.X - PatrolRange) Velocity.X = PatrolSpeed;
    }

    public void ApplyPhysics(TileMap map, float dt)
    {
        if (IsCured) return;
        Velocity.Y += GameConstants.Gravity * dt;
        CollisionHelper.Move(ref Position, ref Velocity, Width, Height, map, dt, out bool onGround);

        if (!onGround) return;
        int ts = GameConstants.TileSize;
        if (Velocity.X > 0 && !map.IsSolid((int)((Position.X + Width + 2) / ts), (int)((Position.Y + Height + 1) / ts)))
            Velocity.X = -PatrolSpeed;
        if (Velocity.X < 0 && !map.IsSolid((int)((Position.X - 2) / ts), (int)((Position.Y + Height + 1) / ts)))
            Velocity.X = PatrolSpeed;
    }
}
