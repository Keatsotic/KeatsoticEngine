using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World
{
	class Entities
	{
		private List<GameObject> _entities;

		public Entities()
		{
			_entities = new List<GameObject>();
		}

		public void CreatePlayer(Vector2 position)
		{
			
		}

		public void AddEntities(GameObject newEntity)
		{
			_entities.Add(newEntity);
		}

		public void Update(GameTime gameTime)
		{
			foreach (var gameObject in _entities)
			{
				gameObject.Update(gameTime);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach(var gameObject in _entities)
			{
				gameObject.Draw(spriteBatch);
			}
		}
	}
}
