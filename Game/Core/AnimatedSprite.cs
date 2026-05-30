using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Core;

// Minimal frame-cycling sprite animation. Draws one row of equally-sized frames.
public class AnimatedSprite
{
    private readonly Texture2D _texture;
    private readonly int _startFrame;    // index of the first frame in the sheet row
    private readonly int _frameCount;
    private readonly float _frameTime;
    private int _currentFrame;
    private float _timer;

    public int FrameWidth  { get; }
    public int FrameHeight { get; }

    // Parameters: startFrame = first frame index, frameCount = how many frames to cycle.
    // Source rectangle: (startFrame + currentFrame) * frameWidth.
    public AnimatedSprite(Texture2D texture, int frameWidth, int frameHeight,
                          int startFrame, int frameCount, float frameTime)
    {
        _texture    = texture;
        FrameWidth  = frameWidth;
        FrameHeight = frameHeight;
        _startFrame = startFrame;
        _frameCount = frameCount;
        _frameTime  = frameTime;
    }

    public void Update(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timer >= _frameTime)
        {
            _timer = 0;
            _currentFrame = (_currentFrame + 1) % _frameCount;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint,
                     SpriteEffects effects = SpriteEffects.None)
    {
        if (_texture == null) return;
        var src = new Rectangle((_startFrame + _currentFrame) * FrameWidth, 0, FrameWidth, FrameHeight);
        spriteBatch.Draw(_texture, position, src, tint, 0f, Vector2.Zero, 1f, effects, 0f);
    }
}
