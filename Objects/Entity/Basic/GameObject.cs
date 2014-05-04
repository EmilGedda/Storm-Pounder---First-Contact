using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Storm_Pounder___First_Contact.Objects.Entity.Basic
{
	abstract class GameObject
	{
		public Animation IdleAnimation { get; private set; }
		protected AnimationPlayer idlePlayer;
		protected Vector2 position;
		protected Color[] textureData;
		public Vector2 Center
		{
			get
			{
				return new Vector2(position.X + Width / 2, position.Y + Height / 2);
			}
		}

		public Vector2 Position
		{
			get { return position; }
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		public float X { get { return position.X; } set { position.X = value; } }
		public float Y { get { return position.Y; } set { position.Y = value; } }
		private float rotation;
		public int Width { get { return IdleAnimation.FrameWidth; } }
		public int Height { get { return IdleAnimation.FrameHeight; } }

		protected GameObject(Animation a, Vector2 position)
		{
			IdleAnimation = a;
			this.position = position;
			idlePlayer.PlayAnimation(a);
			textureData = new Color[Width * Height];
			//animation.Texture.GetData(textureData);
		}

		public void Draw(SpriteBatch sb, GameTime gameTime, SpriteEffects spriteEffect, Color? color = null, float opacity = 1F)
		{
			idlePlayer.Draw(gameTime, sb, Center, spriteEffect, color, opacity, rotation);

			//sb.Draw(texture, position, null, color.HasValue ? color.Value : Color.White * opacity, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
			//sb.Draw(texture, position, Color.White * opacity);
			//((position.Y < 25) ? (position.Y / 25): opacity)
		}
		public virtual void Draw(SpriteBatch sb, GameTime gameTime)
		{
			Draw(sb, gameTime, SpriteEffects.None);
		}
		/*
				public override bool Equals(object obj)
				{
					if (obj == null)
						return false;
					var t = obj as GameObject;
					return t != null && t.guid == guid;
				}

				public override int GetHashCode()
				{
					return guid.GetHashCode();
				}
				*/
	}
}