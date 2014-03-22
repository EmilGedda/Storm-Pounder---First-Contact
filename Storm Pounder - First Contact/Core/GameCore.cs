#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Storm_Pounder___First_Contact
{
    static class GameCore
    {
        public enum State { Play, Menu, HighScore, Quit };
        static List<StandardEnemy> Enemies;
        static List<Menu> menus;
        static private int currentMenu = 0;

        public static void Initialize()
        {
            Enemies = new List<StandardEnemy>();
        }

        public static void LoadContent(ContentManager content, GameWindow window)
        {

        }

        public static void Update()
        {

        }
        public static void MenuDraw(SpriteBatch spriteBatch)
        {
        }
        
    }
}
