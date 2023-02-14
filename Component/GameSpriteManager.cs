using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MyGame.Entity;
using static MyGame.Component.Messenger;

namespace MyGame.Component
{
    public class GameSpriteManager
    {
        private MessengerReadOnly messenger;
        private List<IEntity> spriteEntities;
        private Texture2D player;
        private Texture2D enemy;
        public EntityPlayer EntityPlayer { get; set; }
        public void Initialize(MessengerReadOnly messenger)
        {
            this.messenger = messenger;
            player = messenger.Game.Content.Load<Texture2D>("marioRight1");
            enemy = messenger.Game.Content.Load<Texture2D>("marioRight1");
            spriteEntities = new();
        }
        public void InitScreen1Sprite() 
        {
            EntityPlayer = new(new RectangleF(messenger.GameTiledMapManager.LevelBeginEndDoors[0], new Size2(player.Width, player.Height)));
            EntityPlayer.Initialize(messenger);
            spriteEntities.Add(EntityPlayer);
            messenger.GamePhysicsManager.CollisionComponent.Insert(EntityPlayer);
            Random random = new();
            Point2 position;
            for (var i = 0; i < 0; i++)
            {
                position = new(random.Next(enemy.Width, messenger.Game.GraphicsDevice.Viewport.Width - enemy.Width), random.Next(enemy.Height, messenger.Game.GraphicsDevice.Viewport.Height - enemy.Height));
                //随机生成
                EntityPlayer entityPlayer = new(new RectangleF(position, new Size2(enemy.Width, enemy.Height)));
                entityPlayer.Initialize(messenger);
                spriteEntities.Add(entityPlayer);
            }
            foreach (IEntity entity in spriteEntities)
            {
                messenger.GamePhysicsManager.CollisionComponent.Insert(entity);
            }
        }
        public void InitScreen2Sprite()
        {
            EntityPlayer = new(new RectangleF(messenger.GameTiledMapManager.LevelBeginEndDoors[0], new Size2(player.Width, player.Height)));
            EntityPlayer.Initialize(messenger);
            spriteEntities.Add(EntityPlayer);
            messenger.GamePhysicsManager.CollisionComponent.Insert(EntityPlayer);
            Random random = new();
            Point2 position;
            for (var i = 0; i < 0; i++)
            {
                position = new(random.Next(enemy.Width, messenger.Game.GraphicsDevice.Viewport.Width - enemy.Width), random.Next(enemy.Height, messenger.Game.GraphicsDevice.Viewport.Height - enemy.Height));
                //随机生成
                EntityPlayer entityPlayer = new(new RectangleF(position, new Size2(enemy.Width, enemy.Height)));
                entityPlayer.Initialize(messenger);
                spriteEntities.Add(entityPlayer);
            }
            foreach (IEntity entity in spriteEntities)
            {
                messenger.GamePhysicsManager.CollisionComponent.Insert(entity);
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (IEntity entity in spriteEntities)
            {
                entity.Update(gameTime);
            }
        }
        public void Draw()
        {
            messenger.SpriteBatch.Begin();
            foreach (IEntity entity in spriteEntities)
            {
                entity.Draw(messenger.SpriteBatch);
            }
            messenger.SpriteBatch.End();
        }
    }
}
