using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class BackgroundSprite
    {
        private Vector2 position;

        public float X {get { return position.X; }}
        public float Y { get { return position.Y; } }

        public Texture2D Texture { get; set; }
        public Vector2 Speed { get; set; }

        public BackgroundSprite(Texture2D t, Vector2 speed, float x, float y)
        {
            position = new Vector2(x, y);
            Speed = speed;
            Texture = t;
        }

        public void Update(GameWindow window, int nrBackgroundsX, int nrBackgroundsY)
        {
            position.Y += Speed.Y;
            position.X += Speed.X;

            if (position.Y > window.ClientBounds.Height)
                position.Y -= nrBackgroundsY * Texture.Height;
            if ((position.X + Texture.Width) < 0)
                position.X += nrBackgroundsX * Texture.Width;
            if (position.X > window.ClientBounds.Width)
                position.X -= nrBackgroundsX * Texture.Width;
        }
        public void Draw(SpriteBatch sb, Color? color = null, float opacity = 1F, float rotation = 0)
        {
            sb.Draw(Texture, position, null, color.HasValue ? color.Value : Color.White * opacity, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //sb.Draw(texture, position, Color.White * opacity);
            //((position.Y < 25) ? (position.Y / 25): opacity)
        }
    }
}