
using System;
using System.Collections.Generic;
using System.Linq;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Storm_Pounder___First_Contact.Core.LevelHandler;
using Storm_Pounder___First_Contact.Objects.Entity;
using Storm_Pounder___First_Contact.Objects.Entity.Basic;
using Storm_Pounder___First_Contact.Objects.UI.Background;

namespace Storm_Pounder___First_Contact.Core
{
	static class GameCore
	{

		private const string PlayerTexture = "images/aircraft";
		private const string EnemyTexture = "images/enemy";
		public static readonly Random Rng = new Random();

		public static State CurrentState;
		public enum State { Play, Menu, HighScore, Options, Quit };
		public static IServiceProvider ServiceProvider { get; set; }

		public static List<Level> Levels;
		public static int currentLevel = 0;
		public static Rectangle GameView;
		public static Rectangle HUDView;
		public static GameTime Time;
		private static Background background;
		static Player player;
		public static List<Enemy> enemies;
		private static Menu.Menu menu;

		private const float SpeedX = 4.5F;
		private const float SpeedY = 4.0F;
		private static int missed = 0;
		public static Font Stencil;
		private static Texture2D hud;
		private static Boss boss;
		private static Enemy closestEnemy;
		private static float closestLength;
		private static GraphicsDeviceManager graphicsDevice;
		public static ContentManager Content;
		private static GameWindow _window;
		public static void Initialize(GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
		{
			Enemy.StandardTexture = content.Load<Texture2D>(EnemyTexture);
			Enemy.Destruction = content.Load<SoundEffect>("sounds/Explosion3");
			CurrentState = State.Menu;
			enemies = new List<Enemy>();
			graphicsDevice = graphics;
			player = new Player(new Animation(content.Load<Texture2D>(PlayerTexture), 1F, false, 64), new Vector2(window.ClientBounds.Height - 96, window.ClientBounds.Width / 2), new Vector2(SpeedX * 2, SpeedY * 2), content.Load<Texture2D>("images/lazer"), content.Load<SoundEffect>("sounds/shoot"));
			PhysicalObject.DeathAnimation = new Animation(content.Load<Texture2D>("images/greyExplosion"), 0.03F, true, 102);

			Levels = new List<Level>(1) { Level.One(), Level.Two(), Level.Three() };
		}

		public static void LoadContent(ContentManager content, GameWindow window)
		{
			hud = content.Load<Texture2D>("images/hud/bg");
			GameView = window.ClientBounds;
			GameView.Width -= hud.Width;
			GameView.Y = 0;
			HUDView = new Rectangle(GameView.Right, 0, hud.Width, hud.Height);
			boss = new Boss(new Animation(content.Load<Texture2D>("images/boss"), 0.1F, true, 100), new Vector2(Rng.Next(window.ClientBounds.Width - 64), -1 * Rng.Next(500) - 100), new Vector2(0, (float)Rng.NextDouble() * -3 - 1),
					Enemy.Destruction);
			//enemies.Add(boss);
			Content = content;
			_window = window;
			menu = new Menu.Menu((int)State.Menu, content);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnNewGame"), (int)State.Play, window);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnHighScore"), (int)State.HighScore, window);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnOptions"), (int)State.Options, window);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnQuit"), (int)State.Quit, window);

			background = new Background(content, "images/background/endless/", window);

			content.Load<SoundEffect>("sounds/lets_go").Play();

			Stencil = new Font(content.Load<SpriteFont>("fonts/Stencil"));
			//player.IsInvincible = true;

		}
		#region Update methods

		public static State RunUpdate(GameWindow window, GameTime gameTime)
		{
			FPS.Update(gameTime);
			State s = State.Menu;
			Level.LevelState levelState = Levels[currentLevel].Update(window, gameTime);

			switch (levelState)
			{

				case Level.LevelState.Pause:
					s = State.Menu; // TODO: Pause
					break;
				case Level.LevelState.GameOver:
					s = State.Menu; // TODO: Game over
					break;
				case Level.LevelState.End: // TODO: Level complete
					currentLevel++;
					break;
				case Level.LevelState.Active:
					s = State.Play;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return s;

		}

		public static State MenuUpdate(GameTime gameTime)
		{
			player.Health = 3;
			return (State)menu.Update(gameTime);
		}
		public static State HighscoreUpdate()
		{
			KeyboardState kState = Keyboard.GetState();
			return kState.IsKeyDown(Keys.Escape) ? State.Menu : State.HighScore;
		}
		public static State OptionsUpdate()
		{
			background = new Background(Content, "images/background/endless/", _window);
			return State.Menu;
		}
		#endregion

		#region Drawing methods
		public static void RunDraw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Levels[currentLevel].Draw(spriteBatch, gameTime);

			Stencil.Write(String.Format("Score: {0}\nEnemies: {1}\nFPS: {2}\nLives: {3}\nMissed: {4}", player.Points,
				Levels[currentLevel].EnemiesAlive,
				FPS.ToString(),
				player.Health,
				Levels[currentLevel].Missed),
				spriteBatch,
				20,
				10
			);

			spriteBatch.Draw(hud, HUDView, Color.White);
		}
		public static void HighScoreDraw(SpriteBatch spriteBatch)
		{

		}
		public static void MenuDraw(SpriteBatch spriteBatch)
		{
			background.Draw(spriteBatch);
			menu.Draw(spriteBatch);
		}
		#endregion
	}
}
