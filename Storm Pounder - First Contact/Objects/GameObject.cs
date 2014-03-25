﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;

        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + texture.Width/2, position.Y + texture.Height/2);
            }
        }

        public Vector2 Position { get { return position; } }
        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }
        public float Width { get { return texture.Width; } }
        public float Height { get { return texture.Height; } }

        public GameObject(Texture2D t, float x, float y)
        {
            texture = t;
            position.X = x;
            position.Y = y;
        }

        public virtual void Draw(SpriteBatch sb, Color? color = null, float opacity = 1F, float rotation = 0)
        {
            sb.Draw(texture, position, null, color.HasValue ? color.Value : Color.White * opacity, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //sb.Draw(texture, position, Color.White * opacity);
            //((position.Y < 25) ? (position.Y / 25): opacity)
        }
        public virtual void Draw(SpriteBatch sb)
        {
            Draw(sb, rotation: 0);
        }

    }
}