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
	class FlameStrike : Equipment
	{
		private readonly GameObject _owner;
		private Entities _entities;
		private readonly Texture2D _texture;
		private Transform _transform;
		private int _fireTimer;
		private int _tailTimer;
		private int _counter;

		private int _speed = 4;
		private SpriteSheetAnimationFactory _animationFactory;
		private Vector2 _adjustPosition;
		private Vector2 Position { get;  set; }
		private Vector2 Velocity { get;  set; }

		public override ComponentType ComponentType => ComponentType.FlameStrike;

		public FlameStrike(Entities entities, GameObject owner, ContentManager content)
		{
			_entities = entities;
			_owner = owner;
			_texture = content.Load<Texture2D>("Textures/s_flame_strike");

			// create animation sprite
			var spriteWidth = 32;
			var spriteHeight = 32;
			var objectAtlas = TextureAtlas.Create("objectAtlas", _texture, spriteWidth, spriteHeight);
			_animationFactory = new SpriteSheetAnimationFactory(objectAtlas);

			_animationFactory.Add("Init", new SpriteSheetAnimationData(new[] { 0,1,2,3,4 }, 0.2f, true, false, true));

			
		}

		public override void Update(GameTime gameTime)
		{
			if (_tailTimer > 0)
			{
				_fireTimer--;
				FireTrail();
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
		}

		public override void ItemAction(Transform transform, Direction direction, Vector2 position, out int timer)
		{
			var x = 0;
			var y = 0;

			switch (direction)
			{
				case Direction.Right:
					x = _speed;
					y = 0;
					_adjustPosition = new Vector2(5, -16);
					break;
				case Direction.Left:
					x = -_speed;
					y = 0;
					_adjustPosition = new Vector2(-32, -16);
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

			position += _adjustPosition;
			Position = position;
			_transform = transform;

			if (ManageInput.playerSpecial)
			{
				Velocity = new Vector2(x, y);
				_entities.AddEntities(new Projectile(_entities, _owner, Velocity, Position, 2, new AnimatedSprite(_animationFactory, "Init"), 2, false));
				FireTrail();
				_fireTimer = 5;
				_tailTimer = 20;
				_counter = 6;
			}

			timer = 120;
		}

		private void FireTrail()
		{
			if (_fireTimer <= 0 && _counter >= 0)
			{
				Position = new Vector2(_transform.Position.X + 25, _transform.Position.Y + 18);
				Position += _adjustPosition;
				_entities.AddEntities(new Projectile(_entities, _owner, Velocity, Position, 1, new AnimatedSprite(_animationFactory, "Init"), 2, false));
				_fireTimer = 5;
				_counter--;
			}
		}
	}
}
