using System;
using Microsoft.Xna.Framework;
using Game.Data;

namespace Game.Core;

/// <summary>
/// 2D camera that keeps a target centred on screen.
/// Clamped so the view never scrolls outside the world boundaries.
/// </summary>
public class Camera2D
{
    public Vector2 Position { get; private set; }

    /// <summary>
    /// Translation matrix passed to SpriteBatch.Begin so all world-space
    /// sprites are shifted by the negative camera position.
    /// </summary>
    public Matrix Transform => Matrix.CreateTranslation(-Position.X, -Position.Y, 0);

    /// <summary>
    /// Centers the camera on the target while staying within the world bounds.
    /// </summary>
    public void Follow(Vector2 target, int worldWidth, int worldHeight)
    {
        float x = target.X - GameConstants.WindowWidth / 2f;
        float y = target.Y - GameConstants.WindowHeight / 2f;
        x = Math.Clamp(x, 0, Math.Max(0, worldWidth - GameConstants.WindowWidth));
        y = Math.Clamp(y, 0, Math.Max(0, worldHeight - GameConstants.WindowHeight));
        Position = new Vector2(x, y);
    }
}
