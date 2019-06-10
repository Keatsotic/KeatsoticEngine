using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World.Components.Enemy
{
	class EnemyDeathAnimation : GameObject
	{
		private Vector2 _position;
		private Entities _entities;
		private Sprite _sprite;
		private AnimatedSprite _animation;

		public EnemyDeathAnimation(Entities entities, Vector2 position, AnimatedSprite animation, int width, int height)
		{
			_position = position;
			_entities = entities;
	
			_animation = animation;
			_sprite = _animation;
			_sprite.Origin = new Vector2(width/2, height/2);
		}

		public override void Update(GameTime gameTime)
		{
			_animation.Play("Death", () => _entities.RemoveEntities(this));
			_animation.Update(gameTime);
			_animation.Position = _position;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_sprite);
		}


	}
}
