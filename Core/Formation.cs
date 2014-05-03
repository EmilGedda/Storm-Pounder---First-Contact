using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Storm_Pounder___First_Contact.Core
{
	class Formation
	{
		public List<Enemy> Enemies { get; private set; }

		private Enemy Last
		{
			get
			{
				return Enemies[Enemies.Count - 1];
			}
		}

		private Formation()
		{
			Enemies = new List<Enemy>();
		}

		private void Add(Enemy e)
		{
			Enemies.Add(e);
		}
		public static Formation VShape(Vector2 position, float angle, int spacing, int numLayers, Enemy enemy)
		{
			Formation v = new Formation();
			v.Add(Enemy.Standard(position, enemy.Speed));
			float dY = (float)Math.Sin(angle) * spacing;
			float dX = (float)Math.Cos(angle) * spacing;
			for (int i = 1; i < numLayers; i++)
			{
				enemy = Enemy.Standard(new Vector2(position.X + i * dX, position.Y - i * dY), enemy.Speed);
				v.Add(enemy);
				enemy = Enemy.Standard(new Vector2(position.X - i * dX, v.Last.Y), enemy.Speed);
				v.Add(enemy);
			}
			return v;
		}
	}
}
