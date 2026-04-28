using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Core;
using Game.Scenes;

namespace Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;

    public static InputManager Input { get; private set; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Data.GameConstants.WindowWidth;
        _graphics.PreferredBackBufferHeight = Data.GameConstants.WindowHeight;
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        Input = new InputManager();
        _sceneManager = new SceneManager();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        AssetManager.Load(GraphicsDevice, Content);
        _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update();
        _sceneManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _sceneManager.Draw(gameTime, _spriteBatch);
        base.Draw(gameTime);
    }
}
