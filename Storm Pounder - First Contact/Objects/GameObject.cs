using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class GameObject
    {
        protected Animation animation;
        protected AnimationPlayer animationPlayer;
        protected Vector2 position;

        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + Width/2, position.Y + Height/2);
            }
        }

        public Vector2 Position { get { return position; } }
        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }
        public float Width { get { return animation.FrameWidth; } }
        public float Height { get { return animation.FrameHeight; } }

        public GameObject(Animation a, float x, float y)
        {
            animation = a;
            position.X = x;
            position.Y = y;
            animationPlayer.PlayAnimation(a);
        }

        public virtual void Draw(SpriteBatch sb, GameTime gameTime, SpriteEffects spriteEffect, Color? color = null, float opacity = 1F, float rotation = 0)
        {
            animationPlayer.Draw(gameTime, sb, position, spriteEffect);
            //sb.Draw(texture, position, null, color.HasValue ? color.Value : Color.White * opacity, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //sb.Draw(texture, position, Color.White * opacity);
            //((position.Y < 25) ? (position.Y / 25): opacity)
        }
        public virtual void Draw(SpriteBatch sb, GameTime gameTime)
        {
            Draw(sb,gameTime, SpriteEffects.None, rotation: 0);
        }

    }
}