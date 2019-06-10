using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class Damage : Component
	{
		public  bool IstakingDamage{ get; private set; }
		public bool IsInvincible { get; private set; }
		private Entities _entities;
		private Direction _direction;
		private GameObject _target;
		private GameObject _owner;
		private int _counter;
		private int _invincibleTimer;
		private int _invincibleTimerMax = 80;

		public override ComponentType ComponentType => ComponentType.Damage;


		public Damage(Entities entities, GameObject owner)
		{
			IstakingDamage = false;
			_entities = entities;
			_owner = owner;
			IsInvincible = false;
			if (_owner.Id != "Player") { _invincibleTimerMax = 20; }
		}

		public override void Update(GameTime gameTime)
		{
			var collision = GetComponent<Collision>(ComponentType.Collision);
			var transform = GetComponent<Transform>(ComponentType.Transform);

			if (collision == null)
				return;

			if (_entities.CheckCollision(collision.CollisionBoundingBox, _owner, out _direction, out _target) && _invincibleTimer <= 0)
			{
				TakingDamage(1);
			}

			if (IstakingDamage)
			{
				transform.Move(_direction, new Vector2(2.5f, 0));
				_counter += 1;
				
				if (_counter > 10)
				{
					IstakingDamage = false;
					_counter = 0;
				}
			}

			if (_invincibleTimer > 0)
			{
				IsInvincible = true;
				_invincibleTimer--;
			} 
			else
			{
				IsInvincible = false;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{ }

		public void TakingDamage(int damageToTake)
		{
			if (!IstakingDamage && _invincibleTimer <=0)
			{
				_invincibleTimer = _invincibleTimerMax;
				IstakingDamage = true;
				HurtEntity(damageToTake);
			}
		}

		public void HurtEntity(int damageToTake)
		{
			var health = GetComponent<Health>(ComponentType.Health);
			if (health == null)
				return;

			health.CurrentHealth -= damageToTake;
		}
	}
}
