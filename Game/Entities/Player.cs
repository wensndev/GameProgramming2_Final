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
    private float _animTimer;       
    private bool CanShoot => _shootTimer <= 0;
    private readonly AnimatedSprite _idleAnim;
    private readonly AnimatedSprite _runAnim;

    public event Action<Vector2, bool, bool> OnShoot;

    public Player(Vector2 startPosition)
    {
        Position = startPosition;
        Width  = 24;
        Height = 36;
        DrawColor = Color.CornflowerBlue;

        var sheet = AssetManager.PlayerSheet;
        if (sheet != null)
        {
            _idleAnim = new AnimatedSprite(sheet, 32, 48, startFrame: 0, frameCount: 2, frameTime: 0.35f);
            _runAnim  = new AnimatedSprite(sheet, 32, 48, startFrame: 2, frameCount: 2, frameTime: 0.12f);
        }
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
        _animTimer  += dt;
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

        bool dynamic = State == PlayerState.Run || State == PlayerState.Jump || State == PlayerState.Fall;
        if (dynamic) _runAnim?.Update(gameTime);
        else         _idleAnim?.Update(gameTime);
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

        int drawX = (int)Position.X;
        int drawY = (int)Position.Y;

        if (State == PlayerState.Idle)
            drawY += (int)(Math.Sin(_animTimer * 3.0) * 1.5);

        bool dynamic = State == PlayerState.Run || State == PlayerState.Jump || State == PlayerState.Fall;

        if (_idleAnim != null)
        {
            var anim     = dynamic ? (_runAnim ?? _idleAnim) : _idleAnim;
            int spriteX  = drawX + (Width - anim.FrameWidth)  / 2;
            int spriteY  = drawY + (Height - anim.FrameHeight);
            var effects  = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            anim.Draw(spriteBatch, new Vector2(spriteX, spriteY), Color.White, effects);
        }
        else
        {
            AssetManager.DrawRect(spriteBatch, drawX, drawY, Width, Height, DrawColor);
            int eyeX = FacingRight ? drawX + Width - 6 : drawX + 1;
            AssetManager.DrawRect(spriteBatch, eyeX, drawY + 6, 5, 5, Color.White);
            if (State == PlayerState.Run && IsGrounded)
            {
                bool stepRight = Math.Sin(_animTimer * 12.0) > 0;
                int legX = stepRight ? drawX + Width - 8 : drawX + 1;
                AssetManager.DrawRect(spriteBatch, legX, drawY + Height - 8, 7, 8, Color.DarkBlue);
            }
        }
    }
}
