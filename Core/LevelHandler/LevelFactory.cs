using Microsoft.Xna.Framework;
using Storm_Pounder___First_Contact.Objects.Entity;

namespace Storm_Pounder___First_Contact.Core.LevelHandler
{
	public partial class Level
	{

		public static Level One()
		{
			Level one = new Level();
			Phase first = new Phase(0);
			Player player = Player.Instance;
			
			int current = 0, last = 0;
			for (int i = 1; i < 101; i++)
			{
				while (current == last)
					current = GameCore.Rng.Next(0, 4);
				var v = Formation.VShape(new Vector2(150 + 250 * current, -200 - 700 * (i - 1)), MathHelper.PiOver2 / 1.4F, 70, 5,
					Enemy.Standard(new Vector2(0, 0), new Vector2(0, -5F)));
				foreach (var e in v.Enemies)
				{
					e.Dying += (sender, args) => player.Points++;
					e.Dead += (sender, args) => first.Enemies.Remove(sender as Enemy);
					e.OutOfBounds += (sender, args) => first.Enemies.Remove(sender as Enemy);
					first.Enemies.Add(e);
				}
				last = current;
			}
			one.phases.AddPhase(first);
			return one;
		}

		public static Level Two()
		{
			Level two = new Level();
			return two;
		}

		public static Level Three()
		{
			Level three = new Level();
			return three;
		}

	}
}
