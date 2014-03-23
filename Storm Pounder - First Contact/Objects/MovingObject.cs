﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    abstract class MovingObject : GameObject
    {
        protected Vector2 speed;

        public MovingObject(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y)
        {
            speed.X = speedX;
            speed.Y = speedY;
        }
    }
}
