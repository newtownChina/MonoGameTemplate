using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Timers;
using MyGame.Component;
using System.Diagnostics;

namespace MyGame
{
    public class Game1 : Game
    {
        private GameArchitecture gameArchitecture;
        public Game1()
        {
            gameArchitecture = GameArchitecture.Create(this);
        }
        protected override void Initialize()
        {
            gameArchitecture.Initialize(1);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            gameArchitecture.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            gameArchitecture.Draw();
            base.Draw(gameTime);
        }
    }
}