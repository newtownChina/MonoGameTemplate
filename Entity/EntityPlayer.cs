using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static MyGame.Component.Messenger;

namespace MyGame.Entity
{
    public class EntityPlayer : EntitySquare
    {
        public EntityPlayerAnimation PlayerEntityAnimation { get; set; }
        public EntityTransform PlayerTransform { get; set; }
        public RectangleF[] Borders { get; set; }
        public CollisionSwitcher Switcher { get; set; }
        public bool CollisionFlag { get; set; }
        public enum CollisionSwitcher
        {
            OPEN,
            CLOSE
        }
        public override void Initialize(MessengerReadOnly messenger)
        {
            this.messenger = messenger;
            PlayerTransform = new()
            {
                position = Bounds.Position
            };
            PlayerEntityAnimation = new();
            PlayerEntityAnimation.Initialize(messenger, "EntityPlayer", "mario", messenger.GameConfig.PlayerWidth, messenger.GameConfig.PlayerHeight, 8, 0, 0, messenger.GameConfig.AnimationDuration);
            Borders = new RectangleF[4];
            Borders[0] = new()
            {
                Width = 20,
                Height = 3
            };

            Borders[1] = new()
            {
                Width = 1,
                Height = 20
            };

            Borders[2] = new()
            {
                Width = 20,
                Height = 1
            };

            Borders[3] = new()
            {
                Width = 1,
                Height = 20
            };
            messenger.GameSpriteManager.EntityPlayer = this;
        }
        public EntityPlayer(RectangleF rectangleF) : base(rectangleF)
        {
            Bounds = rectangleF;
            RandomizeVelocity();
        }
        public override void Update(GameTime gameTime)
        {
            //预碰撞检测，存放环绕player的上下左右瓦片
            List<RectangleF> preCollisionList = new();
            foreach (IEntity entity in messenger.GameTiledMapManager.Entities)
            {
                EntitySquareTile squareTile;
                RectangleF rectangleF;
                if (entity.GetType() == typeof(EntitySquareTile))
                {
                    squareTile = (EntitySquareTile)entity;
                    float preCollisionX = squareTile.InitializePositon.X - PlayerTransform.position.X - messenger.GameTiledMapManager.TiledMapOffset.X;
                    float preCollisionY = squareTile.InitializePositon.Y - PlayerTransform.position.Y;
                    if (Math.Abs(preCollisionX) < messenger.GameConfig.TileWidth + messenger.GameConfig.PlayerWidth && Math.Abs(preCollisionY) < messenger.GameConfig.TileWidth * 2)
                    {
                        rectangleF = (RectangleF)squareTile.Bounds;
                        rectangleF = new(rectangleF.Position, new Size2(rectangleF.Width, rectangleF.Height));
                        preCollisionList.Add(rectangleF);
                    }
                }
            }
            PlayerTransform.state.InitializeCollision();
            UpdateBorder();
            for (int i = 0; i < Borders.Length; i++)
            {
                RectangleF playerBounds = (RectangleF)Bounds;
                foreach (RectangleF rectangleF in preCollisionList)
                {
                    //解决下坠速度快穿过碰撞物的问题
                    if (rectangleF.Center.Y > playerBounds.Center.Y) 
                    {
                        RectangleF common = playerBounds.Intersection(rectangleF);
                        if (common.Height >= 4) //相交4像素
                        {
                            PlayerTransform.position = new Vector2(PlayerTransform.position.X, rectangleF.Position.Y-messenger.GameConfig.TileHeight); 
                        }
                    }
                    //border只可能和一个相交
                    if (Borders[i].Intersects(rectangleF))
                    {
                        if (i == 0)
                        {
                            PlayerTransform.state.collisionWithTop = true;
                            break;
                        }
                        if (i == 1)
                        {
                            PlayerTransform.state.collisionWithRight = true;
                            break;
                        }
                        if (i == 2)
                        {
                            PlayerTransform.state.collisionWithBottom = true;
                            if (messenger.GameConfig.DebugMode) 
                            {
                                Debug.WriteLine("CollisionWithBottom");
                            }
                            break;
                        }
                        if (i == 3)
                        {
                            PlayerTransform.state.collisionWithLeft = true;
                            break;
                        }
                    }
                }
            }
            if (Switcher == CollisionSwitcher.CLOSE) 
            {
                CollisionFlag = false;
            }
            PlayerEntityAnimation.Update(gameTime);
            Bounds.Position = PlayerTransform.position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (messenger.GameConfig.DebugMode) 
            {
                spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 1);
            }
            if (messenger.GameConfig.DebugMode)
            {
                foreach (RectangleF border in Borders)
                {
                    messenger.SpriteBatch.DrawRectangle(border, Color.Yellow);
                }
            }
            PlayerEntityAnimation.Draw();
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            Switcher = CollisionSwitcher.OPEN;
            if (Switcher == CollisionSwitcher.OPEN) 
            {
                CollisionFlag = true;
            }
            RectangleF otherBounds = (RectangleF)collisionInfo.Other.Bounds;
            float penetrationVectorX = collisionInfo.PenetrationVector.X;
            float penetrationVectorY = collisionInfo.PenetrationVector.Y;
            if (penetrationVectorY > 0 || otherBounds.Center.Y > PlayerTransform.position.Y + messenger.GameConfig.PlayerHeight)
            {
                if (PlayerTransform.state.movingRightJump)
                {
                    PlayerTransform.state.movingRightIdle = true;
                }
                if (PlayerTransform.state.movingLeftJump)
                {
                    PlayerTransform.state.movingLeftIdle = true;
                }
                PlayerTransform.state.InitializeJump();
            }
            messenger.GamePhysicsManager.ResetForce();
            Vector2 penetrationVector = collisionInfo.PenetrationVector;
            Vector2 positionTest = Bounds.Position - penetrationVector;
            if (positionTest.X >= 0 && positionTest.X <= messenger.Game.GraphicsDevice.Viewport.Width - messenger.GameConfig.PlayerWidth && positionTest.Y >= 0 && positionTest.Y <= messenger.Game.GraphicsDevice.Viewport.Height - messenger.GameConfig.PlayerHeight)
            {
                //落地弹性
                penetrationVector.Y = messenger.GameConfig.PnetrationMultiplier * penetrationVector.Y;
                //解决碰撞后发生横向瞬移的问题
                if (PlayerTransform.state.collisionWithTop || Math.Abs(penetrationVector.X) > 3)
                {
                    penetrationVector.X = 0;
                }
                Debug.WriteLine("penetrationVector.Y:"+ penetrationVector.Y);
                Bounds.Position -= penetrationVector;
                PlayerTransform.position = Bounds.Position;
            }
            Switcher = CollisionSwitcher.CLOSE;
        }
        private void UpdateBorder() 
        {
            Borders[0].X = PlayerTransform.position.X + 2;
            Borders[0].Y = PlayerTransform.position.Y;
            Borders[1].X = PlayerTransform.position.X + messenger.GameConfig.PlayerWidth;
            Borders[1].Y = PlayerTransform.position.Y + 4;
            Borders[2].X = PlayerTransform.position.X + 2;
            Borders[2].Y = PlayerTransform.position.Y + messenger.GameConfig.PlayerHeight;
            Borders[3].X = PlayerTransform.position.X - 1;
            Borders[3].Y = PlayerTransform.position.Y + 4;
        }
    }
}
