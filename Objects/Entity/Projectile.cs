using Microsoft.Xna.Framework;
using Storm_Pounder___First_Contact.Core;
using Storm_Pounder___First_Contact.Core.Event;
using Storm_Pounder___First_Contact.Objects.Entity.Basic;

namespace Storm_Pounder___First_Contact.Objects.Entity
{
    class Projectile : PhysicalObject
    {
        const float ProjectileSpeed = 15F;

        public Projectile(Animation a, Vector2 position)
            : base(a, position, new Vector2(0, ProjectileSpeed))
        {
            Health = 1;
        }

	    public void Update()
	    {
			position.Y -= speed.Y;
			if (position.Y + Height < 0)
				OnOutOfBounds(new GameEventArgs(GameCore.Time));
			UpdateHitBox();
	    }
    }
}
