using System;
using Microsoft.Xna.Framework;
using Game.Data;

namespace Game.Entities;

public class MutantEnemy : Enemy
{
    private float _shootTimer;
    private const float ShootCooldown = 3.0f;
    private const float ShootRange = 280f;

    public event Action<Vector2, bool> OnShoot;

    public MutantEnemy(Vector2 position) : base(position)
    {
        Health = 80;
        PatrolSpeed = 90f;
        PatrolRange = 150f;
        DrawColor = Color.DarkRed;
        Width = 28;
        Height = 38;
        Velocity.X = PatrolSpeed;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (!IsActive || IsCured) return;
        _shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void TryShoot(Vector2 playerPos)
    {
        if (!IsActive || IsCured) return;
        if (_shootTimer > 0) return;
        if (Math.Abs(playerPos.X - Position.X) > ShootRange) return;

        _shootTimer = ShootCooldown;
        bool goRight = playerPos.X > Position.X;
        OnShoot?.Invoke(new Vector2(goRight ? Position.X + Width : Position.X, Position.Y + Height / 2f), goRight);
    }
}
