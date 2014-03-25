#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Timers;
#endregion

namespace Storm_Pounder___First_Contact
{
    public class Main : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        static readonly List<StandardEnemy> Enemies = new List<StandardEnemy>();

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = true,
                PreferredBackBufferHeight = 1080,
                PreferredBackBufferWidth = 1920
            };
            Content.RootDirectory = "data";
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

            switch (GameCore.CurrentState)
            {
                case GameCore.State.Play:
                    GameCore.CurrentState = GameCore.RunUpdate(Content, Window, gameTime);
                    break;
                case GameCore.State.HighScore:
                    GameCore.CurrentState = GameCore.HighscoreUpdate();
                    break;
                case GameCore.State.Quit:
                    Exit();
                    break;
                default:
                    GameCore.CurrentState = GameCore.MenuUpdate();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //var rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            //rect.SetData(new[] { Color.White });
            //spriteBatch.Draw(rect, _player.HitBox, Color.Red);
            // Hitboxes
            switch (GameCore.CurrentState)
            {
                case GameCore.State.Play:
                    GameCore.RunDraw(spriteBatch);
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
