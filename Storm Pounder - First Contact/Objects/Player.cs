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
        double LastBulletTime;

        private float MaxSpeedX;
        private float MaxSpeedY;

        private const int pacifism = 200;

        public int Points { get; set; }
        public bool Pause { get; set; }

        private List<Projectile> bullets;
        private Texture2D bulletTexture;
        private SoundEffect shot;

        public bool IsInvincible { get; set; }
        public new bool IsAlive { get { return base.IsAlive; } set { base.IsAlive = IsInvincible || value; } }

        public IEnumerable<Projectile> Bullets { get { return bullets; } }

        public Player(Animation a, float X, float Y, float maxSpeedX, float maxSpeedY, Texture2D bulletTexture, SoundEffect shot)
            : base(a, X, Y, 0, 0)
        {
            Points = 0;
            this.shot = shot;
            bullets = new List<Projectile>();
            this.bulletTexture = bulletTexture;
            MaxSpeedX = maxSpeedX;
            MaxSpeedY = maxSpeedY;
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            #region movement
            KeyboardState kState = Keyboard.GetState();
            if (position.X <= window.ClientBounds.Width - Width && position.X >= 0)
            {
                if (kState.IsKeyDown(Keys.Right) || kState.IsKeyDown(Keys.D))
                    speed.X += 0.17F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (kState.IsKeyDown(Keys.Left) || kState.IsKeyDown(Keys.A))
                    speed.X -= 0.17F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if ((kState.IsKeyUp(Keys.Left) && kState.IsKeyUp(Keys.Right) && kState.IsKeyUp(Keys.A) &&
                    kState.IsKeyUp(Keys.D)) || ((kState.IsKeyDown(Keys.Left) || kState.IsKeyDown(Keys.A)) && kState.IsKeyDown(Keys.Right) ||
                    kState.IsKeyDown(Keys.D)))
                    speed.X /= 1.3F;

                speed.X = MathHelper.Clamp(speed.X, -MaxSpeedX, MaxSpeedX);
            }
            if (position.Y <= window.ClientBounds.Height - Height && position.Y >= -1)
            {
                if (kState.IsKeyDown(Keys.Down) || kState.IsKeyDown(Keys.S))
                    speed.Y += 0.17F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (kState.IsKeyDown(Keys.Up) || kState.IsKeyDown(Keys.W))
                    speed.Y -= 0.17F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if ((kState.IsKeyUp(Keys.Up) && kState.IsKeyUp(Keys.Down) && kState.IsKeyUp(Keys.W) &&
                     kState.IsKeyUp(Keys.S)) ||
                    ((kState.IsKeyDown(Keys.Up) || kState.IsKeyDown(Keys.W)) && kState.IsKeyDown(Keys.Down) ||
                     kState.IsKeyDown(Keys.S)))
                    speed.Y /= 1.3F;
                speed.Y = MathHelper.Clamp(speed.Y, -MaxSpeedY, MaxSpeedY);
            }
            position += speed;
            if (position.X < 0)
                position.X = 0;
            if (position.X > window.ClientBounds.Width - Width)
                position.X = window.ClientBounds.Width - Width;
            if (position.Y < 0)
                position.Y = 0;
            if (position.Y > window.ClientBounds.Height - Height)
                position.Y = window.ClientBounds.Height - Height;
            #endregion

            #region actions

            Pause = kState.IsKeyDown(Keys.Escape);
            if (kState.IsKeyDown(Keys.Space))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > LastBulletTime + pacifism)
                { 
                    Projectile bRight = new Projectile(new Animation(bulletTexture, 1F, true, bulletTexture.Width), Center.X - (bulletTexture.Width / 2) + 24, position.Y + Height - 24);
                    Projectile bLeft = new Projectile(new Animation(bulletTexture, 1F, true, bulletTexture.Width), Center.X - (bulletTexture.Width / 2) - 24, position.Y + Height - 24);
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
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            foreach (Projectile b in bullets)
                b.Draw(sb, gameTime, SpriteEffects.None, opacity: b.Y < 50 ? (b.Y + 50) / 100 : 1); //opacity:(b.Y < 150 ? (b.Y+50)/ 200 : 1)

            base.Draw(sb, gameTime);

        }
    }
}
