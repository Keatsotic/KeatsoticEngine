using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace KeatsoticEngine.Source.World.Components.Weapons
{
	class Shuriken : Component
	{
		private readonly GameObject _owner;
		private Entities _entities;
		private readonly Texture2D _texture;

		private int _speed = 4;
		private SpriteSheetAnimationFactory _animationFactory;

		private Vector2 Position { get; set; }

		public override ComponentType ComponentType => ComponentType.Shuriken;

		public Shuriken(Entities entities, GameObject owner, ContentManager content)
		{
			_entities = entities;
			_owner = owner;
			_texture = content.Load<Texture2D>("Textures/s_star");

			// create animation sprite
			var spriteWidth = 9;
			var spriteHeight = 9;
			_texture = content.Load<Texture2D>("Textures/s_star");
			var objectAtlas = TextureAtlas.Create("objectAtlas", _texture, spriteWidth, spriteHeight);
			_animationFactory = new SpriteSheetAnimationFactory(objectAtlas);

			_animationFactory.Add("Init", new SpriteSheetAnimationData(new[] { 0, 1 }, 0.05f, true));
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
		}

		public void FireProjectile(Transform transform, Direction direction, Vector2 position)
		{
			var x = 0;
			var y = 0;

			switch (direction)
			{
				case Direction.Right:
					x = _speed;
					y = 0;
					break;
				case Direction.Left:
					x = -_speed;
					y = 0;
					break;
				case Direction.Up:
					x = 0;
					y = -_speed;
					break;
				case Direction.Down:
					x = 0;
					y = _speed;
					break;
			}

			if (ManageInput.playerSpecial)
				_entities.AddEntities(new Projectile(_entities, _owner, new Vector2(x, y), position, 1, new AnimatedSprite(_animationFactory, "Init")));
		}
	}
}
