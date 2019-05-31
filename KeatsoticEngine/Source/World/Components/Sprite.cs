using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class SpriteRenderer : Component
	{
		public  Texture2D Texture { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		

		public override ComponentType ComponentType => ComponentType.SpriteRenderer;

		public SpriteRenderer(Texture2D texture, int width, int height)
		{
			Texture = texture;
			Width = width;
			Height = height;
		}

		public override void Update(GameTime gameTime)
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var animation = GetComponent<Animation>(ComponentType.Animation);

			if (animation == null)
			{ 
				spriteBatch.Draw(Texture, new Rectangle((int)transform.Position.X, (int)transform.Position.Y, Width, Height),
					Color.White);
			}
		}

		
	}
}
