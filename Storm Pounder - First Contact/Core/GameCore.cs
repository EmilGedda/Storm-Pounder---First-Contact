#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Storm_Pounder___First_Contact.Core;
using Storm_Pounder___First_Contact.Objects.UI.Background;

#endregion
namespace Storm_Pounder___First_Contact
{
	static class GameCore
	{
		private const string PlayerTexture = "images/aircraft";
		private const string EnemyTexture = "images/enemy";
		public static readonly Random Rng = new Random();
		public static State CurrentState;
		public enum State { Play, Menu, HighScore, Options, Quit };

		public static Rectangle GameView;
		public static Rectangle HUDView;
		public static GameTime Time;
		private static Background background;
		static Player player;
		public static List<Enemy> enemies;
		private static Menu menu;

		private const float SpeedX = 4.5F;
		private const float SpeedY = 4.0F;
		public static Font Stencil;
		private static Texture2D hud;
		private static Boss boss;
		private static Enemy closestEnemy;
		private static float closestLength;
		private static GraphicsDeviceManager graphicsDevice;
		public static ContentManager Content;
		private static GameWindow _window;
		public static void Initialize(GraphicsDeviceManager graphics)
		{
			CurrentState = State.Menu;
			enemies = new List<Enemy>();
			graphicsDevice = graphics;
		}

		public static void LoadContent(ContentManager content, GameWindow window)
		{
			hud = content.Load<Texture2D>("images/hud/bg");
			GameView = window.ClientBounds;
			GameView.Width -= hud.Width;
			HUDView = new Rectangle(GameView.Right, GameView.Top, hud.Width, hud.Height);
			PhysicalObject.DeathAnimation = new Animation(content.Load<Texture2D>("images/greyExplosion"), 0.03F, true, 102);
			boss = new Boss(new Animation(content.Load<Texture2D>("images/boss"), 0.1F, true, 100), new Vector2(Rng.Next(window.ClientBounds.Width - 64), -1 * Rng.Next(500) - 100), new Vector2(0, (float)Rng.NextDouble() * -3 - 1),
					Enemy.Destruction);
			// enemies.Add(boss);
			Content = content;
			_window = window;
			menu = new Menu((int)State.Menu, content);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnNewGame"), (int)State.Play, window);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnHighScore"), (int)State.HighScore, window);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnOptions"), (int)State.Options, window);
			menu.AddItem(content.Load<Texture2D>("images/menu/btnQuit"), (int)State.Quit, window);

			background = new Background(content, "images/background/endless/", window);

			content.Load<SoundEffect>("sounds/lets_go").Play();

			player = new Player(new Animation(content.Load<Texture2D>(PlayerTexture), 1F, false, 64), new Vector2(window.ClientBounds.Height - 96, window.ClientBounds.Width / 2), new Vector2(SpeedX * 2, SpeedY * 2), content.Load<Texture2D>("images/lazer"), content.Load<SoundEffect>("sounds/shoot"));

			Enemy.StandardTexture = content.Load<Texture2D>(EnemyTexture);
			Enemy.Destruction = content.Load<SoundEffect>("sounds/Explosion3");
			var v = Formation.VShape(new Vector2(500, -100), MathHelper.PiOver2 / 1.4F, 70, 5, Enemy.Standard(new Vector2(GameView.Center.X, -200), new Vector2(0, -2F)));
			foreach (var e in v.Enemies)
			{
				e.Dying += (sender, args) => player.Points++;
				e.Dead += (sender, args) => enemies.Remove(sender as Enemy);
				e.OutOfBounds += (sender, args) => enemies.Remove(sender as Enemy);
				enemies.Add(e);
			}
			//enemies.AddRange(v.Enemies);
			/*for (int i = 0; i < 5; i++)
				enemies.Add(new Enemy(new Animation(Enemy.StandardTexture, 1F, true, Enemy.StandardTexture.Width),
					Rng.Next(window.ClientBounds.Width - 64), -1*Rng.Next(500) - 100, 0,  (float) Rng.NextDouble()*-3 - 1,
					Enemy.Destruction));
			*/
			/*
			 menuSprite = content.Load<Texture2D>("images/menu/btnNewGame");
			 menuPos.X = window.ClientBounds.Width / 2 - menuSprite.Width / 2;
			 menuPos.Y = window.ClientBounds.Height / 2 - menuSprite.Height / 2;
			 */
			Stencil = new Font(content.Load<SpriteFont>("fonts/Stencil"));
			player.IsInvincible = true;

		}
		#region Update methods
		public static State RunUpdate(GameWindow window, GameTime gameTime)
		{
			background.Update(window);
			FPS.Update(gameTime);
			player.Update(window, gameTime);
			closestLength = 100000;
			closestEnemy = null;
			foreach (Enemy enemy in enemies.ToArray())
			{
				if ((enemy.Center - player.Center).Length() < closestLength && enemy.Y < player.Y && !enemy.IsDying)
				{
					closestLength = (enemy.Position - player.Position).Length();
					closestEnemy = enemy;
				}
				foreach (Projectile bullet in player.Bullets)
					if (enemy.IsColliding(bullet) && bullet.Y > 0 && enemy.IsAlive && bullet.IsAlive)
					{
						enemy.Lives--;
						bullet.Lives--;
					}

				if (enemy.IsAlive && enemy.IsColliding(player))
				{
					player.Lives--;
					enemy.Lives--;
				}
				enemy.Update(window);
			}
			return !player.IsAlive || player.Pause ? State.Menu : State.Play;
		}

		public static State MenuUpdate(GameTime gameTime)
		{
			player.Lives = 3;
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
			background.Draw(spriteBatch);
			player.Draw(spriteBatch, gameTime);
			spriteBatch.DrawRectangle(player.HitBox, Color.Red);
			foreach (Enemy enemy in enemies)
			{
				// enemy.DrawAnim(gameTime, spriteBatch);
				enemy.Draw(spriteBatch, gameTime);
				if (enemy != closestEnemy) continue;
				spriteBatch.DrawRectangle(enemy.HitBox, Color.Red);
				Stencil.Write("D: " + Math.Round((enemy.Center - player.Center).Length(), 1, MidpointRounding.AwayFromZero), spriteBatch, enemy.X + 4, enemy.Y + enemy.Height - 3, 0.5f);
				Stencil.Write("A: " + Math.Round(MathHelper.ToDegrees(player.Center.Angle(enemy.Center)), 1, MidpointRounding.AwayFromZero), spriteBatch, enemy.X + 4, enemy.Y + enemy.Height + 9, 0.5f);
				Stencil.Write("L: " + enemy.Lives, spriteBatch, enemy.X + 4, enemy.Y + enemy.Height + 21, 0.5F);
				if (enemy.Center.Y > 0)
					spriteBatch.DrawLine(enemy.Center, player.Center, Color.Red);
			}

			Stencil.Write(String.Format("Score: {0}\nEnemies: {1}\nFPS: {2}\nLives: {3}", player.Points, enemies.Count(x=> x.IsAlive), FPS.ToString(), player.Lives), spriteBatch, 20, 10);
			spriteBatch.Draw(hud, HUDView, Color.White);
		}
		public static void HighScoreDraw(SpriteBatch spriteBatch)
		{

		}
		public static void MenuDraw(SpriteBatch spriteBatch)
		{
			background.Draw(spriteBatch);
			menu.Draw(spriteBatch);
			//spriteBatch.Draw(menuSprite, menuPos, Color.White);

		}
		#endregion
	}
}
