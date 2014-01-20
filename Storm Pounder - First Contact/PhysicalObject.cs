using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storm_Pounder___First_Contact
{
    class PhysicalObject : MovingObject
    {
        private bool isAlive = true;

        public bool IsAlive { get { return this.isAlive; }
                              set { this.isAlive = value; } }

        public PhysicalObject(Texture2D texture, float X, float Y, float speedX, float speedY)
            : base(texture, X, Y, speedX, speedY)
        {
        }

        public bool isColliding(PhysicalObject victim)
        {
            Rectangle we = new Rectangle(Convert.ToInt32(this.X), Convert.ToInt32(this.Y), Convert.ToInt32(this.Width), Convert.ToInt32(this.Height));
            Rectangle them = new Rectangle(Convert.ToInt32(victim.X), Convert.ToInt32(victim.Y), Convert.ToInt32(victim.Width), Convert.ToInt32(victim.Height));

            return we.Intersects(them);
        }
    }
}
