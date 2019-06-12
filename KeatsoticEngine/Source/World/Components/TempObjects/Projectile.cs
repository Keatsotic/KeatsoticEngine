using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;

namespace KeatsoticEngine.Source.World.Components.Weapons
{
	class Projectile : GameObject
	{
		private Entities _entities;
		private Vector2 _velocity;
		private Rectangle _damageRect;
		private AnimatedSprite _animation;
		private Sprite _sprite;
		private GameObject _owner;
		private Direction _direction;
		private GameObject _enemyOut;

		public Projectile(Entities entities, GameObject owner, Vector2 velocity, Vector2 position, int damageAmount, AnimatedSprite animation)
		{
			Id = "Damage";
			_entities = entities;
			_owner = owner;
			_velocity = velocity;
			_animation = animation;

			if (velocity.X < 0)
			{
				_animation.Effect = SpriteEffects.FlipHorizontally;
			}
			_animation.Position = position;

			_sprite = _animation;
			_sprite.Origin = Vector2.Zero;
		}

		public override void Update(GameTime gameTime)
		{
			_sprite.Position += _velocity;

			_animation.Play("Init");
			_animation.Update(gameTime);

			_damageRect = (Rectangle)_sprite.BoundingRectangle;

			if (_entities.CheckCollision(_damageRect, _owner, out _direction, out _enemyOut))
			{
				var hitEnemy = _enemyOut.GetComponent<Damage>(ComponentType.Damage);
				if (hitEnemy == null)
					return;

				hitEnemy.TakingDamage(1);
				_entities.RemoveEntities(this);
			}

			//remove if off camera
			if (!_damageRect.Intersects(Camera.ScreenRect))
			{
				_entities.RemoveEntities(this);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_sprite);
		}

	}
}
