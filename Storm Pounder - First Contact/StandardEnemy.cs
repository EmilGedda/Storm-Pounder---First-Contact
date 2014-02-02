using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storm_Pounder___First_Contact
{
    class StandardEnemy : PhysicalObject
    {
        protected SoundEffect destruction;
        static Random rng = new Random();
        public StandardEnemy(Texture2D texture, float X, float Y, float speedX, float speedY, SoundEffect death)
            : base(texture, X, Y, speedX, speedY)
        {
            destruction = death;
        }
        public void Update(GameWindow Window)
        {
            position.Y -= speed.Y;
            if (position.Y > (Window.ClientBounds.Height + 80) || !IsAlive)
            {
                if (!IsAlive)
                {
                    destruction.Play(.5F, 1f, 0F);
                    IsAlive = true;
                }
                speed.Y = -1*rng.Next(10, 40)/10;
                position.Y = rng.Next(-200, -50);
                position.X = rng.Next(0,(int)(Window.ClientBounds.Width - Width));
            }
        }
        public override void Draw(SpriteBatch sb, float opacity = 1F, float rotation = 0)
        {
            sb.Draw(texture, position, null, Color.LightSalmon * opacity, 0, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0);
            //base.Draw(sb, Color.Salmon, rotation: MathHelper.Pi);
        }

    }
}
