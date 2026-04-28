using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Core;
using Game.Data;
using Game.World;

namespace Game.Entities;

public enum PlayerState { Idle, Run, Jump, Fall, Dead }
public enum ShootMode { Kill, Cure }

public class Player : Entity
{
    public PlayerState State { get; private set; }
    public int Health { get; private set; } = GameConstants.PlayerMaxHealth;
    public bool IsGrounded { get; private set; }
    public bool FacingRight { get; private set; } = true;
    public ShootMode Mode { get; private set; } = ShootMode.Kill;
    public bool CureUnlocked { get; set; } = false;

    private float _shootTimer;
    private bool CanShoot => _shootTimer <= 0;

    public event Action<Vector2, bool, bool> OnShoot;

    public Player(Vector2 startPosition)
    {
        Position = startPosition;
        Width = 24;
        Height = 36;
        DrawColor = Color.CornflowerBlue;
    }

    public void TakeDamage(int amount)
    {
        if (State == PlayerState.Dead) return;
        Health = Math.Max(0, Health - amount);
        if (Health == 0) State = PlayerState.Dead;
    }

    public override void Update(GameTime gameTime)
    {
        if (!IsActive || State == PlayerState.Dead) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _shootTimer -= dt;
        var input = Game1.Input;

        Velocity.X = 0;
        if (input.IsDown(Keys.Left) || input.IsDown(Keys.A))
        {
            Velocity.X = -GameConstants.PlayerSpeed;
            FacingRight = false;
        }
        if (input.IsDown(Keys.Right) || input.IsDown(Keys.D))
        {
            Velocity.X = GameConstants.PlayerSpeed;
            FacingRight = true;
        }

        if ((input.IsPressed(Keys.Up) || input.IsPressed(Keys.W) || input.IsPressed(Keys.Space)) && IsGrounded)
            Velocity.Y = GameConstants.JumpForce;

        if (CureUnlocked && input.IsPressed(Keys.Q))
            Mode = Mode == ShootMode.Kill ? ShootMode.Cure : ShootMode.Kill;

        if ((input.IsPressed(Keys.Z) || input.IsPressed(Keys.LeftControl)) && CanShoot)
        {
            _shootTimer = GameConstants.ShootCooldown;
            float spawnX = FacingRight ? Position.X + Width : Position.X;
            OnShoot?.Invoke(new Vector2(spawnX, Position.Y + Height / 2f), FacingRight, Mode == ShootMode.Cure);
        }

        if (IsGrounded)
            State = Velocity.X != 0 ? PlayerState.Run : PlayerState.Idle;
        else
            State = Velocity.Y < 0 ? PlayerState.Jump : PlayerState.Fall;
    }

    public void ApplyPhysics(TileMap map, float dt)
    {
        Velocity.Y += GameConstants.Gravity * dt;
        CollisionHelper.Move(ref Position, ref Velocity, Width, Height, map, dt, out bool onGround);
        IsGrounded = onGround;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        base.Draw(spriteBatch);
        int eyeX = FacingRight ? (int)Position.X + Width - 6 : (int)Position.X + 1;
        AssetManager.DrawRect(spriteBatch, eyeX, (int)Position.Y + 6, 5, 5, Color.White);
    }
}
