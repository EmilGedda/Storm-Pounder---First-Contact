using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Storm_Pounder___First_Contact.Core
{
    class Font
    {
        private readonly SpriteFont spriteFont;
        public float Scale { get; set; }
        public float Rotation { get; set; }

        public Font(SpriteFont spriteFont, float scale = 1, float rotation = 0)
        {
            this.spriteFont = spriteFont;
            Scale = scale;
            Rotation = rotation;
        }

        public void Write(string text, SpriteBatch spriteBatch, int X, int Y)
        {
            spriteBatch.DrawString(spriteFont, text, new Vector2(X, Y), Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 1);
        }
        
    }
}
