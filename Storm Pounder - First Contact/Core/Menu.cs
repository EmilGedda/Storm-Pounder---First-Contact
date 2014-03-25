#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Storm_Pounder___First_Contact
{
    class Menu
    {
        private List<MenuItem> items;

        public bool IsActive { get; set; }
        private int lastSelected = 0;
        private int selected = 0;
        private double lastChange = 0;
        private const int slowmode = 100;
        private int defaultState;
        private SoundEffect changeEffect;

        private float margin = 20;

        public Menu(int defaultState, ContentManager content)
        {
            changeEffect = content.Load<SoundEffect>("sounds/Click");
            items = new List<MenuItem>();
            this.defaultState = defaultState;
        }
        public void AddItem(Texture2D itemTexture, int state, GameWindow window)
        {
            Vector2 pos = items.Count > 0 ? items[items.Count - 1].Position : new Vector2(window.ClientBounds.Width / 2 - itemTexture.Width / 2, itemTexture.Height * 2 + margin + margin/2);
            pos.Y += itemTexture.Height + margin;

            items.Add(new MenuItem(itemTexture, pos, state));
        }
        public int Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            for(int i = 0; i < items.Count; i++)
                if (items[i].IsMouseHovered())
                    selected = i;
            if (lastChange + slowmode < gameTime.TotalGameTime.TotalMilliseconds)
            {
                if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                    selected = (selected == items.Count - 1) ? 0 : selected + 1;

                if(keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                    selected = (selected == 0) ? items.Count - 1 : selected - 1;
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyUp(Keys.W) && keyboard.IsKeyUp(Keys.S) &&
                keyboard.IsKeyUp(Keys.Down))
                lastChange = 0;
            if (lastSelected != selected)
            {
                lastSelected = selected;
                changeEffect.Play(0.35F, -0.3F, 0F);
            }
            return keyboard.IsKeyDown(Keys.Enter) || items[selected].IsClicked() ? items[selected].State : defaultState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < items.Count; i++)
                spriteBatch.Draw(items[i].Texture, items[i].Position, i == selected ? Color.RosyBrown : Color.White);
            
        }
    }
}
