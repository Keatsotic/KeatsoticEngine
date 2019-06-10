using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World.Components.Enemy
{
	class SpawnController : Component
	{
		private Vector2 _spawnPosition;
		private GameObject _owner;

		public override ComponentType ComponentType => ComponentType.SpawnController;

		SpawnController(Vector2 position, GameObject owner)
		{
			_spawnPosition = position;
			_owner = owner;
		}

		
		public override void Update(GameTime gameTime)
		{
			var collision = GetComponent<Collision>(ComponentType.Collision);
			if (collision == null)
				return;

			if (!collision.CollisionBoundingBox.Intersects(Camera.ScreenRect))
			{ 
			
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}
