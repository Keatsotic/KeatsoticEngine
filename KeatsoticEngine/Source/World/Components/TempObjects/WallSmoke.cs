using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World.Components.TempObjects
{
	class WallSmoke : GameObject
	{
		private Entities _entities;
		private Sprite _sprite;
		private AnimatedSprite _animation;

		public WallSmoke(Entities entities, Texture2D texture, Vector2 position, int width, int height)
		{
			AddComponent(new EfxGenerator(texture));
			_animation = GetComponent<EfxGenerator>(ComponentType.EfxGenerator).ObjectAnimated;
			_entities = entities;
			_animation.Position = position;
			_sprite = _animation;
			_sprite.Origin = new Vector2(0, -10);
		}

		public override void Update(GameTime gameTime)
		{
			_animation.Play("Smoke", () => _entities.RemoveEntities(this));
			_animation.Update(gameTime);
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_sprite);
		}


	}
}
