using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact
{
    class Background
    {
        private BackgroundSprite[][] background;
        private Texture2D[] sprites; 
        private int nrBackgroundsX, nrBackgroundsY;
        public Background(ContentManager content, string folder, GameWindow window)
        {

            sprites = new Texture2D[21];
            sprites[0] = content.Load<Texture2D>(folder + "empty");
            for (int i = 1; i < 21; i++)
                sprites[i] = content.Load<Texture2D>(folder + "space_" + i);
            
            nrBackgroundsX = (int)Math.Ceiling((double)window.ClientBounds.Width / 80) + 1; // 80 = texture.Width
            nrBackgroundsY = (int)Math.Ceiling((double)window.ClientBounds.Height / 80) + 1;
            background = new BackgroundSprite[nrBackgroundsX][];

            int current = GameCore.rng.Next(0, 20);
            for (int i = 0; i < nrBackgroundsX; i++)
                background[i] = new BackgroundSprite[nrBackgroundsY];
            for (int i = 0; i < nrBackgroundsX; i++)
                for (int j = 0; j < nrBackgroundsY; j++)
                {
                    int posX = i * 80 - 80;
                    int posY = j * 80 - 80;
                    background[i][j] = new BackgroundSprite(sprites[current++ % 21], new Vector2(0, 0.7F), posX, posY );
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
