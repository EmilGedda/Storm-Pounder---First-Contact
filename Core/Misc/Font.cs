using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class Font
    {
        private readonly SpriteFont spriteFont;
        public float Scale { get; set; }
		public Color Color { get; set; }
        public float Rotation { get; set; }
        public string Text { get; set; }

        public Font(SpriteFont spriteFont, float scale = 1, float rotation = 0)
        {
	        Color = Color.White;
            this.spriteFont = spriteFont;
            Scale = scale;
            Rotation = rotation;
        }

        public void Write(string text, SpriteBatch spriteBatch, float X, float Y)
        {
            spriteBatch.DrawString(spriteFont, text, new Vector2(X, Y), Color, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 1);
        }
        public void Write(string text, SpriteBatch spriteBatch, float X, float Y, float scale)
        {
            spriteBatch.DrawString(spriteFont, text, new Vector2(X, Y), Color, Rotation, Vector2.Zero, scale, SpriteEffects.None, 1);
        }
        public float Width(string text)
        {
            return Size(text).X;
        }

        public float Height(string text)
        {
            return Size(text).Y;
        }
        public Vector2 Size(string text)
        {
            return spriteFont.MeasureString(text);
        }
        
    }
}
