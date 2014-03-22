using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class PhysicalObject : MovingObject
    {
        private bool isAlive = true;
        private Vector2 spawnPoint;
        private Rectangle hitbox;

        public Vector2 SpawnPoint
        {
            get { return this.spawnPoint; }
            set { this.spawnPoint = value; }
        }

        public Rectangle HitBox
        {
            get { return hitbox; }
            set { this.hitbox = value; }
        }

        public bool IsAlive
        {
            get { return this.isAlive; }
            set { this.isAlive = value; }
        }

        public PhysicalObject(Texture2D texture, float X, float Y, float speedX, float speedY)
            : base(texture, X, Y, speedX, speedY)
        {
            //this.hitbox = new Rectangle(Convert.ToInt32(X+10), Convert.ToInt32(Y+10), Convert.ToInt32(this.Width-20),Convert.ToInt32(this.Height-20));
            this.hitbox.X = Convert.ToInt32(X + 10);
            this.hitbox.Y = Convert.ToInt32(Y + 10);
            this.hitbox.Width = Convert.ToInt32(this.Width - 20);
            this.hitbox.Height = Convert.ToInt32(this.Height - 20);

            spawnPoint.X = X;
            spawnPoint.Y = Y;
        }

        public bool isColliding(PhysicalObject victim)
        {
            return this.hitbox.Intersects(victim.HitBox);
        }
    }
}
