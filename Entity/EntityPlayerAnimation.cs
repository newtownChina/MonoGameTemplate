using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Timers;
using MyGame.Component;
using MyGame.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static MyGame.Component.Messenger;

namespace MyGame.Entity
{
    public class EntityPlayerAnimation
    {
        private MessengerReadOnly messenger;
        private AnimatedSprite animatedSprite;
        private EntityTransform playerTransform;
        private AnimationState animationState;

        private float jumpHeightLimit = 100f;
        private float jumpHeight = 0f;
        private Vector2 jumpStartPosition;//起跳位置

        private bool keyUp;
        private bool keyDown;
        private bool keyLeft;
        private bool keyRight;
        public enum AnimationState
        {
            MovingLeftIdle,
            MovingLeft,
            MovingLeftJump,
            MovingRightIdle,
            MovingRight,
            MovingRightJump
        }
        public AnimatedSprite Initialize(MessengerReadOnly messenger, string atlasName, string atlasTextureName, int regionWidth, int regionHeight, int maxRegionCount, int margin, int spacing, float duration)
        {
           
            this.messenger = messenger;
            playerTransform = messenger.GameSpriteManager.EntityPlayer.PlayerTransform;
            animationState = AnimationState.MovingRightIdle;
            SpriteSheet spriteSheet = new()
            {
                TextureAtlas = TextureAtlas.Create(atlasName, messenger.Game.Content.Load<Texture2D>(atlasTextureName), regionWidth, regionHeight, maxRegionCount, margin, spacing)
            };
            SpriteSheetAnimationCycle movingLeftCycles = new();
            SpriteSheetAnimationCycle movingLeftIdleCycles = new();
            SpriteSheetAnimationCycle movingLeftJumpCycles = new();
            SpriteSheetAnimationCycle movingRightCycles = new();
            SpriteSheetAnimationCycle movingRightIdleCycles = new();
            SpriteSheetAnimationCycle movingRightJumpCycles = new();

            List<SpriteSheetAnimationFrame> movingLeftCyclesFrameList = new();
            List<SpriteSheetAnimationFrame> movingLeftIdleCyclesFrameList = new();
            List<SpriteSheetAnimationFrame> movingLeftJumpCyclesFrameList = new();
            List<SpriteSheetAnimationFrame> movingRightCyclesFrameList = new();
            List<SpriteSheetAnimationFrame> movingRightIdleCyclesFrameList = new();
            List<SpriteSheetAnimationFrame> movingRightJumpCyclesFrameList = new();

            SpriteSheetAnimationFrame movingLeftCyclesFrame1 = new(1, duration);
            SpriteSheetAnimationFrame movingLeftCyclesFrame2 = new(2, duration);
            movingLeftCyclesFrameList.Add(movingLeftCyclesFrame1);
            movingLeftCyclesFrameList.Add(movingLeftCyclesFrame2);
            SpriteSheetAnimationFrame movingLeftIdleCyclesFrame = new(0, duration);
            movingLeftIdleCyclesFrameList.Add(movingLeftIdleCyclesFrame);
            SpriteSheetAnimationFrame movingLeftJumpCyclesFrame = new(3, duration);
            movingLeftJumpCyclesFrameList.Add(movingLeftJumpCyclesFrame);

            SpriteSheetAnimationFrame movingRightCyclesFrame1 = new(5, duration);
            SpriteSheetAnimationFrame movingRightCyclesFrame2 = new(4, duration);
            movingRightCyclesFrameList.Add(movingRightCyclesFrame1);
            movingRightCyclesFrameList.Add(movingRightCyclesFrame2);
            SpriteSheetAnimationFrame movingRightIdleCyclesFrame = new(6, duration);
            movingRightIdleCyclesFrameList.Add(movingRightIdleCyclesFrame);
            SpriteSheetAnimationFrame movingRightJumpCyclesFrame = new(7, duration);
            movingRightJumpCyclesFrameList.Add(movingRightJumpCyclesFrame);

            movingLeftCycles.Frames = movingLeftCyclesFrameList;
            movingLeftIdleCycles.Frames = movingLeftIdleCyclesFrameList;
            movingLeftJumpCycles.Frames = movingLeftJumpCyclesFrameList;
            movingRightCycles.Frames = movingRightCyclesFrameList;
            movingRightIdleCycles.Frames = movingRightIdleCyclesFrameList;
            movingRightJumpCycles.Frames = movingRightJumpCyclesFrameList;

            Dictionary<string, SpriteSheetAnimationCycle> allDirectionSpriteSheetAnimationCycle = new()
            {
                { "MovingLeft", movingLeftCycles },
                { "MovingLeftIdle", movingLeftIdleCycles },
                { "MovingLeftJump", movingLeftJumpCycles },
                { "MovingRight", movingRightCycles },
                { "MovingRightIdle", movingRightIdleCycles },
                { "MovingRightJump", movingRightJumpCycles }
            };
            spriteSheet.Cycles = allDirectionSpriteSheetAnimationCycle;
            animatedSprite = new(spriteSheet);
            return animatedSprite;
            //GlobalGameComponentManager.gameEventManager.keyboardListener.KeyPressed += new EventHandler<KeyboardEventArgs>(KeyPressedCallBackX);
            //GlobalGameComponentManager.gameEventManager.keyboardListener.KeyPressed += new EventHandler<KeyboardEventArgs>(KeyPressedCallBackY);
            //GlobalGameComponentManager.gameEventManager.keyboardListener.KeyReleased += new EventHandler<KeyboardEventArgs>(KeyReleasedCallBack);
            //GlobalGameComponentManager.gameEventManager.keyboardListener.KeyTyped += new EventHandler<KeyboardEventArgs>(KeyTypedCallBack);
        }

        public void Update(GameTime gameTime)
        {
            keyUp = Keyboard.GetState().IsKeyDown(Keys.Up);
            keyDown = Keyboard.GetState().IsKeyDown(Keys.Down);
            keyLeft = Keyboard.GetState().IsKeyDown(Keys.Left);
            keyRight = Keyboard.GetState().IsKeyDown(Keys.Right);
            //重力
            playerTransform.position += messenger.GamePhysicsManager.DownwardForce(gameTime);
            if (playerTransform.state.collisionWithBottom)
            {
                jumpStartPosition = playerTransform.position;
                jumpHeight = 0f;
            }
            if (keyUp)
            {
                if (jumpStartPosition.Y - playerTransform.position.Y > jumpHeightLimit || jumpHeight > jumpHeightLimit) 
                {
                    playerTransform.state.collisionWithTop = true;
                }
                Jump(gameTime);
                jumpHeight += messenger.GamePhysicsManager.UpwardForce(gameTime).Y;
            }
            if (keyDown && !playerTransform.state.collisionWithBottom)
            {
                Drop(gameTime);
            }
            if (keyLeft && !playerTransform.state.collisionWithLeft)
            {
                RunLeft(gameTime);
            }
            if (keyRight && !playerTransform.state.collisionWithRight)
            {
                RunRight(gameTime);
            }
            JumpLeft(gameTime); JumpRight(gameTime); IdleLeft(gameTime); IdleRight(gameTime);
            UpdateAnimation(gameTime); PlayAnimation(animationState);
            CheckBorder(playerTransform);
        }
        public void Draw()
        {
            RectangleF player = (RectangleF)messenger.GameSpriteManager.EntityPlayer.Bounds;
            messenger.SpriteBatch.Draw(animatedSprite, playerTransform.position + new Vector2(player.Width/2, player.Height/2), 0, Vector2.One);
        }
        private void Jump(GameTime gameTime) 
        {
            playerTransform.state.InitializeY();
            playerTransform.state.InitializeCollisionWithBottom();
            playerTransform.state.movingUp = true;
            if (playerTransform.state.movingLeftIdle)
            {
                playerTransform.state.movingLeftJump = true;
            }
            if (playerTransform.state.movingRightIdle)
            {
                playerTransform.state.movingRightJump = true;
            }
            if (!playerTransform.state.collisionWithTop) 
            {
                playerTransform.position -= messenger.GamePhysicsManager.UpwardForce(gameTime);
            }
        }
        private void RunLeft(GameTime gameTime) 
        {
            playerTransform.state.InitializeX();
            playerTransform.state.InitializeIdle();
            playerTransform.state.movingLeft = true;
            if (playerTransform.position.X + messenger.GameConfig.PlayerViewArea.Left > (messenger.Game.GraphicsDevice.Viewport.Width - messenger.GameConfig.PlayerActivityArea.Width) / 2)
            {
                playerTransform.position -= messenger.GamePhysicsManager.LeftwardForce(gameTime);
            }
            else if (playerTransform.position.X + messenger.GameConfig.PlayerViewArea.Left <= (messenger.Game.GraphicsDevice.Viewport.Width - messenger.GameConfig.PlayerActivityArea.Width) / 2)
            {
                messenger.GameTiledMapManager.TiledMapOffset -= messenger.GamePhysicsManager.LeftwardForce(gameTime);
            }
        }
        private void RunRight(GameTime gameTime)
        {
            playerTransform.state.InitializeX();
            playerTransform.state.InitializeIdle();
            playerTransform.state.movingRight = true;
            if (playerTransform.position.X + messenger.GameConfig.PlayerViewArea.Right < (messenger.Game.GraphicsDevice.Viewport.Width + messenger.GameConfig.PlayerActivityArea.Width) / 2)
            {
                playerTransform.position += messenger.GamePhysicsManager.RightwardForce(gameTime);
            }
            else if (playerTransform.position.X + messenger.GameConfig.PlayerViewArea.Right >= (messenger.Game.GraphicsDevice.Viewport.Width + messenger.GameConfig.PlayerActivityArea.Width) / 2)
            {
                messenger.GameTiledMapManager.TiledMapOffset += messenger.GamePhysicsManager.RightwardForce(gameTime);
            }
        }
        private void Drop(GameTime gameTime)
        {
            playerTransform.state.InitializeY();
            playerTransform.state.movingDown = true;
            playerTransform.position += messenger.GamePhysicsManager.DownwardForce(gameTime);
        }
        private void JumpLeft(GameTime gameTime)
        {
            if (playerTransform.state.movingLeft && playerTransform.state.movingUp)
            {
                playerTransform.state.InitializeX();
                playerTransform.state.InitializeY();
                playerTransform.state.InitializeJump();
                playerTransform.state.movingLeftJump = true;
            }
        }
        private void JumpRight(GameTime gameTime)
        {
            if (playerTransform.state.movingRight && playerTransform.state.movingUp)
            {
                playerTransform.state.InitializeX();
                playerTransform.state.InitializeY();
                playerTransform.state.InitializeJump();
                playerTransform.state.movingRightJump = true;
            }
        }
        private void IdleLeft(GameTime gameTime)
        {
            if ((playerTransform.state.movingLeft || playerTransform.state.movingLeftJump) && !keyUp && !keyDown && !keyLeft && !keyRight)
            {
                playerTransform.state.InitializeX();
                playerTransform.state.InitializeY();
                playerTransform.state.InitializeJump();
                playerTransform.state.movingRightIdle = false;
                playerTransform.state.movingLeftIdle = true;
            }
        }
        private void IdleRight(GameTime gameTime)
        {
            if ((playerTransform.state.movingRight || playerTransform.state.movingRightJump) && !keyUp && !keyDown && !keyLeft && !keyRight)
            {
                playerTransform.state.InitializeX();
                playerTransform.state.InitializeY();
                playerTransform.state.InitializeJump();
                playerTransform.state.movingLeftIdle = false;
                playerTransform.state.movingRightIdle = true;
            }
        }
        private void UpdateAnimation(GameTime gameTime)
        {
            if (playerTransform.state.movingLeftIdle)
            {
                animationState = AnimationState.MovingLeftIdle;
            }
            if (playerTransform.state.movingLeft)
            {
                animationState = AnimationState.MovingLeft;
            }
            if (playerTransform.state.movingLeftJump)
            {
                animationState = AnimationState.MovingLeftJump;
            }
            if (playerTransform.state.movingRightIdle)
            {
                animationState = AnimationState.MovingRightIdle;
            }
            if (playerTransform.state.movingRight)
            {
                animationState = AnimationState.MovingRight;
            }
            if (playerTransform.state.movingRightJump)
            {
                animationState = AnimationState.MovingRightJump;
            }
            if (animatedSprite != null)
            {
                animatedSprite.Update(gameTime);
            }
        }
        private void PlayAnimation(AnimationState animationState)
        {
            if (animatedSprite != null) 
            {
                switch (animationState)
                {
                    case AnimationState.MovingLeftIdle: animatedSprite.Play("MovingLeftIdle"); break;
                    case AnimationState.MovingLeft: animatedSprite.Play("MovingLeft"); break;
                    case AnimationState.MovingLeftJump: animatedSprite.Play("MovingLeftJump"); break;
                    case AnimationState.MovingRightIdle: animatedSprite.Play("MovingRightIdle"); break;
                    case AnimationState.MovingRight: animatedSprite.Play("MovingRight"); break;
                    case AnimationState.MovingRightJump: animatedSprite.Play("MovingRightJump"); break;
                }
            }
        }
        private void CheckBorder(EntityTransform playerTransform)
        {
            RectangleF player = (RectangleF)messenger.GameSpriteManager.EntityPlayer.Bounds;
            if (playerTransform.position.X <= 0)
            {
                playerTransform.position.X = 0;
            }
            if (playerTransform.position.X >= messenger.Game.GraphicsDevice.Viewport.Width - player.Width)
            {
                playerTransform.position.X = messenger.Game.GraphicsDevice.Viewport.Width - player.Width;
            }
            if (playerTransform.position.Y <= 0)
            {
                playerTransform.position.Y = 0;
            }
            if (playerTransform.position.Y >= messenger.Game.GraphicsDevice.Viewport.Height - player.Height)
            {
                playerTransform.position.Y = messenger.Game.GraphicsDevice.Viewport.Height - player.Height;
            }
        }
    }
}
