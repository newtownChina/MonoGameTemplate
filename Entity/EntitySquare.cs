using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Entities;
using MyGame.Component;
using static MyGame.Component.Messenger;

namespace MyGame.Entity
{
    public abstract class EntitySquare : IEntity
    {
        protected MessengerReadOnly messenger;
        public IShapeF Bounds { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 InitializePositon { get; set; }//初始世界位置，不变
        public EntitySquare(RectangleF rectangleF)
        {
            Bounds = rectangleF;
            InitializePositon = Bounds.Position;
            RandomizeVelocity();
        }
        public virtual void Initialize(MessengerReadOnly messenger)
        {
            this.messenger = messenger;
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
        }
        public virtual void RandomizeVelocity()
        {
        }
    }
}
