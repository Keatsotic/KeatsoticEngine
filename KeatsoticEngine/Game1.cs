using KeatsoticEngine.Source;
using KeatsoticEngine.Source.World.Components;
using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KeatsoticEngine.Source.Data;
using System.Collections.Generic;

namespace KeatsoticEngine
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		public static readonly bool SideScroller = true;
		public static readonly int Scale = 1;
		public static int RoomNumber = 1;

		private GameObject _player;
		private ManageMap _manageMap;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			ManageResolution.Init(ref graphics);
			ManageResolution.SetVirtualResolution(480, 270);
			ManageResolution.SetResolution(1920, 1080, false);
			
			_player = new GameObject();
			_manageMap = new ManageMap("m_level_1", graphics);
        }


        protected override void Initialize()
        {
			// TODO: Add your initialization logic here
			Camera.Initialize();
			base.Initialize();
        }

        protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			_manageMap.LoadContent(Content);
			_player.AddComponent(new Transform(new Vector2(100.0f, 100.0f)));
			_player.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("Textures/s_player_atlas"), 54, 35));
			_player.AddComponent(new PlayerController());
			_player.AddComponent(new Animation(Content.Load<Texture2D>("Textures/s_player_atlas"), (new SpriteSheetData
			(
				54,
				35,
				(new List<string> { "Idle", "Walk", "Jump", "Fall", "Duck", "Attack" }),
				(new List<int[]> { new[] { 0 }, new[] { 1, 2, 3, 4, 5, 6 }, new[] { 7 }, new[] { 8 }, new[] { 9 }, new[] { 10, 11, 12 } }),
				(new List<float> { 0.2f, 0.1f, 0.2f, 0.2f, 0.2f, 0.1f }),
				(new List<bool> { true, true, true, true, true, false }

			)))));
			_player.AddComponent(new Collision(_manageMap, new Rectangle(0, 0, 13, 24), new Vector2(20, 11), Content.Load<Texture2D>("Textures/s_pixel")));

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

			// TODO: Add your update logic here
			_manageMap.Update(gameTime);
			_player.Update(gameTime);

			UpdateCamera();

			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			ManageResolution.BeginDraw();
			spriteBatch.Begin(SpriteSortMode.BackToFront,
								BlendState.AlphaBlend, 
								SamplerState.PointClamp, 
								null, 
								null, 
								null, 
								Camera.GetTransformMatrix());

			_manageMap.Draw(spriteBatch);
			_player.Draw(spriteBatch);

			spriteBatch.End();
            base.Draw(gameTime);
        }

		//camera methods
		private void UpdateCamera()
		{			
			Camera.Update(_player.GetComponent<Transform>(ComponentType.Transform).Position);
		}
	}
}
