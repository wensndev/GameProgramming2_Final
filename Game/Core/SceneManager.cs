using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Scenes;

namespace Game.Core;

/// <summary>
/// Controls which scene is active and handles the optional overlay layer.
/// An overlay (e.g. pause screen) is drawn on top of the current scene
/// without unloading it, so game state is preserved while paused.
/// </summary>
public class SceneManager
{
    private IScene _currentScene;
    private IScene _overlay;

    /// <summary>Unloads the current scene and overlay, then loads the new scene.</summary>
    public void ChangeScene(IScene newScene)
    {
        _overlay?.Unload();
        _overlay = null;
        _currentScene?.Unload();
        _currentScene = newScene;
        _currentScene.Load();
    }

    /// <summary>Adds an overlay on top of the current scene (used for pause).</summary>
    public void PushOverlay(IScene overlay)
    {
        _overlay = overlay;
        _overlay.Load();
    }

    /// <summary>Removes the overlay and resumes the underlying scene.</summary>
    public void PopOverlay()
    {
        _overlay?.Unload();
        _overlay = null;
    }

    /// <summary>
    /// Only updates the overlay when one is active so the gameplay scene is
    /// effectively frozen while the pause screen is showing.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        if (_overlay != null)
            _overlay.Update(gameTime);
        else
            _currentScene?.Update(gameTime);
    }

    /// <summary>Draws the current scene first, then the overlay on top.</summary>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _currentScene?.Draw(gameTime, spriteBatch);
        _overlay?.Draw(gameTime, spriteBatch);
    }
}
