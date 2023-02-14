using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using static MyGame.Component.Messenger;

namespace MyGame.Component
{
    public class GamePhysicsManager
    {
        private MessengerReadOnly messenger;
        public CollisionComponent CollisionComponent { get; set; }
        private int frameCounter = 0;
        private Vector2[] force;
        public enum CollisionAlgorithm
        {
            EdgePointRectangle,//扫描形状全部边界，描边逐点绘制1x1碰撞矩形
            EdgeLeftRowScanRectangle, //扫描形状最左侧边界，逐行绘制碰撞矩形，减少碰撞矩形数量
            EdgeLeftRowScanMergeRectangle//扫描形状最左侧边界，逐行绘制碰撞矩形，并进一步将Position.X相同，等宽的矩形合并成一个大矩形，进一步减少碰撞矩形数量
        }
        public void Initialize(MessengerReadOnly messenger)
        {
            this.messenger = messenger;
            RectangleF screen = new(0, 0, messenger.Game.GraphicsDevice.Viewport.Width, messenger.Game.GraphicsDevice.Viewport.Height);
            CollisionComponent = new(screen);
            force = new Vector2[4];
            force[0] = new();force[1] = new();force[2] = new();force[3] = new();
        }
        public void Update(GameTime gameTime)
        {
            frameCounter++;
            if (CollisionComponent != null && CollisionComponent.IsEnabled) 
            {
                CollisionComponent.Update(gameTime);
            }
        }
        public Vector2 UpwardForce(GameTime gameTime)
        {
            force[0].Y = messenger.GameConfig.Gravity * gameTime.GetElapsedSeconds() * messenger.GameConfig.ForceUpwardMultiplier / (float)Math.Pow(1.1, frameCounter);
            return force[0];
        }
        public Vector2 RightwardForce(GameTime gameTime)
        {
            force[1].X = gameTime.GetElapsedSeconds() * messenger.GameConfig.ForceRightwardMultiplier;
            return force[1];
        }
        public Vector2 DownwardForce(GameTime gameTime) 
        {
            force[2].Y = messenger.GameConfig.Gravity * gameTime.GetElapsedSeconds() * messenger.GameConfig.ForceDownwardMultiplier * (float)Math.Pow(1.1, frameCounter);
            return force[2];
        }
        public Vector2 LeftwardForce(GameTime gameTime)
        {
            force[3].X = gameTime.GetElapsedSeconds() * messenger.GameConfig.ForceLeftwardMultiplier;
            return force[3];
        }
        public void ResetForce() 
        {
            ClearFrameCounter();
        }
        
        private void ClearFrameCounter()
        {
            frameCounter = 0;
        }
    }
}
