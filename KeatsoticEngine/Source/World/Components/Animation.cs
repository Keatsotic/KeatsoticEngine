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
		public State CurrentState { get; set; }
		public bool AnimationFinished { get; private set; }
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
			AnimationFinished = false;
			var damage = GetComponent<Damage>(ComponentType.Damage);
			var transform = GetComponent<Transform>(ComponentType.Transform);

		if (transform == null)
			return;

			_counter += gameTime.ElapsedGameTime.TotalMilliseconds;

			if (_counter > 50)
			{
				if (CurrentState.ToString().Contains("Attack") || CurrentState.ToString().Contains("Hurt"))
				{
					objectAnimated.Play(CurrentState.ToString(), () => AnimationFinished = true);
				}
				else
				{
					objectAnimated.Play(CurrentState.ToString());
				}

				_counter = 0;

				ChangeDirection();
			}


			objectAnimated.Update(gameTime);
			
			objectAnimated.Position = transform.Position;
			

			if (damage == null)
				return;
			if (damage.IsInvincible)
			{
				if (_counter % 2 == 0)
				{
					objectSprite.Color = new Color(0, 0, 0, 0);
				}
				else
				{
					objectSprite.Color = Color.White;
				}
			}
			else
			{
				objectSprite.Color = Color.White;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(objectSprite);

		}

		private void ChangeDirection()
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			if (!Game1.SideScroller)
			{
				switch (transform.Direction)
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
				switch (transform.Direction)
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
