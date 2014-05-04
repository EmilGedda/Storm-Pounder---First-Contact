using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storm_Pounder___First_Contact.Core.Event;
using Storm_Pounder___First_Contact.Objects.Entity;

namespace Storm_Pounder___First_Contact.Core.LevelHandler
{
	class Phase
	{
		public event EventHandler<GameEventArgs> End;

		public float LifeTime { get; private set; }
		public List<Enemy> Enemies { get; private set; }
		private Player player;

		private float closestLength;
		private Enemy closestEnemy;

		public Phase(float timeLength)
		{
			player = Player.Instance;
			LifeTime = timeLength;
			End = null;
			Enemies = new List<Enemy>();
		}

		public int Update(GameWindow window, GameTime gameTime)
		{
			closestLength = 100000;
			closestEnemy = null;
			foreach (Enemy enemy in Enemies.ToArray())
			{
				if ((enemy.Center - player.Center).Length() < closestLength && enemy.Y < player.Y && !enemy.IsDying)
				{
					closestLength = (enemy.Position - player.Position).Length();
					closestEnemy = enemy;
				}
				foreach (Projectile bullet in player.Bullets)
					if (enemy.IsColliding(bullet) && bullet.Y > 0 && enemy.IsAlive && bullet.IsAlive)
					{
						enemy.Health--;
						bullet.Health--;
					}

				if (enemy.IsAlive && enemy.IsColliding(player))
				{
					player.Health--;
					enemy.Health--;
				}
				enemy.Update(window);
			}
			return !player.IsAlive || player.Pause ? (int) Level.LevelState.Pause : (int) Level.LevelState.Active;
		}

		public void Draw(SpriteBatch sb, GameTime gameTime)
		{
			foreach (Enemy e in Enemies)
			{
				e.Draw(sb, gameTime);
				if (e != closestEnemy) continue;
				sb.DrawRectangle(e.HitBox, Color.Red);
/*
				Stencil.Write("D: " + Math.Round((e.Center - player.Center).Length(), 1, MidpointRounding.AwayFromZero), sb, e.X + 4, e.Y + e.Height - 3, 0.5f);
				Stencil.Write("A: " + Math.Round(MathHelper.ToDegrees(player.Center.Angle(e.Center)), 1, MidpointRounding.AwayFromZero), sb, e.X + 4, e.Y + e.Height + 9, 0.5f);
				Stencil.Write("L: " + e.Health, sb, e.X + 4, e.Y + e.Height + 21, 0.5F);
*/
				if (e.Center.Y > 0)
					sb.DrawLine(e.Center, player.Center, Color.Red);
			}

		}
		private void OnEnd(GameEventArgs e)
		{
			EventHandler<GameEventArgs> handler = End;
			if (handler != null) handler(this, e);
		}

	}
}
