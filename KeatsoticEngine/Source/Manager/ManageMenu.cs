using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KeatsoticEngine.Source.Manager
{
	class ManageMenu
	{

		public bool MenuOpen { get; set; }

		private Texture2D _menuTexture;
		private int _index;
		private List<int> _equipmentArray;
		private SpriteFont _menuFont;

		public ManageMenu(ContentManager content)
		{
			_menuTexture = content.Load<Texture2D>("Textures/s_menu");
			_menuFont = content.Load<SpriteFont>("Fonts/f_menu");
		}

		public void Initialize()
		{
			_index = 0;
			_equipmentArray = new List<int>();
			MenuOpen = true;

			if (PlayerStats.UpgrdStar) { _equipmentArray.Add(0); } else { _equipmentArray.Add(-1); }
			if (PlayerStats.UpgrdFire) { _equipmentArray.Add(1); } else { _equipmentArray.Add(-1); }
			if (PlayerStats.UpgrdGrapple) { _equipmentArray.Add(2); } else { _equipmentArray.Add(-1); }
			if (PlayerStats.UpgrdBarrier) { _equipmentArray.Add(3); } else { _equipmentArray.Add(-1); }
			if (PlayerStats.UpgrdInvul) { _equipmentArray.Add(4); } else { _equipmentArray.Add(-1); }
		}

		public void Update(GameTime gameTime)
		{
			if (MenuOpen)
			{
				if (ManageInput.playerDownPressed)
				{
					_index++;
					if (_index > 5)
					{
						_index = 0;
					}
				}
				if (ManageInput.playerUpPressed)
				{
					_index--;
					if (_index < 0)
					{
						_index = 5;
					}
				}

				if (ManageInput.playerAttack)
				{
					switch (_index)
					{
						case 0:
							if (_equipmentArray[_index] != -1)
							{ HUD.EquippedItem = "Shuriken"; }
							break;
						case 1:
							if (_equipmentArray[_index] != -1)
							{ HUD.EquippedItem = "FlameStrike"; }
							break;
						case 2:
							if (_equipmentArray[_index] != -1)
							{ HUD.EquippedItem = "Grapple"; }
							break;
						case 3:
							if (_equipmentArray[_index] != -1)
							{ HUD.EquippedItem = "Barrier"; }
							break;
						case 4:
							if (_equipmentArray[_index] != -1)
							{ HUD.EquippedItem = "Invul"; }
							break;
						case 5:
							HUD.RestartGame();
							break;
					}
					PlayerController.Player.GetComponent<PlayerController>(ComponentType.PlayerController).EquipPlayerItem();
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(_menuFont, _index.ToString(), new Vector2(100, 100), Color.White);
			spriteBatch.Draw(_menuTexture, new Vector2(64, 48), Color.White);
		}
	}
}
