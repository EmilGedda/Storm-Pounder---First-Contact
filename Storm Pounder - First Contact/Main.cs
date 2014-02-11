#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using System.Timers;
#endregion

namespace Storm_Pounder___First_Contact
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        private SoundEffect startup;

        private const float speedX = 4.5F;
        private const float speedY = 2.5F;
        static int Score;
        static List<StandardEnemy> Enemies = new List<StandardEnemy>();
        const int numEnemies = 10;
        static Random rng = new Random();

        public Main()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
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
            startup = Content.Load<SoundEffect>("sounds/sm64_mario_lets_go.wav");
            startup.Play();
            base.Initialize();
        }

        private void PreventMemoryLeak(object sender, ElapsedEventArgs args)
        {
            GC.Collect();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(Content.Load<Texture2D>("images/aircraft"), Window.ClientBounds.Height - 96, Window.ClientBounds.Width / 2, speedX, speedY, Content.Load<Texture2D>("images/lazer"), Content.Load<SoundEffect>("sounds/Powerup4"));
            for (int i = 0; i < numEnemies; i++)
                Enemies.Add(new StandardEnemy(Content.Load<Texture2D>("images/aircraft"), rng.Next(Window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, Content.Load<SoundEffect>("sounds/Explosion3.wav")));
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(Window, gameTime);
            StandardEnemy[] arr = Enemies.ToArray();
            foreach (StandardEnemy e in arr)
            {
                e.Update(Window);
                foreach (Projectile p in player.Bullets)
                    if (e.isColliding(p))
                    {
                        
                       // p.IsAlive = false;
                        e.IsAlive = false;
                        Score++;
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
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            /*var rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            //spriteBatch.Draw(rect, new Rectangle((int)player.X, (int)player.Y, (int)player.Width, (int)player.Height), Color.Red);*/
            player.Draw(spriteBatch);
            foreach (StandardEnemy e in Enemies)
            {
                //var rect2 = new Texture2D(graphics.GraphicsDevice, 1, 1);
                //rect2.SetData(new[] { Color.White });
                //spriteBatch.Draw(rect2, new Rectangle((int)e.X, (int)e.Y, (int)e.Width, (int)e.Height), Color.Red);
                e.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
