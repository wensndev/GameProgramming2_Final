using Microsoft.Xna.Framework;
using Game.Data;

namespace Game.World;

public static class CollisionHelper
{
    public static void Move(
        ref Vector2 position, ref Vector2 velocity,
        int width, int height,
        TileMap map, float dt,
        out bool onGround)
    {
        onGround = false;
        int ts = GameConstants.TileSize;

        position.X += velocity.X * dt;
        int rMin = (int)(position.Y / ts);
        int rMax = (int)((position.Y + height - 1) / ts);

        if (velocity.X > 0)
        {
            int c = (int)((position.X + width) / ts);
            for (int r = rMin; r <= rMax; r++)
            {
                if (!map.IsSolid(c, r)) continue;
                position.X = c * ts - width;
                velocity.X = 0;
                break;
            }
        }
        else if (velocity.X < 0)
        {
            int c = (int)(position.X / ts);
            for (int r = rMin; r <= rMax; r++)
            {
                if (!map.IsSolid(c, r)) continue;
                position.X = (c + 1) * ts;
                velocity.X = 0;
                break;
            }
        }

        position.Y += velocity.Y * dt;
        int cMin = (int)(position.X / ts);
        int cMax = (int)((position.X + width - 1) / ts);

        if (velocity.Y > 0) // falling
        {
            int r = (int)((position.Y + height) / ts);
            for (int c = cMin; c <= cMax; c++)
            {
                if (!map.IsSolid(c, r)) continue;
                position.Y = r * ts - height;
                velocity.Y = 0;
                onGround = true;
                break;
            }
        }
        else if (velocity.Y < 0)
        {
            int r = (int)(position.Y / ts);
            for (int c = cMin; c <= cMax; c++)
            {
                if (!map.IsSolid(c, r)) continue;
                position.Y = (r + 1) * ts; // bump head on ceiling
                velocity.Y = 0;
                break;
            }
        }

        if (!onGround)
        {
            int r = (int)((position.Y + height + 1) / ts);
            for (int c = cMin; c <= cMax; c++)
            {
                if (!map.IsSolid(c, r)) continue;
                onGround = true;
                break;
            }
        }
    }
}
