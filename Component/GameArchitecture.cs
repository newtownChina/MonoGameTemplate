using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using static MyGame.Component.Messenger;

namespace MyGame.Component
{
    public class GameArchitecture
    {
        private static GameArchitecture self;
        private Game game;
        private GameConfig gameConfig;
        private GameCameraManager gameCameraManager;
        private GameEventManager gameEventManager;
        private GamePhysicsManager gamePhysicsManager;
        private GameScreenManager gameScreenManager;
        private GameSpriteManager gameSpriteManager;
        private GameTiledMapManager gameTiledMapManager;
        private GraphicsDeviceManager graphicsDeviceManager;
        public static GameArchitecture Create(Game game)
        {
            if (self == null)
            {
                self = new()
                {
                    game = game,
                    gameConfig = GameConfig.Create()
                };
                self.InitGraphicsDeviceManager();
                
            }
            return self;
        }
        private void InitGraphicsDeviceManager()
        {
            graphicsDeviceManager = new(game)
            {
                PreferredBackBufferWidth = gameConfig.TileWidth * gameConfig.TileColumns,
                PreferredBackBufferHeight = gameConfig.TileHeight * gameConfig.TileRows
            };
            game.Content.RootDirectory = "Content";
            game.IsMouseVisible = true;
        }
        public void Initialize(int screen)
        {
            Messenger messenger = Messenger.Create();
            gameCameraManager = new();
            gameEventManager = new();
            gamePhysicsManager = new();
            gameScreenManager = new();
            gameSpriteManager = new();
            gameTiledMapManager = new();

            messenger.Game = game;
            messenger.GameArchitecture = self;
            messenger.GameConfig = self.gameConfig;
            messenger.GameCameraManager = gameCameraManager;
            messenger.GameEventManager = gameEventManager;
            messenger.GamePhysicsManager = gamePhysicsManager;
            messenger.GameScreenManager = gameScreenManager;
            messenger.GameSpriteManager = gameSpriteManager;
            messenger.GameTiledMapManager = gameTiledMapManager;

            MessengerReadOnly readOnly = ReadOnly(messenger);
            gameCameraManager.Initialize(readOnly);
            gameEventManager.Initialize(readOnly);
            gamePhysicsManager.Initialize(readOnly);
            gameScreenManager.Initialize(readOnly);
            gameSpriteManager.Initialize(readOnly);
            gameTiledMapManager.Initialize(readOnly);

            gameScreenManager.LoadScreen(screen);
        }
        public void Update(GameTime gameTime) 
        {
            gameTiledMapManager.Update(gameTime);
            gameSpriteManager.Update(gameTime);
            gameCameraManager.Update(gameTime);
            gamePhysicsManager.Update(gameTime);
        }
        public void Draw() 
        {
            graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            //地图
            gameTiledMapManager.Draw();
            //相机
            gameCameraManager.Draw();
            //精灵
            gameSpriteManager.Draw();
        }
    }
}
