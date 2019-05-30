using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using KeatsoticEngine.Source.Data;
using MonoGame.Extended.Sprites;

namespace KeatsoticEngine.Source.World.Components
{
	class Animation : Component
	{
		public override ComponentType ComponentType => ComponentType.Animation;

		public Rectangle TextureRectangle{ get; set; }
		public Direction Direction { get; set; }
		public State CurrentState { get; set; }
		private double _counter;
		public readonly AnimatedSprite objectAnimated;
		public readonly Sprite objectSprite;


		public Animation(Texture2D texture, SpriteSheetData spriteSheetData)
		{
			var spriteWidth = spriteSheetData.Width;
			var spriteHeight = spriteSheetData.Height;
			var objectTexture = texture;
			var objectAtlas = TextureAtlas.Create("objectAtlas", objectTexture, spriteWidth, spriteHeight);

			var animationFactory = new SpriteSheetAnimationFactory(objectAtlas);
			for (int i = 0; i < spriteSheetData.AnimationName.Count; i++)
			{
				animationFactory.Add(spriteSheetData.AnimationName[i], new SpriteSheetAnimationData(spriteSheetData.FrameArray[i], spriteSheetData.FrameDuration[i], spriteSheetData.IsLooping[i]));
			}

			objectAnimated = new AnimatedSprite(animationFactory, "Idle");
			objectSprite = objectAnimated;
			objectSprite.Origin = Vector2.Zero;
		}
		
		public override void Update(GameTime gameTime)
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var owner = GetComponent<PlayerController>(ComponentType.PlayerController);

			if (owner == null)
			{
				//input = GetComponent<ENEMY AI COMPONENT FOR STATE INFO>()
			}

			_counter += gameTime.ElapsedGameTime.TotalMilliseconds;

			if (_counter > 50)
			{
				switch (owner.CurrentState)
				{
					case State.Idle:
						objectAnimated.Play("Idle");
						_counter = 0;
						break;
					case State.Walk:
						objectAnimated.Play("Walk");
						_counter = 0;
						break;
					case State.Jump:
						_counter = 0;
						objectAnimated.Play("Jump");
						break;
					case State.Fall:
						_counter = 0;
						objectAnimated.Play("Fall");
						break;
					case State.Duck:
						_counter = 0;
						objectAnimated.Play("Duck");
						break;
					case State.Attack:
						_counter = 0;
						objectAnimated.Play("Attack", ()=> owner.CurrentState = State.Idle);
						break;
				}
				ChangeDirection();
			}
			objectAnimated.Update(gameTime);
			objectAnimated.Position = transform.Position;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(objectSprite);

		}

		private void ChangeDirection()
		{
			var input = GetComponent<PlayerController>(ComponentType.PlayerController);
			if (!Game1.SideScroller)
			{
				switch (input.Direction)
				{
					case Direction.Up:
						break;
					case Direction.Down:
						break;
					case Direction.Left:
						objectAnimated.Effect = SpriteEffects.FlipHorizontally;
						break;
					case Direction.Right:
						objectAnimated.Effect = SpriteEffects.None;
						break;
				}
			}
			else
			{
				switch (input.Direction)
				{
					case Direction.Right:
						objectAnimated.Effect = SpriteEffects.None;
						break;
					case Direction.Left:
						objectAnimated.Effect = SpriteEffects.FlipHorizontally;
						break;
				}
			}
		}
	}
}
