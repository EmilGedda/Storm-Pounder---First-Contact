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
        static Random rng = new Random();
        public StandardEnemy(Texture2D texture, float X, float Y, float speedX, float speedY, SoundEffect death)
            : base(texture, X, Y, speedX, speedY)
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
                    position.Y = rng.Next(-200, -50);
                    position.X = rng.Next(0, (int) (Window.ClientBounds.Width - Width));
                } while (!GameCore.enemies.Any(IsColliding));
            }
            base.Update();
        }
        public override void Draw(SpriteBatch sb, Color? color = null, float opacity = 1F, float rotation = 0)
        {
            sb.Draw(texture, position, null, (color ?? Color.White) * opacity, 0, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0);
            //base.Draw(sb, Color.Salmon, rotation: MathHelper.Pi);
        }

    }
}
