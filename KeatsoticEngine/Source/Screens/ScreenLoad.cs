using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Sprites;

namespace KeatsoticEngine.Source.Screens
{
	class ScreenLoad : Screen
	{
		private Texture2D _texture;

		public AnimatedSprite ObjectAnimated { get; private set; }
		public AnimatedSprite ObjectSprite { get; private set; }
		private bool _canPressStart;
		private SpriteFont _font;
		private int _timer = 60;

		public ScreenLoad(ManageScreens manageScreens) : base(manageScreens)
		{
			Camera.cameraMax = Vector2.Zero + Camera.cameraOffset;
			Camera.cameraMin = Vector2.Zero + Camera.cameraOffset;
		}
		public override void LoadContent(ContentManager content)
		{
			_texture = content.Load<Texture2D>("Textures/s_load_screen");
			_font = content.Load<SpriteFont>("Fonts/f_menu");

			var spriteWidth = 480;
			var spriteHeight = 270;
			var objectTexture = _texture;
			var objectAtlas = TextureAtlas.Create("objectAtlas", objectTexture, spriteWidth, spriteHeight);

			var animationFactory = new SpriteSheetAnimationFactory(objectAtlas);

			
			animationFactory.Add("LoadScreen", new SpriteSheetAnimationData(new[] { 0 }, 0.3f));

			ObjectAnimated = new AnimatedSprite(animationFactory, "LoadScreen");
			ObjectSprite = ObjectAnimated;

			ObjectSprite.Origin = Vector2.Zero;
			
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			Camera.Update(Vector2.Zero);
			ObjectAnimated.Update(gameTime);

			if (_timer <= 0)
				_canPressStart = true;


			if (_canPressStart)
			{
				if (ManageInput.playerStart)
				{
					ManageScreens.LoadNewScreen(new ScreenWorld(ManageScreens, true), "Fading");
				}
			}
			_timer--;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw(ObjectSprite);
			spriteBatch.DrawString(_font, "Continue", new Vector2(170, 80), Color.White);
			spriteBatch.DrawString(_font, "New Game", new Vector2(170, 130), Color.White);
			spriteBatch.DrawString(_font, "Options", new Vector2(170, 180), Color.White);
		}
	}
}
