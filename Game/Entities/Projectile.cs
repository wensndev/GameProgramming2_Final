using Microsoft.Xna.Framework;
using Game.Data;
using Game.World;

namespace Game.Entities;

public class Projectile : Entity
{
    public bool IsCure { get; }
    public bool IsEnemyShot { get; }

    public Projectile(Vector2 position, bool goRight, bool isCure = false, bool isEnemyShot = false)
    {
        Position = position;
        IsCure = isCure;
        IsEnemyShot = isEnemyShot;
        Velocity.X = goRight ? GameConstants.ProjectileSpeed : -GameConstants.ProjectileSpeed;
        Width = 10;
        Height = 5;
        DrawColor = isEnemyShot ? Color.OrangeRed : (isCure ? Color.Cyan : Color.Yellow);
    }

    public override void Update(GameTime gameTime)
    {
        if (!IsActive) return;
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position.X += Velocity.X * dt;
        if (Position.X < -200 || Position.X > 20000) IsActive = false;
    }

    public void CheckTileCollision(TileMap map)
    {
        if (!IsActive) return;
        int ts = GameConstants.TileSize;
        int c = (int)(Position.X / ts);
        int r = (int)(Position.Y / ts);
        if (map.IsSolid(c, r)) IsActive = false;
    }
}
