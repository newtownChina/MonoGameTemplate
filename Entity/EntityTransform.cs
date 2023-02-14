using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Entity
{
    public class EntityTransform
    {
        public Point2 position = new();
        public State state = new();
    }
    public class State
    {
        public bool movingUp = false;
        public bool movingDown = false;

        public bool movingLeft = false;
        public bool movingRight = false;

        public bool movingLeftJump = false;
        public bool movingRightJump = false;

        public bool movingLeftIdle = false;
        public bool movingRightIdle = false;

        public bool collisionWithTop = false;
        public bool collisionWithBottom = false;

        public bool collisionWithLeft = false;
        public bool collisionWithRight = false;

        public void InitializeX()
        {
            movingLeft = false;
            movingRight = false;
        }
        public void InitializeY()
        {
            movingUp = false;
            movingDown = false;
        }
        public void InitializeJump()
        {
            movingLeftJump = false;
            movingRightJump = false;
        }
        public void InitializeIdle()
        {
            movingLeftIdle = false;
            movingRightIdle = false;
        }
        public void InitializeCollisionWithTop()
        {
            collisionWithTop = false;
        }
        public void InitializeCollisionWithBottom()
        {
            
        }
        public void InitializeCollision()
        {
            collisionWithTop = false;
            collisionWithRight = false;
            collisionWithBottom = false;
            collisionWithLeft = false;
        }
    }
}
