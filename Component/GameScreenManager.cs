using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MyGame.Screen;
using static MyGame.Component.Messenger;

namespace MyGame.Component
{
    public class GameScreenManager
    {
        private MessengerReadOnly messenger;
        private ScreenManager screenManager;
        private FadeTransition fadeTransition;
        public void Initialize(MessengerReadOnly messenger)
        {
            this.messenger = messenger;
            fadeTransition = new(messenger.Game.GraphicsDevice, Color.Black, 0.2f);
            screenManager = new();
            messenger.Game.Components.Add(screenManager);
        }
        public void LoadScreen(int screen)
        {
            switch (screen) 
            {
                case 1: LoadScreen1(); break;
                case 2: LoadScreen2(); break;
            }
        }
        private void LoadScreen1()
        {
            Screen1 screen1 = new(messenger.Game, messenger.GameTiledMapManager, messenger.GameSpriteManager);
            //fadeTransition里面会把上一个屏幕给卸载掉，如果这里不用fadeTransition，会调用两次LoadScreen，导致地图和精灵渲染两次
            screenManager.LoadScreen(screen1, fadeTransition);
        }
        private void LoadScreen2()
        {
            Screen2 screen2 = new(messenger.Game, messenger.GameTiledMapManager, messenger.GameSpriteManager);
            screenManager.LoadScreen(screen2, fadeTransition);
        }
    }
}
