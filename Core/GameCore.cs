
using System;
using System.Collections.Generic;
using System.Globalization;
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
		private static int totalEnemies;
		private static Texture2D heart;
		private static bool shrinking;
		private static float heartTime = 1;
		private static AnimationPlayer heartPlayer;
		private static Vector2 t;
		public static void Initialize(GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
		{
			Enemy.StandardTexture = content.Load<Texture2D>(EnemyTexture);
			Enemy.Destruction = content.Load<SoundEffect>("sounds/Explosion3");
			CurrentState = State.Menu;
			enemies = new List<Enemy>();
			graphicsDevice = graphics;
			player = new Player(new Animation(content.Load<Texture2D>(PlayerTexture), 1F, false, 64), new Vector2(window.ClientBounds.Height - 96, window.ClientBounds.Width / 2), new Vector2(SpeedX * 2, SpeedY * 2), content.Load<Texture2D>("images/lazer"), content.Load<SoundEffect>("sounds/shoot"));
			PhysicalObject.DeathAnimation = new Animation(content.Load<Texture2D>("images/greyExplosion"), 0.03F, true, 102);
			heart = content.Load<Texture2D>("images/hud/heart");
			shrinking = true;
			Levels = new List<Level>(1) { Level.One(), Level.Two(), Level.Three() };
			totalEnemies = Level.One().EnemiesAlive;
		}

		public static void LoadContent(ContentManager content, GameWindow window)
		{
			hud = content.Load<Texture2D>("images/hud/HUD");
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
			t = Stencil.Size("Nothing here yet");
			//player.IsInvincible = true;

		}
		#region Update methods

		public static State RunUpdate(GameWindow window, GameTime gameTime)
		{
			FPS.Update(gameTime);
			State s = State.Menu;
			Level.LevelState levelState = Levels[currentLevel].Update(window, gameTime);
			if (shrinking)
				heartTime -= 0.02F;
			else
				heartTime += 0.02F;

			if (heartTime < 0.5)
				shrinking = false;
			else if(heartTime > 1)
				shrinking = true;
			

			switch (levelState)
			{

				case Level.LevelState.Pause:
					s = State.Menu; // TODO: Pause
					break;
				case Level.LevelState.GameOver:
					Levels[0].End(null, null);
					Levels[0] = Level.One();
					player.Reset(GameView.Width / 2, GameView.Height - 96, SpeedX * 2, SpeedY * 2);
					missed = 0;
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
			spriteBatch.Draw(hud, HUDView, Color.White);
			for (int i = 0; i < player.Health; i++)
			{
				spriteBatch.Draw(heart, new Vector2(HUDView.Width / 2 + HUDView.Left - 95 + 97 * i - heart.Width/2 * heartTime, 800 - heart.Height/2 * heartTime), null, Color.Wheat, 0, Vector2.Zero, heartTime, SpriteEffects.None, 0);
			}
			Stencil.Write(player.Points.ToString(CultureInfo.InvariantCulture), spriteBatch, HUDView.Right - 150, 50);
			Stencil.Write(Levels[currentLevel].EnemiesAlive.ToString(CultureInfo.InvariantCulture), spriteBatch, HUDView.Right - 150, 125);
			double percent = 100 * (player.Points + missed) / totalEnemies;

			Stencil.Write(string.Format("{0}%", percent), spriteBatch, HUDView.Right - 150, 200);
		}
		public static void HighScoreDraw(SpriteBatch spriteBatch)
		{
			Stencil.Write("Nothing here yet", spriteBatch, (GameView.Width + HUDView.Width) / 2 - t.X / 2, GameView.Height / 2);
		}
		public static void MenuDraw(SpriteBatch spriteBatch)
		{
			background.Draw(spriteBatch);
			menu.Draw(spriteBatch);
		}
		#endregion
	}
}
