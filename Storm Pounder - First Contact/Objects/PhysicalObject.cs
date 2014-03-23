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
        private int margin = 10;
        public Vector2 SpawnPoint
        {
            get { return spawnPoint; }
            set { spawnPoint = value; }
        }

        private Rectangle HitBox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        protected PhysicalObject(Texture2D texture, float X, float Y, float speedX, float speedY)
            : base(texture, X, Y, speedX, speedY)
        {

            hitbox = (this is Projectile) ? new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Width), Convert.ToInt32(Height)) : new Rectangle(Convert.ToInt32(X + 5), Convert.ToInt32(Y + 5), Convert.ToInt32(Width - 10), Convert.ToInt32(Height - 10));
            margin = this is Projectile ? 0 : 5;
            spawnPoint.X = X;
            spawnPoint.Y = Y;
        }

        public virtual void Update()
        {
            hitbox.X = (int)X + margin;
            hitbox.Y = (int) Y + margin;
        }
        public bool IsColliding(PhysicalObject victim)
        {
            return hitbox.Intersects(victim.hitbox);
        }
    }
}
