using Microsoft.Xna.Framework;

namespace Storm_Pounder___First_Contact
{
    abstract class MovingObject : GameObject
    {
        protected Vector2 speed;

        public MovingObject(Animation a, float X, float Y, float speedX, float speedY) : base(a, X, Y)
        {
            speed.X = speedX;
            speed.Y = speedY;
        }
    }
}
