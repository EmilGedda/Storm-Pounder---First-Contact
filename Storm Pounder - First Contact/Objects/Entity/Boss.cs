using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Storm_Pounder___First_Contact
{
    class Boss : Enemy
    {
       
		public Boss(Animation a, Vector2 position, Vector2 speed, SoundEffect death)
			: base(a, position, speed, death, sender => Vector2.Zero, sender => Vector2.Zero)
        {
            DefaultLives = 3;
            Lives = DefaultLives;
        }
	    
    }
}