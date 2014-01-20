using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storm_Pounder___First_Contact
{
    class Bullet : PhysicalObject
    {
        const float BulletSpeed = 15F;
        public Bullet(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 0, BulletSpeed)
        {

        }
        public void Update()
        {
            vector.Y -= speed.Y;
            if (vector.Y + texture.Height < 0)
                IsAlive = false;
        }
    }
}
