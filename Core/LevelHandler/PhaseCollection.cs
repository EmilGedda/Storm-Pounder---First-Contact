using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storm_Pounder___First_Contact.Core.Event;

namespace Storm_Pounder___First_Contact.Core.LevelHandler
{
	class PhaseCollection
	{
		public List<Phase> Phases { get; set; }
		public Phase CurrentPhase { get { return Phases[index]; } }
		public event EventHandler<GameEventArgs> Ending;

		protected virtual void OnEnding(GameEventArgs e)
		{
			EventHandler<GameEventArgs> handler = Ending;
			if (handler != null) handler(this, e);
		}

		public float LifeTime
		{
			get { return Phases.Sum(x => x.LifeTime); }
		}
		public Phase Last
		{
			get { return Phases[Phases.Count - 1]; }
		}

		public Phase this[int number]
		{
			get { return Phases[number]; }
			set { Phases[number] = value; }
		}
		private int index = 0;

		public PhaseCollection()
		{
			Phases = new List<Phase>();
		}
		public PhaseCollection(int phases)
		{
			Phases = new List<Phase>(phases);
			for (int i = 0; i < Phases.Count; i++)
			{
				Phase phase = Phases[i];
				phase.End += (sender, args) => index++;
				if (i == Phases.Count - 1)
					phase.End += (sender, args) => OnEnding(new GameEventArgs(GameCore.Time));
			}
		}

		public void AddPhase(Phase p)
		{
			Phases.Add(p);
		}
		public int Update(GameWindow window, GameTime gameTime)
		{
			return CurrentPhase.Update(window, gameTime);
		}

		public void Draw(SpriteBatch sb, GameTime gameTime)
		{
			CurrentPhase.Draw(sb, gameTime);
		}

	}
}
