using Microsoft.Xna.Framework;

namespace Storm_Pounder___First_Contact.Objects.Entity.Basic
{
    abstract class MovingObject : GameObject
    {
        protected Vector2 speed;
		public Vector2 Speed { get { return speed; }}
        public MovingObject(Animation a, Vector2 position, Vector2 speed) : base(a, position)
        {
	        this.speed = speed;
        }
    }
}
