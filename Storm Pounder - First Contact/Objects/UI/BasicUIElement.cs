using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact.Objects.UI
{
	abstract class BasicUIElement
	{
		public Texture2D Texture { get; private set; }

		protected Vector2 position;

		public float X { get { return position.X; } }
		public float Y { get { return position.Y; } }

		protected BasicUIElement(Texture2D texture, Vector2 position)
		{
			this.position = position;
			Texture = texture;
		}

		public abstract void Update(GameWindow window, int nrBackgroundsX, int nrBackgroundsY);

		public virtual void Draw(SpriteBatch sb, Color? color = null, float opacity = 1F, float rotation = 0)
		{
			sb.Draw(Texture, position, null, color.HasValue ? color.Value : Color.White * opacity, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
		}
	}
}
