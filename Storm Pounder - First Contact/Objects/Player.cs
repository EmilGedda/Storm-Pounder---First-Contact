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
        private const int pacifism = 100;
        private int points = 0;
        public int Points { get { return points; } set { points = value; } }
        private List<Projectile> bullets;
        private Texture2D bulletTexture;
        private SoundEffect shot;

        public List<Projectile> Bullets { get { return bullets; } }

        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture, SoundEffect shot)
            : base(texture, X, Y, speedX, speedY)
        {
            this.shot = shot;
            bullets = new List<Projectile>();
            this.bulletTexture = bulletTexture;

        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            #region movement
            KeyboardState ks = Keyboard.GetState();
            if (position.X <= window.ClientBounds.Width - texture.Width && position.X >= 0)
            {
                if (ks.IsKeyDown(Keys.Right))
                    position.X += speed.X;
                if (ks.IsKeyDown(Keys.Left))
                    position.X -= speed.X;
            }
            if (position.Y <= window.ClientBounds.Height - texture.Height && position.Y >= -1)
            {
                if (ks.IsKeyDown(Keys.Down))
                    position.Y += speed.Y;
                if (ks.IsKeyDown(Keys.Up))
                    position.Y -= speed.Y;
            }
            
            if (position.X < 0)
                position.X = 0;
            if (position.X > window.ClientBounds.Width - texture.Width)
                position.X = window.ClientBounds.Width - texture.Width;
            if (position.Y < 0)
                position.Y = 0;
            if (position.Y > window.ClientBounds.Height - texture.Height)
                position.Y = window.ClientBounds.Height - texture.Height;
            #endregion

            #region actions
            if (ks.IsKeyDown(Keys.Space))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > LastBulletTime + pacifism)
                {
                    Projectile bLeft = new Projectile(bulletTexture, position.X + texture.Width, position.Y + texture.Height - 10);
                    Projectile bRight = new Projectile(bulletTexture, position.X - 8, position.Y + texture.Height - 10);
                    bullets.Add(bLeft);
                    bullets.Add(bRight);
                    LastBulletTime = gameTime.TotalGameTime.TotalMilliseconds;
                    this.shot.Play(0.3F, 0F, 0F);
                }
            }
            foreach (Projectile b in bullets.ToList())
            {
                b.Update();
                if (!b.IsAlive)
                    bullets.Remove(b);
            }
            #endregion
        }
        public override void Draw(SpriteBatch sb, float opacity = 1F, float rotation = 90F)
        {
            foreach (Projectile b in bullets)
                b.Draw(sb); //opacity:(b.Y < 150 ? (b.Y+50)/ 200 : 1)
            base.Draw(sb);

        }
    }
}
