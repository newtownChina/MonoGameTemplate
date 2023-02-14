using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using static MyGame.Component.Messenger;

namespace MyGame.Component
{
    public class GameCameraManager
    {
        private MessengerReadOnly messenger;
        public OrthographicCamera MainCamera { get; set; }//全局相机
        private RectangleF[] shader;
        public void Initialize(MessengerReadOnly messenger)
        {
            this.messenger = messenger;
            MainCamera = new(new BoxingViewportAdapter(messenger.Game.Window, messenger.Game.GraphicsDevice, messenger.Game.GraphicsDevice.Viewport.Width, messenger.Game.GraphicsDevice.Viewport.Height));
            shader = new RectangleF[4];
            shader[0] = new(); shader[1] = new(); shader[2] = new(); shader[3] = new();
            shader[0].X = 0;
            shader[0].Y = 0;
            shader[0].Width = messenger.Game.GraphicsDevice.Viewport.Width;
            shader[1].Height = messenger.GameConfig.ShaderViewArea.Height;
            shader[2].X = 0;
            shader[2].Width = messenger.Game.GraphicsDevice.Viewport.Width;
            shader[3].X = 0;
            shader[3].Height = messenger.GameConfig.ShaderViewArea.Height;
        }
        public void Update(GameTime gameTime)
        {
            if (messenger.GameSpriteManager.EntityPlayer != null) 
            {
                MainCamera.LookAt(MainCamera.Origin - (Vector2)messenger.GameSpriteManager.EntityPlayer.PlayerTransform.position);
            }
        }
        public void Draw()
        {
            DrawPlayerView();
            if (messenger.GameConfig.ShaderEnabled) 
            {
                Drawshader();
            }
        }
        //玩家视野
        private void DrawPlayerView() 
        {
            messenger.SpriteBatch.Begin(transformMatrix: MainCamera.GetViewMatrix());
            messenger.SpriteBatch.DrawRectangle(messenger.GameConfig.PlayerViewArea, Color.Black, 4f);
            messenger.SpriteBatch.End();
        }
        //玩家遮罩
        private void Drawshader()
        {
            messenger.SpriteBatch.Begin();
            //上
            shader[0].Height = messenger.GameSpriteManager.EntityPlayer.PlayerTransform.position.Y - (messenger.GameConfig.ShaderViewArea.Height - messenger.GameConfig.PlayerHeight) / 2;
            //右
            shader[1].X = messenger.GameSpriteManager.EntityPlayer.PlayerTransform.position.X + messenger.GameConfig.PlayerWidth + (messenger.GameConfig.ShaderViewArea.Width - messenger.GameConfig.PlayerWidth) / 2;
            shader[1].Y = shader[0].Height;
            shader[1].Width = messenger.Game.GraphicsDevice.Viewport.Width - shader[1].X;
            //下
            shader[2].Y = shader[1].Y + messenger.GameConfig.ShaderViewArea.Height;
            shader[2].Height = messenger.Game.GraphicsDevice.Viewport.Height - shader[2].Y;
            //左
            shader[3].Y = shader[1].Y;
            shader[3].Width = messenger.GameSpriteManager.EntityPlayer.PlayerTransform.position.X - (messenger.GameConfig.ShaderViewArea.Width - messenger.GameConfig.PlayerWidth) / 2;

            messenger.SpriteBatch.FillRectangle(shader[0], Color.Black);
            messenger.SpriteBatch.FillRectangle(shader[1], Color.Black);
            messenger.SpriteBatch.FillRectangle(shader[2], Color.Black);
            messenger.SpriteBatch.FillRectangle(shader[3], Color.Black);
            messenger.SpriteBatch.End();
        }
    }
}
