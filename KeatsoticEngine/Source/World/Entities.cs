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
	public class Entities
	{
		private List<GameObject> _entities;

		public Entities()
		{
			_entities = new List<GameObject>();
		}

		public void AddEntities(GameObject newEntity)
		{
			_entities.Add(newEntity);
		}

		public void RemoveEntities(GameObject destroyEntity)
		{
			_entities.Remove(destroyEntity);
		}

		public void Update(GameTime gameTime)
		{
			if (!ManageInput.GamePaused)
			{
				foreach (var gameObject in _entities.ToList())
				{
					gameObject.Update(gameTime);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach(var gameObject in _entities.ToList())
			{
				gameObject.Draw(spriteBatch);
			}
		}

		public bool CheckCollision(Rectangle rectangle, GameObject owner, out Direction direction, out GameObject objectHit)
		{
			foreach (var gameObject in _entities)
			{
				var collision = gameObject.GetComponent<Collision>(ComponentType.Collision);
				if (gameObject == owner || collision == null)
					continue;


				if (collision.CollisionBoundingBox.Intersects(rectangle))
				{
					if (owner.Id == "Player")
					{
						if (gameObject.GetComponent<Collision>(ComponentType.Collision).CollisionBoundingBox.X > rectangle.X)
						{
							direction = Direction.Right;
						}
						else
						{
							direction = Direction.Left;
						}
						objectHit = gameObject;
						return true;
					}
					else if ((owner.Id != gameObject.Id) && (owner.Id == "Damage"))
					{
						direction = Direction.None;
						objectHit = gameObject;
						return true;
					}
				}

			}
			direction = Direction.None;
			objectHit = null;
			return false;
		}
	}
}
