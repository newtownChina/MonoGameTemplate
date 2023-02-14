using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;


namespace MyGame.Entity
{
    public class EntityPolygonTile : EntityPolygon
    {
        public EntityPolygonTile(Point2 position, Polygon polygon) : base(position,polygon)
        {
    
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawPolygon(Bounds.Position, polygon, Color.Red, 1);
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Green, 1);
        }
    }
}
