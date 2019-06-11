using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World
{
	public static class HUD
	{
		static public int PlayerCurrentHealth { get; set; }
		static public int MaxHealth { get; set; }
		static public int Lives { get; set; }

		static public Vector2 PlayerCurrentPosition;
		static public Direction PlayerCurrentDirection { get; private set; }
		static public State PlayerCurrentState { get; private set; }


		static private Texture2D _texture;
		static private Texture2D _healthUnit;
		static private Texture2D _pauseTexture;
		static private int _alpha = 100;


		static public void LoadContent(ContentManager content)
		{
			_texture = content.Load<Texture2D>("Textures/s_health_bar");
			_healthUnit = content.Load<Texture2D>("Textures/s_health_unit");
			_pauseTexture = content.Load<Texture2D>("Textures/s_pixel");
		}

		static public void Initialize()
		{
			MaxHealth = 12;

			PlayerCurrentHealth = MaxHealth;
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
		}

		static public void Draw(SpriteBatch spriteBatch)
		{
			if (_texture == null || _healthUnit == null)
				return;
			var _player = PlayerController.Player;
			if (_player == null)
				return;

			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, ManageResolution.GetTransformationMatrix());
			spriteBatch.Draw(_texture, new Vector2(16, 32), Color.White);

			for (int i = 0; i < PlayerCurrentHealth; i++)
			{
				spriteBatch.Draw(_healthUnit, new Vector2(16 + (4), 49 + 32 - (4 * i)), Color.White);
			}

			if (ManageInput.GamePaused)
			{
				spriteBatch.Draw(_pauseTexture, new Rectangle(0,0, ManageResolution.VirtualWidth, ManageResolution.VirtualHeight), new Color(Color.Black, _alpha));
			}


			spriteBatch.End();
		}
	}
}
