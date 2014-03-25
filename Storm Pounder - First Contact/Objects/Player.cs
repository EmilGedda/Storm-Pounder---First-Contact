using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Storm_Pounder___First_Contact
{
    class Player : PhysicalObject
    {
        double LastBulletTime = 0;
        private const int pacifism = 200;
        public int Points { get; set; }
        public bool Pause { get; set; }
        private List<Projectile> bullets;
        private Texture2D bulletTexture;
        private SoundEffect shot;
        public bool IsInvincible { get; set; }
        public new bool IsAlive { get { return base.IsAlive; } set { base.IsAlive = IsInvincible ||  value; }}

        public IEnumerable<Projectile> Bullets { get { return bullets; } }

        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture, SoundEffect shot)
            : base(texture, X, Y, speedX, speedY)
        {
            Points = 0;
            this.shot = shot;
            bullets = new List<Projectile>();
            this.bulletTexture = bulletTexture;

        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            #region movement
            KeyboardState kState = Keyboard.GetState();
            if (position.X <= window.ClientBounds.Width - texture.Width && position.X >= 0)
            {
                if (kState.IsKeyDown(Keys.Right))
                    position.X += speed.X;
                if (kState.IsKeyDown(Keys.Left))
                    position.X -= speed.X;
            }
            if (position.Y <= window.ClientBounds.Height - texture.Height && position.Y >= -1)
            {
                if (kState.IsKeyDown(Keys.Down))
                    position.Y += speed.Y;
                if (kState.IsKeyDown(Keys.Up))
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

            Pause = kState.IsKeyDown(Keys.Escape);
            if (kState.IsKeyDown(Keys.Space))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > LastBulletTime + pacifism)
                { //
                    Projectile bRight = new Projectile(bulletTexture, Center.X  - (bulletTexture.Width / 2) + 24, position.Y + texture.Height - 24);
                    Projectile bLeft = new Projectile(bulletTexture, Center.X - (bulletTexture.Width / 2) - 24, position.Y + texture.Height - 24);
                    bullets.Add(bLeft);
                    bullets.Add(bRight);
                    LastBulletTime = gameTime.TotalGameTime.TotalMilliseconds;
                    shot.Play(0.15F, 0F, 0F);
                }
            }
            foreach (Projectile b in bullets.ToList())
            {
                b.Update();
                if (!b.IsAlive)
                    bullets.Remove(b);
            }
            #endregion
            base.Update();
        }

        public void Reset(float X, float Y, float speedX, float speedY)
        {
            
        }
        public override void Draw(SpriteBatch sb)
        {
            foreach (Projectile b in bullets)
                b.Draw(sb, opacity:b.Y < 150 ? (b.Y+50)/ 200 : 1); //opacity:(b.Y < 150 ? (b.Y+50)/ 200 : 1)
            
            base.Draw(sb);

        }
    }
}
