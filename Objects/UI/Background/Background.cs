using System;
using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact.Objects.UI.Background
{
    class Background
    {
        private BackgroundSprite[][] background;
        private Texture2D[] sprites;
        private int nrBackgroundsX, nrBackgroundsY;

        public  Background(ContentManager content, string folder, GameWindow window)
        {

            sprites = new Texture2D[21];
            sprites[0] = content.Load<Texture2D>(folder + "empty");
            for (int i = 1; i < 21; i++)
                sprites[i] = content.Load<Texture2D>(folder + "space_" + i);

            nrBackgroundsX = (int)Math.Ceiling((double)window.ClientBounds.Width / 80) + 1; // 80 = texture.Width
            nrBackgroundsY = (int)Math.Ceiling((double)window.ClientBounds.Height / 80) + 1;
            background = new BackgroundSprite[nrBackgroundsX][];

            int[] list = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            for (int i = 0; i < nrBackgroundsX; i++)
                background[i] = new BackgroundSprite[nrBackgroundsY];
            for (int i = 0; i < nrBackgroundsX; i++)
            {
                list.Shuffle();
                for (int j = 0; j < nrBackgroundsY; j++)
					background[i][j] = new BackgroundSprite(sprites[list[j]], new Vector2(0, 0.7F), new Vector2((i - 1) * sprites[list[j]].Width, (j - 1) * sprites[list[j]].Width));              
            }
        }

        public void Update(GameWindow window)
        {
            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                    background[i][j].Update(window, nrBackgroundsX, nrBackgroundsY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                    background[i][j].Draw(spriteBatch, opacity: 0.7F);
        }
    }
}
