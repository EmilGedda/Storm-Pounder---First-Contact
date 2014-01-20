using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storm_Pounder___First_Contact
{
    class GameObject
    {
        protected Texture2D texture;
        protected Vector2 vector;

        public float X { get { return vector.X; } }
        public float Y { get { return vector.Y; } }
        public float Width { get { return texture.Width; } }
        public float Height { get { return texture.Height; } }

        public GameObject(Texture2D t, float x, float y)
        {
            this.texture = t;
            this.vector.X = x;
            this.vector.Y = y;
        }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, vector, Color.White);
        }

    }
}