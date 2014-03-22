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
        SpriteBatch _spriteBatch;
        Player _player;

        private SoundEffect _startup;

        private const float SpeedX = 4.5F;
        private const float SpeedY = 2.5F;
        static int _score = 0;
        static readonly List<StandardEnemy> Enemies = new List<StandardEnemy>();
        const int NumEnemies = 10;
        static readonly Random rng = new Random();

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            /*graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.PreferredBackBufferWidth = 1920;*/
            Content.RootDirectory = "data";
        }

        protected override void Initialize()
        {
            Timer t = new Timer(1000);
            t.Elapsed += PreventMemoryLeak;
            t.Start();
            _startup = Content.Load<SoundEffect>("sounds/sm64_mario_lets_go.wav");
            _startup.Play();
            base.Initialize();
        }

        private static void PreventMemoryLeak(object sender, ElapsedEventArgs args)
        {
            GC.Collect();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _player = new Player(Content.Load<Texture2D>("images/aircraft"), Window.ClientBounds.Height - 96, Window.ClientBounds.Width / 2, SpeedX, SpeedY, Content.Load<Texture2D>("images/lazer"), Content.Load<SoundEffect>("sounds/Powerup4"));
            for (int i = 0; i < NumEnemies; i++)
                Enemies.Add(new StandardEnemy(Content.Load<Texture2D>("images/aircraft"), rng.Next(Window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, Content.Load<SoundEffect>("sounds/Explosion3.wav")));
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(Window, gameTime);
            StandardEnemy[] arr = Enemies.ToArray();
            foreach (StandardEnemy e in arr)
            {
                e.Update(Window);
                foreach (Projectile p in _player.Bullets)
                    if (e.isColliding(p))
                    {
                        
                       // p.IsAlive = false;
                        e.IsAlive = false;
                        //_score++;
                        //if (Score % 8 == 0)
                        //    Enemies.Add(new StandardEnemy(Content.Load<Texture2D>("images/aircraft"), rng.Next(Window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, Content.Load<SoundEffect>("sounds/Explosion3.wav")));

                    }
                //if (e.isColliding(player))
                  //  Exit();

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            /*var rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            //spriteBatch.Draw(rect, new Rectangle((int)player.X, (int)player.Y, (int)player.Width, (int)player.Height), Color.Red);*/
            _player.Draw(_spriteBatch);
            foreach (StandardEnemy e in Enemies)
            {
                //var rect2 = new Texture2D(graphics.GraphicsDevice, 1, 1);
                //rect2.SetData(new[] { Color.White });
                //spriteBatch.Draw(rect2, new Rectangle((int)e.X, (int)e.Y, (int)e.Width, (int)e.Height), Color.Red);
                e.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
