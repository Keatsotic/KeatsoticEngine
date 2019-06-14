using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KeatsoticEngine.Source.Data.SaveLoad;

namespace KeatsoticEngine.Source.Data
{
	static class PlayerStats
	{
		static public int MaxHealth { get; set; }

		//Upgrades
		static public bool UpgrdStar { get; set; } // equipment, throwing star                       - level 1 (woods) - complete!
		static public bool UpgrdSlide { get; set; } //permanent equip, can slide faster              - level 2 (windmill) 
		static public bool UpgrdGrapple { get; set; } // equipment, grapling hookshot                - level 3 (train)
		static public bool UpgrdFire { get; set; } // equipment, shoots forward                      - level 4 (shrine)
		static public bool UpgrdBarrier { get; set; } // equipment, protects and hurts enemies       - level 5 (graveyard)
		static public bool UpgrdInvul { get; set; } // equipment, invincible for a time              - level 6 (dungeon)
		static public bool UpgrdHyper { get; set; } // permanent equip, makes all attacks stronger   - Final Level (castle)

		//Health upgrades
		static private bool Health1 { get; set; }
		static private bool Health2 { get; set; }
		static private bool Health3 { get; set; }
		static private bool Health4 { get; set; }
		static private bool Health5 { get; set; }

		private static List<bool> _saveList;


		static public void Initialize()
		{
			MaxHealth = 12;

			Load();

			UpgrdStar = _saveList[0];
			UpgrdSlide = _saveList[1];
			UpgrdGrapple = _saveList[2];
			UpgrdFire = _saveList[3];
			UpgrdBarrier = _saveList[4];
			UpgrdHyper = _saveList[5];

			//health
			Health1 = _saveList[6];
			Health2 = _saveList[7];
			Health3 = _saveList[8];
			Health4 = _saveList[9];
			Health5 = _saveList[10];

			UpgradeHealth();
		}

		static public void Save()
		{
			_saveList = new List<bool>
			{
					//upgrades
					UpgrdStar,
					UpgrdSlide,
					UpgrdGrapple,
					UpgrdFire,
					UpgrdBarrier,
					UpgrdHyper,

					//health
					Health1,
					Health2,
					Health3,
					Health4,
					Health5
			};

			XmlSerialization.WriteToXmlFile("FullMoon.txt", _saveList);
		}

		static public void Load()
		{
			var path = "FullMoon.txt";
			if (File.Exists(path))
			{
				_saveList = XmlSerialization.ReadFromXmlFile<List<bool>>("FullMoon.txt");
				UpgradeHealth();
			} 
			else
			{ 
				_saveList = new List<bool>
				{
					//upgrades
					false,
					false,
					false,
					false,
					false,
					false,

					//health
					false,
					false,
					false,
					false,
					false
				};
				XmlSerialization.WriteToXmlFile("FullMoon.txt", _saveList);
			}
		}

		static private void UpgradeHealth()
		{
			MaxHealth = 12;

			if (Health1) { MaxHealth += 1; }
			if (Health2) { MaxHealth += 1; }
			if (Health3) { MaxHealth += 1; }
			if (Health4) { MaxHealth += 1; }
			if (Health5) { MaxHealth += 1; }
		}

		static public void StartWithUpgrades()
		{
			_saveList = new List<bool>
			{
					//upgrades
					true,
					true,
					true,
					true,
					true,
					true,

					//health
					true,
					true,
					true,
					true,
					true
			};

			XmlSerialization.WriteToXmlFile("FullMoon.txt", _saveList);
		}
	}
}
