using Microsoft.Xna.Framework;
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
        private float health = 100;
        private Vector2 spawnPoint;
        public float Health { get { return health; } set { health = value; } }
        private float maxDistance = 100;
        public float MaxDistance { get { return maxDistance; } set { maxDistance = value; } } 

        public Bullet(Texture2D texture, float X, float Y)
            : base(texture, X, Y, 0, BulletSpeed)
        {

        }
        public void Update()
        {
            vector.Y -= speed.Y;
            if (vector.Y + texture.Height < 0 || health <= 0)
                IsAlive = false;
        }
    }
}
