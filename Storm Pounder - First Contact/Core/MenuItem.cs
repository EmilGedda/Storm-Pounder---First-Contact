#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using System.Timers;
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
            this.state = currentstate;
        }
    }
}
