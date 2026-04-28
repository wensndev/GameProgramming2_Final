using System;
using Microsoft.Xna.Framework;
using Game.Data;

namespace Game.Core;

public class Camera2D
{
    public Vector2 Position { get; private set; }

    public Matrix Transform => Matrix.CreateTranslation(-Position.X, -Position.Y, 0);

    public void Follow(Vector2 target, int worldWidth, int worldHeight)
    {
        float x = target.X - GameConstants.WindowWidth / 2f;
        float y = target.Y - GameConstants.WindowHeight / 2f;
        x = Math.Clamp(x, 0, Math.Max(0, worldWidth - GameConstants.WindowWidth));
        y = Math.Clamp(y, 0, Math.Max(0, worldHeight - GameConstants.WindowHeight));
        Position = new Vector2(x, y);
    }
}
