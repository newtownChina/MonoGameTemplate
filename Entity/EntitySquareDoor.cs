using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Content;
using MyGame.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyGame.Component.GamePhysicsManager;
using static MyGame.Component.Messenger;

namespace MyGame.Entity
{
    public class EntitySquareDoor : EntitySquare
    {
        public DoorTypes DoorType { get; set; }
        public DoorStates State { get; set; }
        public EntitySquareDoor(RectangleF rectangleF) : base(rectangleF)
        {
            State = DoorStates.Close;
        }
        public enum DoorTypes
        {
            BeginDoor,
            EndDoor
        }
        public enum DoorStates{
            Open,
            Close
        }
        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Yellow, 1);
        }
        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            //门碰撞到了玩家
            if (collisionInfo.Other.GetType() == typeof(EntityPlayer) && State == DoorStates.Close && DoorType == DoorTypes.EndDoor)
            {
                messenger.GameArchitecture.Initialize(2);
                State = DoorStates.Open;
            }
        }
        public override void RandomizeVelocity()
        {
            Velocity = new();
        }
    }
}
