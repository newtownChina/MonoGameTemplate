using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;
using static MyGame.Component.Messenger;

namespace MyGame.Entity
{
    public interface IEntity : ICollisionActor
    {
        public void Initialize(MessengerReadOnly messenger);
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
