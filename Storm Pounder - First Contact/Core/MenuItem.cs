#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Storm_Pounder___First_Contact
{
    class MenuItem
    {
        private Texture2D texture;
        private Vector2 position;
        private int state;

        public Texture2D Texture { get { return texture; } }
        public Vector2 Position { get { return position; } }

        public float X { get { return position.X; } }
        public float Y { get { return position.Y; } }

        public int State { get { return state; } }

        public MenuItem(Texture2D texture, Vector2 position, int currentstate)
        {
            this.texture = texture;
            this.position = position;
            state = currentstate;
        }
    }
}
