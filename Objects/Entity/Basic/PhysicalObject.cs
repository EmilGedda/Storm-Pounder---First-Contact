using System;
using Microsoft.Xna.Framework;
using Storm_Pounder___First_Contact.Core.Event;

namespace Storm_Pounder___First_Contact.Objects.Entity.Basic
{
	abstract class PhysicalObject : MovingObject
	{
		public bool IsDying { get; set; }
		public bool IsDead { get { return !IsAlive && !IsDying; } }
		private Vector2 spawnPoint;
		private Rectangle hitbox;
		private int margin = 10;

		public event EventHandler<GameEventArgs> Spawn;
		public event EventHandler<GameEventArgs> OutOfBounds;

		protected virtual void OnOutOfBounds(GameEventArgs e)
		{
			EventHandler<GameEventArgs> handler = OutOfBounds;
			if (handler != null) handler(this, e);
		}

		public event EventHandler<GameEventArgs> Dying;
		public event EventHandler<GameEventArgs> Dead;
		public event EventHandler<GameEventArgs> Move;

		protected virtual void OnMove(GameEventArgs e)
		{
			EventHandler<GameEventArgs> handler = Move;
			if (handler != null) handler(this, e);
		}

		protected virtual void OnSpawn(GameEventArgs e)
		{
			EventHandler<GameEventArgs> handler = Spawn;
			if (handler != null) handler(this, e);
		}
		protected virtual void OnDying(GameEventArgs e)
		{
			IsDying = true;
			EventHandler<GameEventArgs> handler = Dying;
			if (handler != null) handler(this, e);
		}
		protected virtual void OnDead(GameEventArgs e)
		{
			IsDying = false;
			EventHandler<GameEventArgs> handler = Dead;
			if (handler != null) handler(this, e);
		}


		public static Animation DeathAnimation { get; set; }
		protected AnimationPlayer deathPlayer;

		public Vector2 SpawnPoint
		{
			get { return spawnPoint; }
			set { spawnPoint = value; }
		}

		public Rectangle HitBox
		{
			get { return hitbox; }
		}

		public bool IsAlive
		{
			get { return Health > 0; }
		}

		public int Health { get; set; }

		protected PhysicalObject(Animation a, Vector2 position, Vector2 speed)
			: base(a, position, speed)
		{
			margin = this is Projectile ? 0 : 5;
			hitbox = new Rectangle(Convert.ToInt32(X + margin), Convert.ToInt32(Y + margin), Convert.ToInt32(Width - 2 * margin), Convert.ToInt32(Height - 2 * margin));
			spawnPoint.X = X;
			spawnPoint.Y = Y;

		}
		public virtual void UpdateHitBox()
		{
			hitbox.X = (int)X + margin;
			hitbox.Y = (int)Y + margin;
		}
		public bool IsColliding(PhysicalObject victim)
		{
			return hitbox.Intersects(victim.hitbox) && PixelCollision(victim);
		}

		private bool PixelCollision(PhysicalObject victim)
		{
			IdleAnimation.Texture.GetData(0, idlePlayer.CurrentFrame, textureData, 0, textureData.Length);
			victim.IdleAnimation.Texture.GetData(0, victim.idlePlayer.CurrentFrame, victim.textureData, 0, victim.textureData.Length);

			int x1 = Math.Max(hitbox.X, victim.HitBox.X);
			int x2 = Math.Min(hitbox.X + hitbox.Width, victim.HitBox.X + victim.HitBox.Width);

			int y1 = Math.Max(hitbox.Y, victim.HitBox.Y);
			int y2 = Math.Min(hitbox.Y + hitbox.Height, victim.HitBox.Y + victim.HitBox.Height);

			for (int y = y1; y < y2; y++)
			{
				for (int x = x1; x < x2; x++)
				{
					// Get the color of both pixels at this point
					Color a = textureData[(x - hitbox.X) + (y - hitbox.Y) * Width];
					Color b = victim.textureData[(x - victim.hitbox.X) + (y - victim.hitbox.Y) * victim.Width];

					// If both pixels are not completely transparent,
					if (a.A > 127 && b.A > 127)
						return true;

				}
			}
			return false;
		}
	}
}
