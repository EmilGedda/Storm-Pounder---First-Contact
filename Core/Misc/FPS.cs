using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace Storm_Pounder___First_Contact
{
    static class FPS
    {
        static int frameRate = 0;
        static int frameCounter = 0;
        static TimeSpan elapsedTime = TimeSpan.Zero;

        public static void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime <= TimeSpan.FromSeconds(1)) return;

            elapsedTime -= TimeSpan.FromSeconds(1);
            frameRate = frameCounter;
            frameCounter = 0;
        }
        public new static string ToString()
        {
            frameCounter++;
            return frameRate.ToString(CultureInfo.InvariantCulture);
        }
    }
}
