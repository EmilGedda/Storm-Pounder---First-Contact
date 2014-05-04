using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Storm_Pounder___First_Contact.Core;
using Storm_Pounder___First_Contact.Core.Event;
using Storm_Pounder___First_Contact.Objects.Entity.Basic;

namespace Storm_Pounder___First_Contact.Objects.Entity
{
	class Enemy : PhysicalObject
	{
		public static SoundEffect Destruction { get; set; }
		private SoundEffect destruction { get; set; }
		public static Texture2D StandardTexture;
		public delegate Vector2 VectorUpdate(Enemy sender);
		private EventHandler<GameEventArgs> playDeathSound = (sender, args) => ((Enemy)sender).destruction.Play(0.2F, 1F, 0F);
		public VectorUpdate UpdatePosition;
		public VectorUpdate UpdateSpeed;
		protected int DefaultLives = 1;

		protected static readonly Random rng = new Random();

		public Enemy(Animation a, Vector2 position, Vector2 speed, SoundEffect death, VectorUpdate p, VectorUpdate s)//, VectorUpdate updatePosition, VectorUpdate updateSpeed)
			: base(a, position, speed)
		{
			deathPlayer.PlayAnimation(DeathAnimation);
			destruction = death;
			Health = DefaultLives;

			Dying += playDeathSound;
			UpdatePosition += p;
			UpdateSpeed += s;
			OutOfBounds += (sender, args) => Dying -= playDeathSound;

			OnSpawn(new GameEventArgs(GameCore.Time));
		}
		public void Update(GameWindow window)
		{
			if (UpdateSpeed != null)
				speed = UpdateSpeed(this);
			if (UpdatePosition != null)
			{
				position = UpdatePosition(this);
				OnMove(new GameEventArgs(GameCore.Time));
			}
			if (Y > GameCore.GameView.Bottom + 20 || X < GameCore.GameView.Left - 20 || X > GameCore.GameView.Right + 20)
				OnOutOfBounds(new GameEventArgs(GameCore.Time));
			if (!IsAlive)
			{
				if (!IsDying)
					OnDying(new GameEventArgs(GameCore.Time));
				else if (deathPlayer.FrameIndex + 2 > DeathAnimation.FrameCount)
					OnDead(new GameEventArgs(GameCore.Time));
			}
			UpdateHitBox();
		}
		private void Respawn(object sender, GameEventArgs gameEventArgs)
		{
			do
			{
				speed.Y = (float)rng.NextDouble() * -3 - 1;
				position.Y = rng.Next(-Height * 2, -Height);
				position.X = rng.Next(0, GameCore.GameView.Width - Width);
			} while (!GameCore.enemies.Any(IsColliding));
			Health = DefaultLives;
			OnSpawn(new GameEventArgs(GameCore.Time));
		}

		public override void Draw(SpriteBatch sb, GameTime gameTime)
		{
			if (IsDying)
			{
				if (deathPlayer.FrameIndex < 3)
					Draw(sb, gameTime, SpriteEffects.None, opacity: 1 - deathPlayer.FrameIndex / 3);
				deathPlayer.Draw(gameTime, sb, Center, SpriteEffects.None);
			}
			if (IsAlive)
				base.Draw(sb, gameTime);
		}

		public static Enemy Standard(Vector2 position, Vector2 speed)
		{
			return Standard(position, speed, (sender) => sender.Position - sender.Speed);
		}
		public static Enemy Standard(Vector2 position, Vector2 speed, VectorUpdate positionUpdate)
		{
			Enemy e = new Enemy(new Animation(StandardTexture, 1F, true, StandardTexture.Width), position, speed, Destruction, positionUpdate, (sender) => sender.IsAlive ? sender.Speed : Vector2.Zero);
			return e;
		}
	}
}
