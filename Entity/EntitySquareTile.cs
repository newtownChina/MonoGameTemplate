using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Entity
{
    public class EntitySquareTile : EntitySquare
    {
        public EntitySquareTile(RectangleF rectangleF) : base(rectangleF)
        {
            Bounds = rectangleF;
            RandomizeVelocity();
        }
        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            /*if (messenger.GameConfig.DebugMode)
            {
                spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Red, 1);
            }*/
        }
        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
        }
        public override void RandomizeVelocity()
        {
            Velocity = new();
        }
    }
}
