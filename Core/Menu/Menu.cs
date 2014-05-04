#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Storm_Pounder___First_Contact.Core.Menu
{
    class Menu
    {
        private readonly List<MenuItem> items;

        public bool IsActive { get; set; }
        private int lastSelected = -1;
        private int selected = -1;
        private double lastChange = 0;
        private const int slowmode = 130;
        private readonly int defaultState;

        private readonly SoundEffect changeEffect;

        private const float Margin = 20;

        public Menu(int defaultState, ContentManager content)
        {  
            changeEffect = content.Load<SoundEffect>("sounds/Click2");
            items = new List<MenuItem>(4);
            this.defaultState = defaultState;
        }
        public void AddItem(Texture2D itemTexture, int state, GameWindow window)
        {
            Vector2 pos = items.Count > 0 ? items[items.Count - 1].Position : new Vector2(window.ClientBounds.Width / 2 - itemTexture.Width / 2, itemTexture.Height * 2 + Margin + Margin/2);
            pos.Y += itemTexture.Height + Margin;

            items.Add(new MenuItem(itemTexture, pos, state));
        }
        public int Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].MouseDehovered())
                {
                    selected = -1;
                    lastSelected = -1;

                }
                if (items[i].IsMouseHovered())
                    selected = i;
                
            }
            if (lastChange + slowmode < gameTime.TotalGameTime.TotalMilliseconds)
            {
                if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                    selected = (selected >= items.Count - 1) ? 0 : selected + 1;

                if(keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                    selected = (selected <= 0) ? items.Count - 1 : selected - 1;
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;

            }
            if (keyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyUp(Keys.W) && keyboard.IsKeyUp(Keys.S) &&
                keyboard.IsKeyUp(Keys.Down))
                lastChange = 0;
            if (lastSelected != selected && selected != -1)
            {
                lastSelected = selected;
                changeEffect.Play(0.5F, 0F, 0F);
            }

            return  selected > -1  && (keyboard.IsKeyDown(Keys.Enter) || items[selected].IsClicked()) ? items[selected].State : defaultState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < items.Count; i++)
                spriteBatch.Draw(items[i].Texture, items[i].Position, i == selected ? Color.RosyBrown : Color.White);
            
        }
    }
}
