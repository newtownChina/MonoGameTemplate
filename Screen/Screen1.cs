using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MyGame.Component;

namespace MyGame.Screen
{
    public class Screen1 : GameScreen
    {
        public readonly static string mapName = "MyGameMap1";
        public readonly static int collisionDetectingTileLayersIndex = 3;//仅TileLayer层内的index，不是所有层的index;
        public readonly static int collisionDetectingObjectLayersIndex = 0;//仅对象层内的index，不是所有层的index，只有一个对象层则为0;
        private GameTiledMapManager gameTiledMapManager;
        private GameSpriteManager gameSpriteManager;
        public Screen1(Game game, GameTiledMapManager gameTiledMapManager, GameSpriteManager gameSpriteManager) : base(game)
        {
            this.gameTiledMapManager = gameTiledMapManager;
            this.gameSpriteManager = gameSpriteManager;
        }
        public override void Initialize()
        {
            
        }
        public override void LoadContent()
        {
            gameTiledMapManager.LoadTiledMap(mapName,collisionDetectingTileLayersIndex,collisionDetectingObjectLayersIndex);
            gameSpriteManager.InitScreen1Sprite();
        }
        public override void Draw(GameTime gameTime)
        {

        }
        public override void Update(GameTime gameTime)
        {

        }
    }
}
