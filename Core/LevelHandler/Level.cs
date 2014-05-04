using System;
using System.Collections.Generic;
using System.Linq;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Storm_Pounder___First_Contact.Core.Event;
using Storm_Pounder___First_Contact.Objects.Entity;
using Storm_Pounder___First_Contact.Objects.UI.Background;

namespace Storm_Pounder___First_Contact.Core.LevelHandler
{
	public partial class Level
	{
		public enum LevelState
		{
			Active,
			Pause,
			GameOver,
			End
		};
		public bool Started { get; set; }
		public int Missed { get; set; }
		private LevelState currentState;
		private Background background;
		private Player player;
		private readonly PhaseCollection phases;
		private ContentManager contentManager;

		public int EnemiesAlive
		{
			get { return phases.CurrentPhase.Enemies.Count(x => x.IsAlive); }
		}
		private Level(int phases)
		{
			player = Player.Instance;
			this.phases = new PhaseCollection(phases);
		}

		private Level()
		{
			player = Player.Instance;
			phases = new PhaseCollection();
			Ending += (sender, args) => currentState = LevelState.End;
		}

		public event EventHandler<GameEventArgs> Starting;
		public event EventHandler<GameEventArgs> Ending;

		public void Start(GameWindow window)
		{
			if (Started) return;
			Started = true;
			LoadContent(window);
			OnStarting(new GameEventArgs(GameCore.Time));
			phases.Ending += End;

			foreach (var e in phases.Phases.SelectMany(phase => phase.Enemies))
				e.OutOfBounds +=  MissedEnemy;

		}

		private void MissedEnemy(object sender, GameEventArgs args)
		{
			Missed++;
		}
		public void End(object sender, GameEventArgs args)
		{
			UnloadContent();
			OnEnding(new GameEventArgs(GameCore.Time));
		}

		private void LoadContent(GameWindow window)
		{
			contentManager = new ContentManager(GameCore.ServiceProvider, "Content");
			background = new Background(contentManager, "images/background/endless/", window);
		}

		private void UnloadContent()
		{
			contentManager.Unload();
			phases.Ending -= End;
			foreach (var e in phases.Phases.SelectMany(phase => phase.Enemies))
				e.OutOfBounds -= MissedEnemy;
			foreach (Phase phase in phases.Phases)
				phase.Enemies.Clear();
			Started = false;
		}
		protected virtual void OnStarting(GameEventArgs e)
		{
			EventHandler<GameEventArgs> handler = Starting;
			if (handler != null) handler(this, e);
		}
		protected virtual void OnEnding(GameEventArgs e)
		{
			EventHandler<GameEventArgs> handler = Ending;
			if (handler != null) handler(this, e);
		}

		public LevelState Update(GameWindow window, GameTime gameTime)
		{
			background.Update(window);
			Player.Instance.Update(window, gameTime);
			return (LevelState) phases.Update(window, gameTime);
		}

		public void Draw(SpriteBatch sb, GameTime gameTime)
		{
			background.Draw(sb);
			sb.DrawRectangle(player.HitBox, Color.Red);
			phases.Draw(sb, gameTime);
			player.Draw(sb, gameTime);
		}
	}
}