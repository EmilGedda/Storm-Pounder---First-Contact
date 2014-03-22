#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Storm_Pounder___First_Contact
{
    class Menu
    {
        private List<MenuItem> items;

        private int selected = 0;
        private double lastChange = 0;
        private const int slowmode = 130;
        private int defaultState;

        private float margin = 20;

        public Menu(int defaultState)
        {
            items = new List<MenuItem>();
            this.defaultState = defaultState;
        }
        public void AddItem(Texture2D itemTexture, int state)
        {
            Vector2 pos = items[items.Count - 1].Position;
            pos.Y += margin;

            items.Add(new MenuItem(itemTexture, pos, state));
        }
        public int Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (lastChange + slowmode < gameTime.TotalGameTime.TotalMilliseconds)
            {
                if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                    selected = (selected == items.Count - 1) ? 0 : selected + 1;

                if(keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                    selected = (selected == 0) ? items.Count - 1 : selected - 1;
            }

            if (keyboard.IsKeyDown(Keys.Enter))
                return items[selected].State;

            return defaultState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
