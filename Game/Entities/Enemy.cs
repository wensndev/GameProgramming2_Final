using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;
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
    private float _pulseTimer;

    protected AnimatedSprite _walkAnim;

    public Enemy(Vector2 position)
    {
        Position       = position;
        _startPosition = position;
        Width          = 24;
        Height         = 32;
        DrawColor      = Color.IndianRed;
        Velocity.X     = PatrolSpeed;

        var sheet = AssetManager.EnemySheet;
        if (sheet != null)
            _walkAnim = new AnimatedSprite(sheet, 24, 32, startFrame: 0, frameCount: 3, frameTime: 0.15f);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) IsActive = false;
    }

    public void Cure()
    {
        IsCured    = true;
        Velocity.X = 0;
        DrawColor  = Color.LightGreen;
    }

    public override void Update(GameTime gameTime)
    {
        _pulseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (!IsActive || IsCured) return;
        _walkAnim?.Update(gameTime);
        if (Position.X > _startPosition.X + PatrolRange) Velocity.X = -PatrolSpeed;
        if (Position.X < _startPosition.X - PatrolRange) Velocity.X = PatrolSpeed;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;

        if (_walkAnim != null)
        {
            var effects = Velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Color tint = IsCured ? Color.LightGreen : Color.White;
            _walkAnim.Draw(spriteBatch, Position, tint, effects);
        }
        else
        {
            if (IsCured)
            {
                base.Draw(spriteBatch);
                return;
            }
            float pulse  = 0.72f + 0.28f * (float)Math.Sin(_pulseTimer * 3.5f);
            var pulsed   = new Color((int)(DrawColor.R * pulse), (int)(DrawColor.G * pulse), (int)(DrawColor.B * pulse));
            AssetManager.DrawRect(spriteBatch, Bounds, pulsed);
        }
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
