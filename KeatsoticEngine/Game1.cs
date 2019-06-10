using KeatsoticEngine.Source;
using KeatsoticEngine.Source.World.Components;
using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KeatsoticEngine.Source.Data;
using System.Collections.Generic;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.Screens;
using System;

namespace KeatsoticEngine
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		public static bool SideScroller = true;

		private int _fpsCounter;
		private TimeSpan _counterElapsed = TimeSpan.Zero;

		private ManageScreens _manageScreens;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			ManageResolution.Init(ref graphics);
			ManageResolution.SetVirtualResolution(480, 270);
			ManageResolution.SetResolution(1920, 1080, false);

			//IsFixedTimeStep = false;
        }


        protected override void Initialize()
        {
			// TODO: Add your initialization logic here

			Camera.Initialize();
			base.Initialize();
        }

        protected override void LoadContent()
        {
			// TODO: use this.Content to load your game content here

			spriteBatch = new SpriteBatch(GraphicsDevice);
			_manageScreens = new ManageScreens(Content, graphics);
			_manageScreens.LoadNewScreen(new ScreenStart(_manageScreens));
		}

		protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{ Exit(); }

			// TODO: Add your update logic here
			_manageScreens.Update(gameTime);
			ManageInput.Update();
			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

			_fpsCounter++;
			_counterElapsed += gameTime.ElapsedGameTime;

			if (_counterElapsed >= TimeSpan.FromSeconds(1))
			{
#if DEBUG
				Window.Title = "FULL MOON " + _fpsCounter.ToString() + "fps - " + 
								(GC.GetTotalMemory(false) / 1048576f).ToString("F") + "MB"; 
#endif
				_fpsCounter = 0;
				_counterElapsed -= TimeSpan.FromSeconds(1);
			}

			// TODO: Add your drawing code here

			ManageResolution.BeginDraw();
			spriteBatch.Begin(SpriteSortMode.BackToFront,
								BlendState.AlphaBlend, 
								SamplerState.PointClamp, 
								null, 
								null, 
								null, 
								Camera.GetTransformMatrix());
			_manageScreens.Draw(spriteBatch);
			spriteBatch.End();
			HUD.Draw(spriteBatch);
			base.Draw(gameTime);
        }
	}
}
