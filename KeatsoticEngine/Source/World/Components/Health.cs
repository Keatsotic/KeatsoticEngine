using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeatsoticEngine.Source.World.Components.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class Health : Component
	{
		private int _maxHealth;
		private Entities _entities;
		private GameObject _owner;

		public int CurrentHealth { get; set; }

		public override ComponentType ComponentType => ComponentType.Health;

		public Health(Entities entities, GameObject owner, int maxHealth, int currentHealth)
		{
			_entities = entities;
			_owner = owner;
			_maxHealth = maxHealth;
			CurrentHealth = currentHealth;
		}

		public override void Update(GameTime gameTime)
		{
			if (_owner.Id == "Player")
				return;

			var transform = GetComponent<Transform>(ComponentType.Transform);
			if (CurrentHealth <= 0)
			{
				var animation = GetComponent<EfxGenerator>(ComponentType.EfxGenerator).ObjectAnimated;
				var sprite = GetComponent<SpriteRenderer>(ComponentType.SpriteRenderer);

				_entities.AddEntities(new EnemyDeathAnimation(_entities, transform.Position, animation, sprite.Width, sprite.Height));
				_entities.RemoveEntities(_owner);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}
