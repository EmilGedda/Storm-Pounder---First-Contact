#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Storm_Pounder___First_Contact
{
    class MenuItem
    {
        private Vector2 position;

        public Texture2D Texture { get; private set; }
        public Vector2 Position { get { return position; } }

        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }
        
        private MouseState lastState;
        private ButtonState leftButtonState;
        public int State { get; private set; }

        public MenuItem(Texture2D texture, Vector2 position, int currentstate)
        {
            Texture = texture;
            this.position = position;
            State = currentstate;
        }

        public bool IsMouseHovered()
        {
            var newState = Mouse.GetState();
            if (lastState == newState)
                return false;
            lastState = newState;
            return newState.X >= X && newState.X <= X + Texture.Width && newState.Y >= Y && newState.Y <= Y + Texture.Height;
        }

        public bool IsClicked()
        {
                MouseState tmp = Mouse.GetState();
                bool b = ButtonState.Released == tmp.LeftButton && leftButtonState == ButtonState.Pressed;
                leftButtonState = tmp.LeftButton;
                return b && tmp.X >= X && tmp.X <= X + Texture.Width && tmp.Y >= Y && tmp.Y <= Y + Texture.Height;
        }

        public bool MouseDehovered()
        {
            bool last = lastState.X >= X && lastState.X <= X + Texture.Width && lastState.Y >= Y && lastState.Y <= Y + Texture.Height;
            MouseState tmp = Mouse.GetState();
            bool current = tmp.X >= X && tmp.X <= X + Texture.Width && tmp.Y >= Y && tmp.Y <= Y + Texture.Height;
            return last && !current;
        }
    }
}
