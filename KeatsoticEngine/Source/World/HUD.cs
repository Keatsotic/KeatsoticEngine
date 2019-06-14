using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace KeatsoticEngine.Source.World
{
	public static class HUD
	{
		static public int PlayerCurrentHealth { get; set; }
		
		static public int Lives { get; set; }

		static public Vector2 PlayerCurrentPosition;
		static public Direction PlayerCurrentDirection { get; private set; }
		static public State PlayerCurrentState { get; private set; }

		static private ManageMenu _manageMenu;

		static private Texture2D _healthBar;
		private static Texture2D _healthBarExtender;
		static private Texture2D _healthUnit;
		static private Texture2D _pauseTexture;
		static private int _alpha = 100;

		//Equipment
		public static string EquippedItem = null;


		static public void LoadContent(ContentManager content)
		{
			_healthBar = content.Load<Texture2D>("Textures/s_health_bar");
			_healthBarExtender = content.Load<Texture2D>("Textures/s_health_bar_extender");
			_healthUnit = content.Load<Texture2D>("Textures/s_health_unit");
			_pauseTexture = content.Load<Texture2D>("Textures/s_pixel");
			_manageMenu = new ManageMenu(content);
		}

		static public void Initialize()
		{
			PlayerCurrentHealth = PlayerStats.MaxHealth;
		}

		static public void Update(GameTime gameTime)
		{
			var _player = PlayerController.Player;
			if (_player == null)
				return;
			
			var _playerHealth = _player.GetComponent<Health>(ComponentType.Health).CurrentHealth;
			var _playerTransform = _player.GetComponent<Transform>(ComponentType.Transform).Position;
			var _playerController = _player.GetComponent<PlayerController>(ComponentType.PlayerController);

			PlayerCurrentHealth = _playerHealth;
			PlayerCurrentPosition = _playerTransform;
			PlayerCurrentDirection = _playerController.Direction;
			PlayerCurrentState = _playerController.CurrentState;

			//menu
			if (ManageInput.GamePaused)
			{
				_manageMenu.Update(gameTime);

				if (!_manageMenu.MenuOpen)
					_manageMenu.Initialize();
			}
			else
			{
				_manageMenu.MenuOpen = false;
			}
			
		}

		static public void Draw(SpriteBatch spriteBatch)
		{
			if (_healthBar == null || _healthUnit == null)
				return;
			var _player = PlayerController.Player;
			if (_player == null)
				return;

			spriteBatch.Begin(SpriteSortMode.FrontToBack, 
							  BlendState.AlphaBlend, 
							  SamplerState.PointClamp, 
							  null, 
							  null,
							  null, 
							  ManageResolution.GetTransformationMatrix());
			
			spriteBatch.Draw(_healthBarExtender, new Vector2(16, 40 - (4 * (PlayerStats.MaxHealth - 12))), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f );
			spriteBatch.Draw(_healthBar, new Vector2(16, 40), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, .1f);


			for (int i = 0; i < PlayerCurrentHealth; i++)
			{
				spriteBatch.Draw(_healthUnit, new Vector2(16 + (4), 49 + 40 - (4 * i)), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
			}

			if (ManageInput.GamePaused)
			{
				spriteBatch.Draw(_pauseTexture, new Rectangle(0,0, ManageResolution.VirtualWidth, ManageResolution.VirtualHeight), new Color(Color.Black, _alpha));
			}

			if (ManageInput.GamePaused)
			{
				_manageMenu.Draw(spriteBatch);
			}

			spriteBatch.End();
		}

		static private void PlayerDeath()
		{
			if (PlayerCurrentHealth <= 0)
			{
				PlayerStats.Save();
			}
		}

		static public void RestartGame()
		{
			PlayerStats.Save();
			_manageMenu.MenuOpen = false;
			Game1.RestartGame = true;
		}
	}
}
