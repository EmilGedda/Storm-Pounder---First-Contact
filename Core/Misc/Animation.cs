using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class Animation
    {
        public Texture2D Texture { get; private set; }

        public float FrameTime { get; private set; }

        public bool IsLooping { get; private set; }

        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        public int FrameWidth { get; set; }

        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        public Animation(Texture2D texture, float frameTime, bool isLooping, int width)
        {
            Texture = texture;
            FrameTime = frameTime;
            IsLooping = isLooping;
            FrameWidth = width;
        }
		public Animation(Texture2D texture, float frameTime, bool isLooping)
		{
			Texture = texture;
			FrameTime = frameTime;
			IsLooping = isLooping;
			FrameWidth = texture.Height;
		}
    }
}
