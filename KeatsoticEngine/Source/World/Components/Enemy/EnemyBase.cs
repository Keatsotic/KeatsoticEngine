using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components.Enemy
{
	class EnemyBase : Component
	{
		public override ComponentType ComponentType => ComponentType.EnemyBase;

		public override void Update(GameTime gameTime)
		{
			var sprite = GetComponent<SpriteRenderer>(ComponentType.SpriteRenderer);
			var transform = GetComponent<Transform>(ComponentType.Transform);
			if (sprite == null || transform == null)
				return;

			var enemyRect = new Rectangle((int)(transform.Position.X), (int)(transform.Position.Y), sprite.Width, sprite.Height);

			if(!enemyRect.Intersects(Camera.ScreenRect))
			{
				//System.Diagnostics.Debug.WriteLine("enemy is off screen");
			}
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}
