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

        public int State { get; private set; }

        public MenuItem(Texture2D texture, Vector2 position, int currentstate)
        {
            Texture = texture;
            this.position = position;
            State = currentstate;
        }

        public bool IsMouseHovered()
        {
            Mouse.POINT pointNew;
            if (!Mouse.GetCursorPos(out pointNew)) return false;
            return pointNew.X >= X && pointNew.X <= X + Texture.Width && pointNew.Y >= Y && pointNew.Y <= Y + Texture.Height;
        }

        public bool IsClicked()
        {
                return ButtonState.Pressed == Mouse.GetState().LeftButton && IsMouseHovered();            
        }
    }
}
