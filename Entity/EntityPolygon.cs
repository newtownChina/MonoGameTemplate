using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using MyGame.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyGame.Component.Messenger;

namespace MyGame.Entity
{
    public abstract class EntityPolygon : IEntity
    {
        protected MessengerReadOnly messenger;
        protected Polygon polygon;
        public IShapeF Bounds { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 InitializePositon { get; set; }//初始世界位置，不变
        public EntityPolygon(Point2 position, Polygon polygon)
        {
            Bounds = polygon.BoundingRectangle;
            Bounds.Position = position;
            InitializePositon = Bounds.Position;
            this.polygon = polygon;
            RandomizeVelocity();
        }
        public void Initialize(MessengerReadOnly messenger)
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
