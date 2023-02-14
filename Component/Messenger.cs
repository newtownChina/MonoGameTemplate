using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Component
{
    //用于组件间引用
    public class Messenger
    {
        private static Messenger self;
        private static MessengerReadOnly readOnlySelf;
        private Game game;
        private GameArchitecture gameArchitecture;
        private GameConfig gameConfig;
        private GameCameraManager gameCameraManager;
        private GameEventManager gameEventManager;
        private GamePhysicsManager gamePhysicsManager;
        private GameScreenManager gameScreenManager;
        private GameSpriteManager gameSpriteManager;
        private GameTiledMapManager gameTiledMapManager;

        public Game Game { set => game = value;  }
        public GameArchitecture GameArchitecture { set => gameArchitecture = value; }
        public GameConfig GameConfig { set => gameConfig = value; }
        public GameCameraManager GameCameraManager { set => gameCameraManager = value; }
        public GameEventManager GameEventManager { set => gameEventManager = value; }
        public GamePhysicsManager GamePhysicsManager { set => gamePhysicsManager = value; }
        public GameScreenManager GameScreenManager { set => gameScreenManager = value; }
        public GameSpriteManager GameSpriteManager { set => gameSpriteManager = value; } 
        public GameTiledMapManager GameTiledMapManager { set => gameTiledMapManager = value; }
        public static Messenger Create()
        {
            if (self == null)
            {
                self = new();
            }
            return self;
        }
        public static MessengerReadOnly ReadOnly(Messenger messenger)
        {
            if (readOnlySelf == null)
            {
                readOnlySelf = new(messenger);
            }
            return readOnlySelf;
        }
        public class MessengerReadOnly
        {
            private Messenger messenger;
            private SpriteBatch spriteBatch;
            public Game Game => messenger.game;
            public GameConfig GameConfig => messenger.gameConfig;
            public GameArchitecture GameArchitecture => messenger.gameArchitecture;
            public GameCameraManager GameCameraManager => messenger.gameCameraManager;
            public GameEventManager GameEventManager => messenger.gameEventManager;
            public GamePhysicsManager GamePhysicsManager => messenger.gamePhysicsManager;
            public GameScreenManager GameScreenManager => messenger.gameScreenManager; 
            public GameSpriteManager GameSpriteManager => messenger.gameSpriteManager;
            public GameTiledMapManager GameTiledMapManager => messenger.gameTiledMapManager;
            public SpriteBatch SpriteBatch => spriteBatch;
            public MessengerReadOnly(Messenger messenger)
            {
                this.messenger = messenger;
                if (spriteBatch == null)
                {
                    spriteBatch = new(messenger.game.GraphicsDevice);
                }
            }
        }
    }
}
