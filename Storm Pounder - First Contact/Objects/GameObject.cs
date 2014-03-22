using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;

        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }
        public float Width { get { return texture.Width; } }
        public float Height { get { return texture.Height; } }

        public GameObject(Texture2D t, float x, float y)
        {
            this.texture = t;
            this.position.X = x;
            this.position.Y = y;
        }

        public virtual void Draw(SpriteBatch sb, float opacity = 1F, float rotation = 0)
        {
            sb.Draw(texture, position, null, Color.White * opacity, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //sb.Draw(texture, position, Color.White * opacity);
            //((position.Y < 25) ? (position.Y / 25): opacity)
        }
        public virtual void Draw(SpriteBatch sb, Color color, float opacity = 1F, float rotation = 0)
        {
            sb.Draw(texture, position, null, color * opacity, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //sb.Draw(texture, position, Color.White * opacity);
            //((position.Y < 25) ? (position.Y / 25): opacity)
        }

    }
}