using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact.Objects.UI.Background
{
    class BackgroundSprite : BasicUIElement
    {
        public Vector2 Speed { get; set; }

        public BackgroundSprite(Texture2D t, Vector2 speed, Vector2 position) : base(t, position)
        {
            Speed = speed;
        }

        public override void Update(GameWindow window, int nrBackgroundsX, int nrBackgroundsY)
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
    }
}