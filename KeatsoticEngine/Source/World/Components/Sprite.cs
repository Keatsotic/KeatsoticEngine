using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class Sprite : Component
	{
		private Texture2D _texture;
		public int Width { get; private set; }
		public int Height { get; private set; }
		public Vector2 Position { get; private set; }

		public override ComponentType ComponentType => ComponentType.Sprite;

		public Sprite(Texture2D texture, int width, int height, Vector2 position)
		{
			_texture = texture;
			Width = width;
			Height = height;
			Position = position;
		}

		public override void Update(double gameTime)
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			var animation = GetComponent<Animation>(ComponentType.Animation);
			if (animation != null)
			{
				spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Width * Game1.Scale, Height * Game1.Scale),
					animation.TextureRectangle,
					Color.White);
			}
			else
			{ 
				spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Width * Game1.Scale, Height * Game1.Scale),
					Color.White);
			}
		}

		public void Move(float x, float y)
		{
			Position = new Vector2(Position.X + x, Position.Y + y);

			var animation = GetComponent<Animation>(ComponentType.Animation);
			if (animation == null)
				return;
			if (!Game1.SideScroller)
			{
				if (x > 0)
				{
					animation.ResetCounter(State.Walk, Direction.Right);
				}

				if (x < 0)
				{
					animation.ResetCounter(State.Walk, Direction.Left);
				}

				if (y > 0)
				{
					animation.ResetCounter(State.Walk, Direction.Down);
				}

				if (y < 0)
				{
					animation.ResetCounter(State.Walk, Direction.Up);
				}
			}
			else
			{
				if (x > 0)
				{
					animation.ResetCounter(State.Walk, Direction.Right);
				}

				if (x < 0)
				{
					animation.ResetCounter(State.Walk, Direction.Left);
				}
			}
		}
	}
}
