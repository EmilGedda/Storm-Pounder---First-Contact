namespace Storm_Pounder___First_Contact
{
    class Projectile : PhysicalObject
    {
        const float ProjectileSpeed = 15F;
        private float health = 100;

        public float Health { get { return health; } set { health = value; } }

        public Projectile(Animation a, float X, float Y)
            : base(a, X, Y, 0, ProjectileSpeed)
        {
            Lives = 1;
        }
        public override void UpdateHitBox()
        {
            position.Y -= speed.Y;
            if (position.Y + Height < 0 || health <= 0)
                Lives--;
            base.UpdateHitBox();
        }
    }
}
