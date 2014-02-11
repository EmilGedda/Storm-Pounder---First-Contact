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
        private Vector2 spawnPoint;

        public Vector2 SpawnPoint
        {
            get { return this.spawnPoint; }
            set { this.spawnPoint = value; }
        }

        public bool IsAlive
        {
            get { return this.isAlive; }
            set { this.isAlive = value; }
        }

        public PhysicalObject(Texture2D texture, float X, float Y, float speedX, float speedY)
            : base(texture, X, Y, speedX, speedY)
        {
            spawnPoint = new Vector2(X, Y);
        }

        public bool isColliding(PhysicalObject victim)
        {
            Rectangle we = new Rectangle(Convert.ToInt32(this.X+10), Convert.ToInt32(this.Y+10), Convert.ToInt32(this.Width-20), Convert.ToInt32(this.Height-20));
            Rectangle them = new Rectangle(Convert.ToInt32(victim.X), Convert.ToInt32(victim.Y), Convert.ToInt32(victim.Width), Convert.ToInt32(victim.Height));

            return we.Intersects(them);
        }
    }
}
