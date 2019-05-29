using KeatsoticEngine.Source;
using KeatsoticEngine.Source.World.Components;
using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeatsoticEngine
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		public static readonly bool SideScroller = false;
		public static readonly int Scale = 3;

		private GameObject _player;
		private ManageInput _manageInput;
		private ManageMap _manageMap;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1920;
			graphics.PreferredBackBufferHeight = 1080;

			_player = new GameObject();
			_manageInput = new ManageInput();
			_manageMap = new ManageMap("test");
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			_manageMap.LoadContent(Content);
			_player.AddComponent(new Sprite(Content.Load<Texture2D>("Textures/s_player_atlas"), 54, 35, new Vector2(100.0f, 100.0f)));
			_player.AddComponent(new PlayerInput());
			_player.AddComponent(new Animation(54, 35, 6));
			_player.AddComponent(new Collision(_manageMap));

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
			_manageInput.Update(gameTime.ElapsedGameTime.Milliseconds);
			_manageMap.Update(gameTime.ElapsedGameTime.Milliseconds);

			_player.Update(gameTime.ElapsedGameTime.Milliseconds);
			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			spriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.AlphaBlend, SamplerState.PointClamp);
			_manageMap.Draw(spriteBatch);
			_player.Draw(spriteBatch);
			spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
