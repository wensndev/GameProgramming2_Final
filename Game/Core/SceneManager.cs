using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Scenes;

namespace Game.Core;

public class SceneManager
{
    private IScene _currentScene;
    private IScene _overlay;


    public void ChangeScene(IScene newScene)
    {
        _overlay?.Unload();
        _overlay = null;
        _currentScene?.Unload();
        _currentScene = newScene;
        _currentScene.Load();
    }


    public void PushOverlay(IScene overlay)
    {
        _overlay = overlay;
        _overlay.Load();
    }

    public void PopOverlay()
    {
        _overlay?.Unload();
        _overlay = null;
    }

    public void Update(GameTime gameTime)
    {
        if (_overlay != null)
            _overlay.Update(gameTime);
        else
            _currentScene?.Update(gameTime);
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _currentScene?.Draw(gameTime, spriteBatch);
        _overlay?.Draw(gameTime, spriteBatch);
    }
}
