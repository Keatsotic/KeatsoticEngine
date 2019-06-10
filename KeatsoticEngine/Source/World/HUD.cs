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

		static private Texture2D _texture;
		static private Texture2D _healthUnit;


		static public void LoadContent(ContentManager content)
		{
			_texture = content.Load<Texture2D>("Textures/s_health_bar");
			_healthUnit = content.Load<Texture2D>("Textures/s_health_unit");
		}

		static public void Update(GameTime gameTime)
		{
			var _player = PlayerController.Player;
			if (_player == null)
				return;

			var _playerHealth = _player.GetComponent<Health>(ComponentType.Health).CurrentHealth;
			PlayerCurrentHealth = _playerHealth;
		}

		static public void Draw(SpriteBatch spriteBatch)
		{
			if (_texture == null || _healthUnit == null)
				return;

			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, ManageResolution.GetTransformationMatrix());
			spriteBatch.Draw(_texture, new Vector2(16, 32), Color.White);

			for (int i = 0; i < PlayerCurrentHealth; i++)
			{
				spriteBatch.Draw(_healthUnit, new Vector2(16 + (4), 49 + 32 - (4 * i)), Color.White);
			}


			spriteBatch.End();
		}
	}
}
