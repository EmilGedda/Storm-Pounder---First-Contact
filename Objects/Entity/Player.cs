using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Storm_Pounder___First_Contact.Core;
using Storm_Pounder___First_Contact.Core.Event;
using Storm_Pounder___First_Contact.Objects.Entity.Basic;

namespace Storm_Pounder___First_Contact.Objects.Entity
{
    class Player : PhysicalObject
    {
	    public static Player Instance { get; private set; }
        double LastBulletTime;

        private float MaxSpeedX;
        private float MaxSpeedY;

        private const int pacifism = 200;

        public int Points { get; set; }
        public bool Pause { get; set; }

	    private Texture2D bulletTexture;
        private SoundEffect shot;

        public bool IsInvincible { get; set; }
        public new bool IsAlive { get { return base.IsAlive || IsInvincible; } }

        public new int Health { get { return base.Health; } set { base.Health = IsInvincible ? base.Health : value; } }

	    public List<Projectile> Bullets { get; private set; }

	    public Player(Animation a, Vector2 position, Vector2 maxSpeed, Texture2D bulletTexture, SoundEffect shot)
            : base(a, position, Vector2.Zero)
	    {
		    Health = 3;
	        Instance = this;
            Points = 0;
            this.shot = shot;
            Bullets = new List<Projectile>();
            this.bulletTexture = bulletTexture;
            MaxSpeedX = maxSpeed.X;
            MaxSpeedY = maxSpeed.Y;
        }

        public void Update(GameWindow window, GameTime gameTime)
        {
            #region movement
            KeyboardState kState = Keyboard.GetState();
            if (position.X <= GameCore.GameView.Width - Width && position.X >= 0)
            {
                if (kState.IsKeyDown(Keys.Right) || kState.IsKeyDown(Keys.D))
                    speed.X += 0.17F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (kState.IsKeyDown(Keys.Left) || kState.IsKeyDown(Keys.A))
                    speed.X -= 0.17F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if ((kState.IsKeyUp(Keys.Left) && kState.IsKeyUp(Keys.Right) && kState.IsKeyUp(Keys.A) &&
                    kState.IsKeyUp(Keys.D)) || ((kState.IsKeyDown(Keys.Left) || kState.IsKeyDown(Keys.A)) && (kState.IsKeyDown(Keys.Right) ||
                    kState.IsKeyDown(Keys.D))))
                    speed.X /= 1.5F;

                speed.X = MathHelper.Clamp(speed.X, -MaxSpeedX, MaxSpeedX);
            }
            if (position.Y <= GameCore.GameView.Height - Height && position.Y >= -1)
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
            if (position.X > GameCore.GameView.Width - Width)
                position.X = GameCore.GameView.Width - Width;
            if (position.Y < 0)
                position.Y = 0;
            if (position.Y > GameCore.GameView.Height - Height)
                position.Y = GameCore.GameView.Height - Height;
            #endregion

            #region actions

            Pause = kState.IsKeyDown(Keys.Escape);
            if (kState.IsKeyDown(Keys.Space))
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > LastBulletTime + pacifism)
                {
                    Projectile bRight = new Projectile(new Animation(bulletTexture, 1F, true, bulletTexture.Width), new Vector2(Center.X - (bulletTexture.Width / 2) + 24, position.Y + Height - 24));
                    Projectile bLeft = new Projectile(new Animation(bulletTexture, 1F, true, bulletTexture.Width), new Vector2(Center.X - (bulletTexture.Width / 2) - 24, position.Y + Height - 24));
                    Bullets.Add(bLeft);
                    Bullets.Add(bRight);
	                bLeft.Dead += Remove;
					bLeft.OutOfBounds += Remove;
	                bRight.Dead += Remove;
					bRight.OutOfBounds += Remove;
                    LastBulletTime = gameTime.TotalGameTime.TotalMilliseconds;
                    shot.Play(0.15F, 0F, 0F);
                }
            }
            foreach (Projectile b in Bullets.ToList())
                b.Update();
            
            #endregion
            UpdateHitBox();
        }

	    private void Remove(object sender, GameEventArgs args)
	    {
		    Bullets.Remove(sender as Projectile);
	    }
        public void Reset(float X, float Y, float speedX, float speedY)
        {
			Position = new Vector2(X, Y);
	        MaxSpeedX = speedX;
	        MaxSpeedY = speedY;
	        Points = 0;
	        Health = 3;
	        foreach (var bullet in Bullets)
	        {
		        bullet.Dead -= Remove;
		        bullet.OutOfBounds -= Remove;
	        }
			Bullets.Clear();
        }
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            foreach (Projectile b in Bullets)
                b.Draw(sb, gameTime, SpriteEffects.None, opacity: b.Y < 50 ? (b.Y + 50) / 100 : 1); //opacity:(b.Y < 150 ? (b.Y+50)/ 200 : 1)

            base.Draw(sb, gameTime);

        }
    }
}
