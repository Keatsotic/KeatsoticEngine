﻿using System;
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
	class ScreenStart : Screen
	{
		private Texture2D _texture;

		public AnimatedSprite ObjectAnimated { get; private set; }
		public AnimatedSprite ObjectSprite { get; private set; }
		private bool _canPressStart;

		public ScreenStart(ManageScreens manageScreens) : base(manageScreens)
		{
			Camera.cameraMax = Vector2.Zero + Camera.cameraOffset;
			Camera.cameraMin = Vector2.Zero + Camera.cameraOffset;
		}
		public override void LoadContent(ContentManager content)
		{
			_texture = content.Load<Texture2D>("Textures/s_start_screen");

			var spriteWidth = 480;
			var spriteHeight = 270;
			var objectTexture = _texture;
			var objectAtlas = TextureAtlas.Create("objectAtlas", objectTexture, spriteWidth, spriteHeight);

			var animationFactory = new SpriteSheetAnimationFactory(objectAtlas);

			animationFactory.Add("Fade", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 0.3f, false));
			animationFactory.Add("StartScreen", new SpriteSheetAnimationData(new[] { 10, 11, 12, 13 }, 0.3f));

			ObjectAnimated = new AnimatedSprite(animationFactory, "StartScreen");
			ObjectSprite = ObjectAnimated;

			ObjectSprite.Origin = Vector2.Zero;
			ObjectAnimated.Play("Fade", () => _canPressStart = true);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			Camera.Update(Vector2.Zero);
			ObjectAnimated.Update(gameTime);

			if (_canPressStart)
			{
				ObjectAnimated.Play("StartScreen");

				if (ManageInput.playerStart)
				{
					ManageScreens.LoadNewScreen(new ScreenWorld(ManageScreens, true), "Fading");
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(ObjectSprite);
		}
	}
}
