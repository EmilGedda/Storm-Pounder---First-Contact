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
#endregion

namespace Storm_Pounder___First_Contact
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        private SoundEffect startup;

        private const float speedX = 4.5F;
        private const float speedY = 2.5F;
        private float frameRate = 0;
        static int Score;
        static List<StandardEnemy> Enemies = new List<StandardEnemy>();
        const int numEnemies = 10;
        static Random rng = new Random();

        public Main()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.PreferredBackBufferWidth = 1920;
            Content.RootDirectory = "data";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            startup = Content.Load<SoundEffect>("sounds/sm64_mario_lets_go.wav");
            startup.Play();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(Content.Load<Texture2D>("images/aircraft"), Window.ClientBounds.Height - 96, Window.ClientBounds.Width / 2, speedX, speedY, Content.Load<Texture2D>("images/lazer"), Content.Load<SoundEffect>("sounds/Powerup4"));
            for (int i = 0; i < numEnemies; i++)
                Enemies.Add(new StandardEnemy(Content.Load<Texture2D>("images/aircraft"), rng.Next(Window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, Content.Load<SoundEffect>("sounds/Explosion3.wav")));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(Window, gameTime);
            foreach (StandardEnemy e in Enemies.ToArray())
            {
                e.Update(Window);
                foreach (Projectile p in player.Bullets)
                    if (e.isColliding(p))
                    {
                        p.IsAlive = false;
                        e.IsAlive = false;
                        Score++;
                        if (Score % 8 == 0)
                            Enemies.Add(new StandardEnemy(Content.Load<Texture2D>("images/aircraft"), rng.Next(Window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, Content.Load<SoundEffect>("sounds/Explosion3.wav")));

                    }
                if (e.isColliding(player))
                    Exit();

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
                /*var rect2 = new Texture2D(graphics.GraphicsDevice, 1, 1);
                rect2.SetData(new[] { Color.White });
                spriteBatch.Draw(rect2, new Rectangle((int)e.X, (int)e.Y, (int)e.Width, (int)e.Height), Color.Red);*/
                e.Draw(spriteBatch);
            }
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Window.Title = "FPS: " + ((int)frameRate).ToString() + " Score: " + Score + " Enemies: " + Enemies.Count;
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
