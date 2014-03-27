using Microsoft.Xna.Framework.Audio;

namespace Storm_Pounder___First_Contact
{
    class Boss : StandardEnemy
    {
        public Boss(Animation a, float X, float Y, float speedX, float speedY, SoundEffect death)
            : base(a, X, Y, speedX, speedY, death)
        {
            DefaultLives = 3;
            Lives = DefaultLives;
        }
    }
}