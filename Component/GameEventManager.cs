using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using static MyGame.Component.Messenger;

namespace MyGame.Component
{
    public class GameEventManager
    {
        private KeyboardListener keyboardListener;
        private MouseListener mouseListener;
        private GamePadListener gamePadListener;
        private TouchListener touchListener;
        public void Initialize(MessengerReadOnly messenger)
        {
            KeyboardListenerSettings keyboardListenerSettings = new();
            MouseListenerSettings mouseListenerSettings = new();
            GamePadListenerSettings gamePadListenerSettings = new();
            TouchListenerSettings touchListenerSettings = new();
            keyboardListenerSettings.InitialDelayMilliseconds = 10;
            keyboardListenerSettings.RepeatPress = true;
            keyboardListenerSettings.RepeatDelayMilliseconds = 5;
            keyboardListener = new(keyboardListenerSettings);
            mouseListener = new(mouseListenerSettings);
            gamePadListener = new(gamePadListenerSettings);
            touchListener = new(touchListenerSettings);
            messenger.Game.Components.Add(new InputListenerComponent(messenger.Game, keyboardListener, mouseListener, gamePadListener, touchListener));
            mouseListener.MouseClicked += (sender, args) =>
            {
                messenger.Game.Window.Title = $"Mouse {args.Button} Clicked";
            };
            keyboardListener.KeyPressed += (sender, args) =>
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    messenger.Game.Exit();
                }
            };
            gamePadListener.ButtonDown += (sender, args) =>
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    messenger.Game.Exit();
                }
            };
        }
    }
}
