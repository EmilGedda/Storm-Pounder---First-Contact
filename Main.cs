#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Storm_Pounder___First_Contact
{
    public class Main : Game
    {
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;
        static readonly List<Enemy> Enemies = new List<Enemy>();

        public Main() 
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = true,
                PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,
                PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                PreferMultiSampling = true,
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GameCore.Initialize(_graphics);
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
                    
                    GameCore.CurrentState = GameCore.RunUpdate(Window, gameTime);
                    break;
                case GameCore.State.HighScore:
                    IsMouseVisible = true;
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
            //var rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            //rect.SetData(new[] { Color.White });
            //spriteBatch.Draw(rect, _player.HitBox, Color.Red);
            // Hitboxes
            switch (GameCore.CurrentState)
            {
                case GameCore.State.Play:
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
