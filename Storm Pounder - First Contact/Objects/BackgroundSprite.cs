using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class BackgroundSprite : GameObject
    {
        public Vector2 Speed { get; set; }

        public BackgroundSprite(Texture2D texture, Vector2 speed, float x, float y)
            : base(texture, x, y)
        {
            Speed = speed;
        }

        public void Update(GameWindow window, int nrBackgroundsX, int nrBackgroundsY)
        {
            position.Y += Speed.Y;
            position.X += Speed.X;

            if (position.Y > window.ClientBounds.Height)
                position.Y -= nrBackgroundsY * texture.Height;
            if ((position.X + texture.Width) < 0)
                position.X += nrBackgroundsX * texture.Width;
            if (position.X > window.ClientBounds.Width)
                position.X -= nrBackgroundsX * texture.Width;
        }
    }
}