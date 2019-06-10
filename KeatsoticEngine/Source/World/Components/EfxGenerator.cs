using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeatsoticEngine.Source.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;

namespace KeatsoticEngine.Source.World.Components
{
	class EfxGenerator : Component
	{
		public override ComponentType ComponentType => ComponentType.EfxGenerator;

		public AnimatedSprite ObjectAnimated { get; }
		public AnimatedSprite ObjectSprite { get; }
		public Texture2D ObjectTexture { get; private set; }

		public EfxGenerator(Texture2D texture)
		{
			var spriteWidth = 32;
			var spriteHeight = 32;
			ObjectTexture = texture;
			var objectAtlas = TextureAtlas.Create("objectAtlas", ObjectTexture, spriteWidth, spriteHeight);

			var animationFactory = new SpriteSheetAnimationFactory(objectAtlas);
	
			animationFactory.Add("Init", new SpriteSheetAnimationData(new[] { 0 }, 0.1f, false));
			animationFactory.Add("Death", new SpriteSheetAnimationData(new[] {1, 2, 3, 4 }, 0.05f, false));
			animationFactory.Add("Smoke", new SpriteSheetAnimationData(new[] { 5, 6, 7, 8, 9, 10 }, 0.12f, false));


			ObjectAnimated = new AnimatedSprite(animationFactory, "Init");
			ObjectSprite = ObjectAnimated;
			ObjectSprite.Origin = Vector2.Zero;
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}
	}
}
