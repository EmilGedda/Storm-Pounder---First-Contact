#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storm_Pounder___First_Contact.Core;
using Storm_Pounder___First_Contact.Objects.Entity;

#endregion

namespace Storm_Pounder___First_Contact
{
	public class Main : Game
	{
		readonly GraphicsDeviceManager _graphics;
		SpriteBatch spriteBatch;

		public Main()
		{
			GameCore.ServiceProvider = Content.ServiceProvider;
			_graphics = new GraphicsDeviceManager(this)
			{
				IsFullScreen = true,
				PreferredBackBufferHeight = 1080,
				PreferredBackBufferWidth = 1920,
				PreferMultiSampling = true,
			};
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			GameCore.Initialize(_graphics, Content, Window);
			base.Initialize();
		}
		protected override void LoadContent()
		{

			spriteBatch = new SpriteBatch(GraphicsDevice);
			GameCore.LoadContent(Content, Window);
		}


		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			GameCore.Time = gameTime;
			switch (GameCore.CurrentState)
			{
				case GameCore.State.Play:
					IsMouseVisible = false;
					if (!GameCore.Levels[GameCore.currentLevel].Started)
						GameCore.Levels[GameCore.currentLevel].Start(Window);
					GameCore.CurrentState = GameCore.RunUpdate(Window, gameTime);
					break;
				case GameCore.State.HighScore:

					GameCore.CurrentState = GameCore.HighscoreUpdate();
					break;
				case GameCore.State.Quit:
					Exit();
					break;
				case GameCore.State.Options:
					GameCore.CurrentState = GameCore.OptionsUpdate();
					break;
				default:
					IsMouseVisible = true;
					GameCore.CurrentState = GameCore.MenuUpdate(gameTime);
					break;
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			switch (GameCore.CurrentState)
			{
				case GameCore.State.Play:
					if (!GameCore.Levels[GameCore.currentLevel].Started)
						GameCore.Levels[GameCore.currentLevel].Start(Window);
					GameCore.RunDraw(gameTime, spriteBatch);
					break;
				case GameCore.State.HighScore:
					GameCore.HighScoreDraw(spriteBatch);
					break;
				case GameCore.State.Quit:
					Exit();
					break;
				default:
					GameCore.MenuDraw(spriteBatch);
					break;
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
