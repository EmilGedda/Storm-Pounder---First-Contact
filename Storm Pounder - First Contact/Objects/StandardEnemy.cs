using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Storm_Pounder___First_Contact
{
    class StandardEnemy : PhysicalObject
    {
        public static SoundEffect Destruction;
        public static Texture2D StandardTexture;
        
        protected static Random rng = new Random();
        public StandardEnemy(Animation a, float X, float Y, float speedX, float speedY, SoundEffect death)
            : base(a, X, Y, speedX, speedY)
        {
            Destruction = death;
            
        }
        public void Update(GameWindow Window)
        {
            position.Y -= speed.Y;
            if (position.Y > (Window.ClientBounds.Height + 80) || !IsAlive)
            {
                if (!IsAlive)
                {
                    Destruction.Play(0.2F, 1F, 0F);
                    IsAlive = true;
                }
               do
                {
                    speed.Y = (float)rng.NextDouble() * -3 - 1;
                    position.Y = rng.Next((int)-Height * 2, (int)-Height);
                    position.X = rng.Next(0, (int) (Window.ClientBounds.Width - Width));
                } while (!GameCore.enemies.Any(IsColliding));
            }
            base.Update();
        }
    }
}
