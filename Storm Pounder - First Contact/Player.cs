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
    class Player : PhysicalObject
    {
        double LastBulletTime = 0;
        private const int pacifism = 70;
        private int points = 0;
        public int Points { get { return points; } set { points = value; } }
        private List<Bullet> bullets;
        private Texture2D bulletTexture;
        private SoundEffect shot;

        public List<Bullet> Bullets { get { return bullets; } }

        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture, SoundEffect shot)
            : base(texture, X, Y, speedX, speedY)
        {
            this.shot = shot;
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;

        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            #region movement
            KeyboardState ks = Keyboard.GetState();
            if (vector.X <= window.ClientBounds.Width - texture.Width && vector.X >= 0)
            {
                if (ks.IsKeyDown(Keys.Right))
                    vector.X += speed.X;
                if (ks.IsKeyDown(Keys.Left))
                    vector.X -= speed.X;
            }
            if (vector.Y <= window.ClientBounds.Height - texture.Height && vector.Y >= -1)
            {
                if (ks.IsKeyDown(Keys.Down))
                    vector.Y += speed.Y;
                if (ks.IsKeyDown(Keys.Up))
                    vector.Y -= speed.Y;
            }
            
            if (vector.X < 0)
                vector.X = 0;
            if (vector.X > window.ClientBounds.Width - texture.Width)
                vector.X = window.ClientBounds.Width - texture.Width;
            if (vector.Y < 0)
                vector.Y = 0;
            if (vector.Y > window.ClientBounds.Height - texture.Height)
                vector.Y = window.ClientBounds.Height - texture.Height;
            #endregion

            #region actions
            if (ks.IsKeyDown(Keys.Space))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > LastBulletTime + pacifism)
                {
                    Bullet bLeft = new Bullet(bulletTexture, vector.X + texture.Width, vector.Y + texture.Height - 10);
                    Bullet bRight = new Bullet(bulletTexture, vector.X - 8, vector.Y + texture.Height - 10);
                    bullets.Add(bLeft);
                    bullets.Add(bRight);
                    LastBulletTime = gameTime.TotalGameTime.TotalMilliseconds;
                    this.shot.Play(0.3F, 0F, 0F);
                }
            }
            foreach (Bullet b in bullets.ToList())
            {
                b.Update();
                if (!b.IsAlive)
                    bullets.Remove(b);
            }
            #endregion
        }
        public override void Draw(SpriteBatch sb, float opacity = 1F)
        {
            foreach (Bullet b in bullets)
                b.Draw(sb, (b.Y < 150 ? (b.Y+50)/ 200 : 1));
            sb.Draw(texture, vector, Color.White);
        }
    }
}
