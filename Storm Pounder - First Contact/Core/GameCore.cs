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
        public static readonly Random Rng = new Random();
        public static State CurrentState;
        public enum State { Play, Menu, HighScore, Options, Quit };

        private static Background background;
        static Player player;
        public static List<StandardEnemy> enemies;
        private static Menu menu;
        static private int currentMenu = 0;

        private const float SpeedX = 4.5F;
        private const float SpeedY = 4.0F;
        public static Font Stencil;
        private static Boss boss;
        private static StandardEnemy closestEnemy;
        private static float closestLength;
        private static GraphicsDeviceManager graphicsDevice;
        public static ContentManager contentMngr;
        private static GameWindow _window;
        public static void Initialize(GraphicsDeviceManager graphics)
        {
            CurrentState = State.Menu;
            enemies = new List<StandardEnemy>();
            graphicsDevice = graphics;
        }

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            boss = new Boss(new Animation(content.Load<Texture2D>("images/boss"), 0.1F, true, 100),Rng.Next(window.ClientBounds.Width - 64), -1*Rng.Next(500) - 100, 0, (float) Rng.NextDouble()*-3 - 1,
                    StandardEnemy.Destruction);
            enemies.Add(boss);
            MouseState s = new MouseState();
            contentMngr = content;
            _window = window;
            menu = new Menu((int)State.Menu, content);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnNewGame"), (int)State.Play, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnHighScore"), (int)State.HighScore, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnOptions"), (int)State.Options, window);
            menu.AddItem(content.Load<Texture2D>("images/menu/btnQuit"), (int)State.Quit, window);

            background = new Background(content, "images/background/endless/", window);
            
            content.Load<SoundEffect>("sounds/sm64_mario_lets_go.wav").Play();
            
            player = new Player(new Animation(content.Load<Texture2D>("images/aircraft"), 1F, true, 64), window.ClientBounds.Height - 96, window.ClientBounds.Width / 2, SpeedX * 2, SpeedY, content.Load<Texture2D>("images/lazer"), content.Load<SoundEffect>("sounds/shoot"));
            
            StandardEnemy.StandardTexture = content.Load<Texture2D>("images/enemy");
            StandardEnemy.Destruction = content.Load<SoundEffect>("sounds/Explosion3.wav");
            for (int i = 0; i < 10; i++)
                enemies.Add(new StandardEnemy(new Animation(StandardEnemy.StandardTexture, 1F, true, StandardEnemy.StandardTexture.Width),
                    Rng.Next(window.ClientBounds.Width - 64), -1*Rng.Next(500) - 100, 0, (float) Rng.NextDouble()*-3 - 1,
                    StandardEnemy.Destruction));
           /*
            menuSprite = content.Load<Texture2D>("images/menu/btnNewGame");
            menuPos.X = window.ClientBounds.Width / 2 - menuSprite.Width / 2;
            menuPos.Y = window.ClientBounds.Height / 2 - menuSprite.Height / 2;
            */
            Stencil = new Font(content.Load<SpriteFont>("fonts/Stencil"));
            //player.IsInvincible = true;

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
                            enemies.Add(new StandardEnemy(new Animation(StandardEnemy.StandardTexture, 1F, true, StandardEnemy.StandardTexture.Width), Rng.Next(window.ClientBounds.Width - 64), -1 * Rng.Next(500) - 100, 0, Rng.Next(10, 40) / -10, StandardEnemy.Destruction));
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
            player.IsAlive = true;
            return (State) menu.Update(gameTime);
        }
        public static State HighscoreUpdate()
        {
            KeyboardState kState = Keyboard.GetState();
            return kState.IsKeyDown(Keys.Escape) ? State.Menu : State.HighScore;
        }
        public static State OptionsUpdate()
        {
            background = new Background(contentMngr, "images/background/endless/", _window);
            return State.Menu;
        }
        #endregion

        #region Drawing methods
        public static void RunDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            player.Draw(spriteBatch, gameTime);
            foreach (StandardEnemy enemy in enemies)
            {
               // enemy.DrawAnim(gameTime, spriteBatch);
                enemy.Draw(spriteBatch, gameTime, SpriteEffects.FlipVertically);
                if (enemy != closestEnemy) continue;
                spriteBatch.DrawRectangle(enemy.HitBox, Color.Red);
                Stencil.Write("D: " + Math.Round((enemy.Center - player.Center).Length(), 1, MidpointRounding.AwayFromZero), spriteBatch, enemy.X + 4, enemy.Y + enemy.Height - 3, 0.4f);
                Stencil.Write("A: " + Math.Round(MathHelper.ToDegrees(player.Center.Angle(enemy.Center)), 1, MidpointRounding.AwayFromZero), spriteBatch, enemy.X + 4, enemy.Y + enemy.Height + 10, 0.4f);
                if(enemy.Center.Y > 0)
                    spriteBatch.DrawLine(enemy.Center, player.Center, Color.Red);
            }

            Stencil.Write(String.Format("Score: {0}\nEnemies: {1}\nFPS: {2}", player.Points, enemies.Count, FPS.ToString()), spriteBatch, 20, 10);

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
