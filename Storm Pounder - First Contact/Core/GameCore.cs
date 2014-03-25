#region Using Statements

using System;
using System.Collections.Generic;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion
namespace Storm_Pounder___First_Contact
{
    static class GameCore
    {
        public static readonly Random rng = new Random();
        public static State CurrentState;
        public enum State { Play, Menu, HighScore, Options, Quit };

        private static Background background;
        static Texture2D menuSprite;
        static Vector2 menuPos;
        static Player player;
        public static List<StandardEnemy> enemies;
        private static Menu menu;
        static private int currentMenu = 0;

        private const float SpeedX = 4.5F;
        private const float SpeedY = 3.5F;
        private static Font stencil;

        private static StandardEnemy closestEnemy;
        private static float closestLength;
        private static GraphicsDeviceManager graphicsDevice;
        public static void Initialize(GraphicsDeviceManager graphics)
        {
            CurrentState = State.Menu;
            enemies = new List<StandardEnemy>();
            graphicsDevice = graphics;
        }

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            MouseState s = new MouseState();
            
            menu = new Menu((int)State.Menu, content);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnNewGame"), (int)State.Play, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnHighScore"), (int)State.HighScore, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnOptions"), (int)State.Options, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnQuit"), (int)State.Quit, window);

            background = new Background(content, "images/background/endless/", window);
            
            content.Load<SoundEffect>("sounds/sm64_mario_lets_go.wav").Play();
            
            player = new Player(content.Load<Texture2D>("images/aircraft"), window.ClientBounds.Height - 96, window.ClientBounds.Width / 2, SpeedX * 2, SpeedY, content.Load<Texture2D>("images/lazer"), content.Load<SoundEffect>("sounds/Powerup4"));
            
            StandardEnemy.StandardTexture = content.Load<Texture2D>("images/enemy");
            StandardEnemy.Destruction = content.Load<SoundEffect>("sounds/Explosion3.wav");
            for (int i = 0; i < 15; i++)
                enemies.Add(new StandardEnemy(StandardEnemy.StandardTexture, rng.Next(window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, (float)rng.NextDouble() * -3 - 1, StandardEnemy.Destruction));
           /*
            menuSprite = content.Load<Texture2D>("images/menu/btnNewGame");
            menuPos.X = window.ClientBounds.Width / 2 - menuSprite.Width / 2;
            menuPos.Y = window.ClientBounds.Height / 2 - menuSprite.Height / 2;
            */
            stencil = new Font(content.Load<SpriteFont>("fonts/Stencil"));
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
            foreach (StandardEnemy enemy in enemies.ToArray())
            {
                if ((enemy.Position - player.Position).Length() < closestLength && enemy.Y < player.Y)
                {
                    closestLength = (enemy.Position - player.Position).Length();
                    closestEnemy = enemy;
                }
                foreach (Projectile bullet in player.Bullets)
                    if (enemy.IsColliding(bullet) && bullet.Y > 0 && enemy.IsAlive && bullet.IsAlive)
                    {
                        enemy.IsAlive = false;
                        bullet.IsAlive = false;
                        player.Points++;
                        if (player.Points % 5 == 0)
                            enemies.Add(new StandardEnemy(StandardEnemy.StandardTexture, rng.Next(window.ClientBounds.Width - 64), -1 * rng.Next(500) - 100, 0, rng.Next(10, 40) / -10, StandardEnemy.Destruction));
                        

                    }

                if (enemy.IsAlive)
                {
                    if (enemy.IsColliding(player))
                    {
                        player.IsAlive = false;
                        enemy.IsAlive = false;
                    }
                }
                enemy.Update(window);
                //else
                //    enemies.Remove(enemy);

            }
            return !player.IsAlive || player.Pause ? State.Menu : State.Play;
        }

        public static State MenuUpdate(GameTime gameTime)
        {
            return (State) menu.Update(gameTime);
            /*
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.S))
            {
                player.IsAlive = true;
                return State.Play;
            }
            if (keyboardState.IsKeyDown(Keys.H))
                return State.HighScore;

            return keyboardState.IsKeyDown(Keys.A) ? State.Quit : State.Menu;*/
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
            background.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (StandardEnemy enemy in enemies)
            {

                enemy.Draw(spriteBatch);
                if (enemy != closestEnemy) continue;
                spriteBatch.DrawRectangle(enemy.HitBox, Color.Red);
                stencil.Write("D: " + Math.Round((enemy.Center - player.Center).Length(), 1, MidpointRounding.AwayFromZero), spriteBatch, enemy.X + 4, enemy.Y + enemy.Height - 3, 0.4f);
                stencil.Write("A: " + Math.Round(MathHelper.ToDegrees(player.Center.Angle(enemy.Center)), 1, MidpointRounding.AwayFromZero), spriteBatch, enemy.X + 4, enemy.Y + enemy.Height + 10, 0.4f);
                if(enemy.Center.Y > 0)
                    spriteBatch.DrawLine(enemy.Center, player.Center, Color.Red);
            }

            stencil.Write(String.Format("Score: {0}\nEnemies: {1}\nFPS: {2}", player.Points, enemies.Count, FPS.ToString()), spriteBatch, 20, 10);

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
