using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MyGame.Entity;
using static MyGame.Component.GamePhysicsManager;
using static MyGame.Component.Messenger;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.Component
{
    public class GameTiledMapManager
    {
        private MessengerReadOnly messenger;
        private TiledMap tiledMap;
        private TiledMapRenderer tiledMapRenderer;
        public List<IEntity> Entities { get; set; }
        public Vector2[] LevelBeginEndDoors { get; set; }
        public Vector2 TiledMapOffset { get; set; }
        public void Initialize(MessengerReadOnly messenger)
        {
            this.messenger = messenger;
            Entities = new();
            LevelBeginEndDoors = new Vector2[2];
            TiledMapOffset = new();
        }
        public void LoadTiledMap(string mapName, int tileLayersIndex, int objectLayersIndex)
        {
            tiledMap = messenger.Game.Content.Load<TiledMap>(mapName);
            tiledMapRenderer = new(messenger.Game.GraphicsDevice, tiledMap);
            //绘制地图碰撞
            if (messenger.GamePhysicsManager.CollisionComponent.IsEnabled) 
            {
                TileLayersCollision(messenger.Game, tiledMap, tileLayersIndex);
                ObjectLayersCollision(messenger.Game, tiledMap, objectLayersIndex);
            }
        }
        public void Update(GameTime gameTime)
        {
            //判断null，等待初始化
            if (tiledMapRenderer != null)
            {
                tiledMapRenderer.Update(gameTime);
            }
            foreach (IEntity entity in Entities)
            {
                if (entity.GetType() == typeof(EntitySquareTile))
                {
                    EntitySquareTile squareTile = (EntitySquareTile)entity;
                    entity.Bounds.Position = squareTile.InitializePositon - TiledMapOffset;
                }
                else if (entity.GetType() == typeof(EntityPolygonTile))
                {
                    EntityPolygonTile mapEntity = (EntityPolygonTile)entity;
                    entity.Bounds.Position = mapEntity.InitializePositon - TiledMapOffset;
                }
                else if (entity.GetType() == typeof(EntitySquareDoor))
                {
                    EntitySquareDoor squareDoor = (EntitySquareDoor)entity;
                    entity.Bounds.Position = squareDoor.InitializePositon - TiledMapOffset;
                }
                entity.Update(gameTime);
            }
        }
        public void Draw()
        {
            messenger.SpriteBatch.Begin();
            foreach (IEntity entity in Entities)
            {
                entity.Draw(messenger.SpriteBatch);
            }
            if (tiledMapRenderer != null)
            {
                TranslateMap();
            }
            messenger.SpriteBatch.End();
        }
        //移动地图相机
        public void TranslateMap()
        {
            Vector2 playerWorldPosition = messenger.GameCameraManager.MainCamera.ScreenToWorld(-TiledMapOffset.X, TiledMapOffset.Y);
            Matrix viewMatrix = messenger.GameCameraManager.MainCamera.GetViewMatrix() * Matrix.CreateTranslation(playerWorldPosition.X, playerWorldPosition.Y, 0);
            //解决瓦片间有间隙问题——Texture Bleeding
            messenger.Game.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            tiledMapRenderer.Draw(viewMatrix);
        }
        //检测LevelBeginDoor、LevelEndDoor，设置玩家开始的位置
        //检测非瓦片集对象
        private void ObjectNonTilesetCollision(TiledMapObject tiledMapObject) 
        {
            tiledMapObject.Properties.TryGetValue("LevelBeginDoor", out string beginDoor);
            tiledMapObject.Properties.TryGetValue("LevelEndDoor", out string endDoor);
            string objectType = tiledMapObject.Type;
            if (beginDoor != null && bool.Parse(beginDoor) && objectType == "CollisionRectangle")
            {
                Point2 position = new(tiledMapObject.Position.X, tiledMapObject.Position.Y);
                EntitySquareDoor squareDoor = new(new RectangleF(position, new Size2(tiledMapObject.Size.Width, tiledMapObject.Size.Height)));
                squareDoor.Initialize(messenger);
                squareDoor.DoorType = EntitySquareDoor.DoorTypes.BeginDoor;
                Entities.Add(squareDoor);
                messenger.GamePhysicsManager.CollisionComponent.Insert(squareDoor);
                LevelBeginEndDoors[0] = position;
            }
            if (endDoor != null && bool.Parse(endDoor) && objectType == "CollisionRectangle")
            {
                Point2 position = new(tiledMapObject.Position.X, tiledMapObject.Position.Y);
                EntitySquareDoor squareDoor = new(new RectangleF(position, new Size2(tiledMapObject.Size.Width, tiledMapObject.Size.Height)));
                squareDoor.Initialize(messenger);
                squareDoor.DoorType = EntitySquareDoor.DoorTypes.EndDoor;
                Entities.Add(squareDoor);
                messenger.GamePhysicsManager.CollisionComponent.Insert(squareDoor);
                LevelBeginEndDoors[1] = position;
            }
        }
        //绘制TiledMapEditor绘制的ObjectLayers类型的碰撞形状
        private void ObjectLayersCollision(Game game, TiledMap tiledMap, int objectLayerIndex)
        {
            TiledMapObject[] tiledMapObjects = tiledMap.ObjectLayers.ElementAt(objectLayerIndex).Objects;
            foreach (TiledMapObject tiledMapObject in tiledMapObjects) //GameObject层的对象
            {
                ObjectNonTilesetCollision(tiledMapObject);
                TiledMapProperties properties = tiledMapObject.Properties;
                string globalIdentifierValue;
                properties.TryGetValue("GlobalIdentifier", out globalIdentifierValue);
                if (globalIdentifierValue == null)
                {
                    continue;
                }
                TiledMapTileset tiledMapTileset = tiledMap.GetTilesetByTileGlobalIdentifier(int.Parse(globalIdentifierValue));
                foreach (TiledMapTilesetTile tiledMapTilesetTile in tiledMapTileset.Tiles)
                {
                    List<TiledMapObject> tiledMapTilesetTileObjects = tiledMapTilesetTile.Objects;//该对象所在的tsx瓦片集每一个tile所吸附的对象
                    if (tiledMapTilesetTileObjects.Count == 0)
                    {
                        continue;
                    }
                    //判断对象类型（满足properties带有Collison和对象类型为CollisionPoint或CollisionRectangle），点还是矩阵
                    TiledMapObject tiledMapTilesetFirstTileObject = tiledMapTilesetTileObjects.First();
                    TiledMapProperties tilesetFirstTileObjectProperties = tiledMapTilesetFirstTileObject.Properties;
                    string value;
                    tilesetFirstTileObjectProperties.TryGetValue("Collision", out value);
                    string objectType = tiledMapTilesetFirstTileObject.Type;
                    if (value != null && bool.Parse(value) && objectType == "CollisionPoint")
                    {
                        Polygon polygon = null;
                        bool drawPolygonFlag = false;
                        List<Vector2> collisionPoints = new();
                        foreach (TiledMapObject tiledMapTilesetTileObject in tiledMapTilesetTileObjects)
                        {
                            collisionPoints.Add(tiledMapTilesetTileObject.Position);
                        }
                        if (collisionPoints.Count > 0)
                        {
                            polygon = new(collisionPoints);
                            drawPolygonFlag = true;
                        }
                        else
                        {
                            collisionPoints = null;
                        }
                        if (drawPolygonFlag)
                        {
                            Point2 tilePosition = new(tiledMapObject.Position.X, tiledMapObject.Position.Y);
                            EntityPolygonTile polygonTile = new(tilePosition, polygon);
                            polygonTile.Initialize(messenger);
                            Entities.Add(polygonTile);
                            messenger.GamePhysicsManager.CollisionComponent.Insert(polygonTile);
                        }
                    }
                    else if (value != null && bool.Parse(value) && objectType == "CollisionRectangle")
                    {
                        Point2 tilePosition = new(tiledMapObject.Position.X, tiledMapObject.Position.Y);
                        EntitySquareTile squareTile = new(new RectangleF(tilePosition, new Size2(tiledMapObject.Size.Width, tiledMapObject.Size.Height)));
                        squareTile.Initialize(messenger);
                        Entities.Add(squareTile);
                        messenger.GamePhysicsManager.CollisionComponent.Insert(squareTile);
                    }
                }
            }
        }
        //绘制TiledMapEditor绘制的TileLayers类型的碰撞形状
        private void TileLayersCollision(Game game, TiledMap tiledMap, int tileLayerIndex)
        {
            TiledMapTile[] tiledMapTiles = tiledMap.TileLayers.ElementAt(tileLayerIndex).Tiles;
            foreach (TiledMapTile tile in tiledMapTiles)
            {
                if (tile.IsBlank)
                {
                    continue;
                }
                int globalIdentifier = tile.GlobalIdentifier;
                TiledMapTileset tiledMapTileset = tiledMap.GetTilesetByTileGlobalIdentifier(globalIdentifier);
                List<TiledMapTilesetTile> tiledMapTilesetTiles = tiledMapTileset.Tiles;
                foreach (var tiledMapTilesetTile in tiledMapTilesetTiles)
                {
                    if (tiledMapTilesetTile.LocalTileIdentifier + 1 == globalIdentifier)
                    {
                        List<TiledMapObject> tiledMapTilesetTileObjects = tiledMapTilesetTile.Objects;
                        if (tiledMapTilesetTileObjects.Count == 0)
                        {
                            continue;
                        }
                        //判断对象类型（满足properties带有Collison和对象类型为CollisionPoint或CollisionRectangle），点还是矩阵
                        TiledMapObject tiledMapTilesetFirstTileObject = tiledMapTilesetTileObjects.First();
                        TiledMapProperties properties = tiledMapTilesetFirstTileObject.Properties;
                        string value;
                        properties.TryGetValue("Collision", out value);
                        string objectType = tiledMapTilesetFirstTileObject.Type;
                        if (value != null && bool.Parse(value) && objectType == "CollisionPoint")
                        {
                            Polygon polygon = null;
                            bool drawPolygonFlag = false;
                            List<Vector2> collisionPoints = new();
                            foreach (TiledMapObject tiledMapTilesetTileObject in tiledMapTilesetTileObjects)
                            {
                                if (!tile.IsFlippedHorizontally)
                                {
                                    collisionPoints.Add(tiledMapTilesetTileObject.Position);
                                }
                                else
                                {
                                    Vector2 verticePos;
                                    verticePos.X = messenger.GameConfig.TileWidth - tiledMapTilesetTileObject.Position.X ;
                                    verticePos.Y = tiledMapTilesetTileObject.Position.Y;
                                    collisionPoints.Add(verticePos);
                                }
                            }
                            if (collisionPoints.Count > 0)
                            {
                                polygon = new(collisionPoints);
                                drawPolygonFlag = true;
                            }
                            if (drawPolygonFlag)
                            {
                                Point2 tilePosition = new(messenger.GameConfig.TileWidth * tile.X, messenger.GameConfig.TileHeight * tile.Y);
                                /* 
                                   直接绘制多边形和下面的边形切割成碰撞矩形绘制的两遍，此处不再添加
                                   EntityPolygonTile polygonTile = new EntityPolygonTile(tilePosition, polygon);
                                   Entities.Add(polygonTile);
                                */
                                //多边形切割成碰撞矩形
                                if (messenger.GameConfig.CollisionAlgorithm == CollisionAlgorithm.EdgeLeftRowScanMergeRectangle)
                                {
                                    //扫描形状最左侧边界，逐行绘制碰撞矩形，并进一步将Position.X相同，等宽的矩形合并成一个大矩形算法
                                    float pixalRectangleHeight = 1f;
                                    float pixalRectangleWidth = 1f;
                                    List<RectangleF> rectangleList = new();
                                    for (float row = 0; row <= polygon.Bottom - polygon.Top;)//行扫描
                                    {
                                        pixalRectangleWidth = 1f;
                                        float startDrawCol = 0;
                                        for (float col = 0; col <= polygon.Right - polygon.Left;)//列扫描 
                                        {
                                            if (polygon.Contains(col, row))
                                            {
                                                pixalRectangleWidth++;
                                            }
                                            if (polygon.Contains(col, row) && !polygon.Contains(col - 1f, row))
                                            {
                                                startDrawCol = col;
                                            }
                                            col += 1;
                                        }
                                        RectangleF rectangle = new(tilePosition.X + startDrawCol, tilePosition.Y + row, pixalRectangleWidth, pixalRectangleHeight);
                                        rectangleList.Add(rectangle);
                                        row += 1;
                                    }
                                    float mergedRectangleHeight = 1f;
                                    float mergedRectangleWidth = 1f;
                                    int topBorderThickness = 0;//上边界跳过topBorderThickness个矩形不进行合并
                                    for (int i = 0; i < rectangleList.Count - 1; i++)
                                    {
                                        RectangleF currentRectangle = rectangleList[i];
                                        RectangleF nextRectangle = rectangleList[i + 1];
                                        mergedRectangleWidth = currentRectangle.Width;
                                        if (i > topBorderThickness - 1 && nextRectangle.Width == currentRectangle.Width && nextRectangle.Position.X == currentRectangle.Position.X)
                                        {
                                            mergedRectangleHeight += nextRectangle.Height;
                                        }
                                        else
                                        {
                                            RectangleF mergedRectangle = new(currentRectangle.X, currentRectangle.Y - (mergedRectangleHeight - 1f)/*向上找第一个矩形的位置*/, mergedRectangleWidth, mergedRectangleHeight);
                                            //如果这里不判断，会在多边形外可能出现游离的碰撞点
                                            if (polygon.Contains(mergedRectangle.TopLeft - tilePosition))
                                            {
                                                EntitySquareTile squareTile = new(mergedRectangle);
                                                squareTile.Initialize(messenger);
                                                Entities.Add(squareTile);
                                                messenger.GamePhysicsManager.CollisionComponent.Insert(squareTile);
                                                mergedRectangleHeight = 1f;
                                            }
                                        }
                                    }
                                }
                                else if (messenger.GameConfig.CollisionAlgorithm == CollisionAlgorithm.EdgeLeftRowScanRectangle)
                                {
                                    //扫描形状最左侧边界，逐行绘制碰撞矩形算法
                                    float pixalRectangleHeight = 1f;
                                    float pixalRectangleWidth = 1f;
                                    for (float row = 0; row <= polygon.Bottom - polygon.Top;)//行扫描
                                    {
                                        pixalRectangleWidth = 1f;
                                        float startDrawCol = 0;
                                        for (float col = 0; col <= polygon.Right - polygon.Left;)//列扫描 
                                        {
                                            if (polygon.Contains(col, row))
                                            {
                                                pixalRectangleWidth++;
                                            }
                                            if (polygon.Contains(col, row) && !polygon.Contains(col - 1f, row))
                                            {
                                                startDrawCol = col;
                                            }
                                            col += 1;
                                        }
                                        RectangleF rectangle = new(tilePosition.X + startDrawCol, tilePosition.Y + row, pixalRectangleWidth, pixalRectangleHeight);
                                        //如果这里不判断，会在多边形外可能出现游离的碰撞点
                                        if (polygon.Contains(rectangle.TopLeft - tilePosition))
                                        {
                                            EntitySquareTile squareTile = new(rectangle);
                                            squareTile.Initialize(messenger);
                                            Entities.Add(squareTile);
                                            messenger.GamePhysicsManager.CollisionComponent.Insert(squareTile);
                                        }
                                        row += 1;
                                    }
                                }
                                else if (messenger.GameConfig.CollisionAlgorithm == CollisionAlgorithm.EdgePointRectangle)
                                {
                                    //扫描形状全部边界，描边逐点绘制碰撞矩形算法
                                    float pixalRectangleHeight = 1f;
                                    float pixalRectangleWidth = 1f;
                                    for (float row = 0; row <= polygon.Bottom - polygon.Top;)//行扫描
                                    {
                                        for (float col = 0; col <= polygon.Right - polygon.Left;)//列扫描 
                                        {
                                            RectangleF rectangle = new(tilePosition.X + row, tilePosition.Y + col, pixalRectangleWidth, pixalRectangleHeight);
                                            RectangleF rectangleOutOfTopBorder = new(tilePosition.X + row, tilePosition.Y + col - 1f, pixalRectangleWidth, pixalRectangleHeight);
                                            RectangleF rectangleOutOfBottomBorder = new(tilePosition.X + row, tilePosition.Y + col + 1f, pixalRectangleWidth, pixalRectangleHeight);
                                            RectangleF rectangleOutOfLeftBorder = new(tilePosition.X + row - 1f, tilePosition.Y + col, pixalRectangleWidth, pixalRectangleHeight);
                                            RectangleF rectangleOutOfRightBorder = new(tilePosition.X + row + 1f, tilePosition.Y + col, pixalRectangleWidth, pixalRectangleHeight);
                                            if (polygon.Contains(rectangle.Center - tilePosition) && (!polygon.Contains(rectangleOutOfTopBorder.Center - tilePosition) || !polygon.Contains(rectangleOutOfBottomBorder.Center - tilePosition) || !polygon.Contains(rectangleOutOfLeftBorder.Center - tilePosition) || !polygon.Contains(rectangleOutOfRightBorder.Center - tilePosition)))
                                            {
                                                EntitySquareTile squareTile = new(rectangle);
                                                squareTile.Initialize(messenger);
                                                Entities.Add(squareTile);
                                                messenger.GamePhysicsManager.CollisionComponent.Insert(squareTile);
                                            }
                                            col += 1;
                                        }
                                        row += 1;
                                    }
                                }
                            }
                        }
                        else if (value != null && bool.Parse(value) && objectType == "CollisionRectangle")
                        {
                            Vector2 position = new(messenger.GameConfig.TileWidth * tile.X + tiledMapTilesetFirstTileObject.Position.X, messenger.GameConfig.TileHeight * tile.Y + tiledMapTilesetFirstTileObject.Position.Y);
                            EntitySquareTile squareTile = new(new RectangleF(position, new Size2(tiledMapTilesetFirstTileObject.Size.Width, tiledMapTilesetFirstTileObject.Size.Height)));
                            squareTile.Initialize(messenger);
                            Entities.Add(squareTile);
                            messenger.GamePhysicsManager.CollisionComponent.Insert(squareTile);
                        }
                    }
                }
            }
        }
    }
}
