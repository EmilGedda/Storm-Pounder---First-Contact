#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Storm_Pounder___First_Contact.Core;

#endregion

namespace Storm_Pounder___First_Contact
{
    static class GameCore
    {
        static readonly Random rng = new Random();
        public static State CurrentState;
        public enum State { Play, Menu, HighScore, Quit };

        static Texture2D menuSprite;
        static Vector2 menuPos;
        static Player player;
        public static List<StandardEnemy> enemies;
        static List<Menu> menus;
        static private int currentMenu = 0;

        private const float SpeedX = 4.5F;
        private const float SpeedY = 3.5F;
        private static Font stencil;

        public static void Initialize()
        {
            CurrentState = State.Menu;
            enemies = new List<StandardEnemy>();
        }

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            content.Load<SoundEffect>("sounds/sm64_mario_lets_go.wav").Play();
            player = new Player(content.Load<Texture2D>("images/aircraft"), window.ClientBounds.Height - 96, window.ClientBounds.Width / 2, SpeedX*2, SpeedY, content.Load<Texture2D>("images/lazer"), content.Load<SoundEffect>("sounds/Powerup4"));
            StandardEnemy.StandardTexture = content.Load<Texture2D>("images/aircraft");
            StandardEnemy.Destruction = content.Load<SoundEffect>("sounds/Explosion3.wav");
            for (int i = 0; i < 15; i++)
                enemies.Add(new StandardEnemy(StandardEnemy.StandardTexture, rng.Next(window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, StandardEnemy.Destruction ));
            menuSprite = content.Load<Texture2D>("images/buttonNG");
            menuPos.X = window.ClientBounds.Width / 2 - menuSprite.Width / 2;
            menuPos.Y = window.ClientBounds.Height/2 - menuSprite.Height / 2;
            stencil = new Font(content.Load<SpriteFont>("fonts/Stencil"));
            
        }
        #region Update methods
        public static State RunUpdate(ContentManager manager, GameWindow window, GameTime gameTime)
        {
            FPS.Update(gameTime);
            player.Update(window, gameTime);
            foreach (StandardEnemy enemy in enemies.ToArray())
            {
                foreach (Projectile bullet in player.Bullets)
                    if (enemy.IsColliding(bullet) && bullet.Y > 0)
                    {
                        enemy.IsAlive = false;
                        bullet.IsAlive = false;
                        player.Points++;
                        if(player.Points % 5 == 0)
                            enemies.Add(new StandardEnemy(StandardEnemy.StandardTexture, rng.Next(window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, StandardEnemy.Destruction));


                    }

                if (enemy.IsAlive)
                {
                    if (enemy.IsColliding(player))
                    {
                        //player.IsAlive = false;
                        enemy.IsAlive = false;
                    }
                }
                enemy.Update(window);
                //else
                //    enemies.Remove(enemy);

            }
            return !player.IsAlive || player.Pause? State.Menu : State.Play;
        }

        public static State MenuUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.S))
            {
                player.IsAlive = true;
                return State.Play;
            }
            if (keyboardState.IsKeyDown(Keys.H))
                return State.HighScore;

            return keyboardState.IsKeyDown(Keys.A) ? State.Quit : State.Menu;
        }
        public static State HighscoreUpdate()
        {
            KeyboardState kState = Keyboard.GetState();
            return kState.IsKeyDown(Keys.Escape) ? State.Menu : State.HighScore;
        }
        #endregion

        #region Drawing methods
        public static void RunDraw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
            foreach (StandardEnemy enemy in enemies)
                enemy.Draw(spriteBatch);
            stencil.Write(String.Format("Score: {0}\nEnemies: {1}\nFPS: {2}", player.Points.ToString(), enemies.Count, FPS.ToString()), spriteBatch, 0, 0);

        }
        public static void HighScoreDraw(SpriteBatch spriteBatch)
        {

        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuSprite, menuPos, Color.White);
        }
        #endregion


    }
}
