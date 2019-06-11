﻿using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Prefabs
{
	public class PlayerPrefab
	{
		public PlayerPrefab(Entities entities, ManageMap manageMap, ContentManager content, Vector2 position, out Entities outEntities) 
		{ 
			var _player = new GameObject { Id = "Player" };
			//add player
			_player.AddComponent(new Transform(position, (int)HUD.PlayerCurrentDirection));
			_player.AddComponent(new SpriteRenderer(content.Load<Texture2D>("Textures/s_player_atlas"), 54, 35));
			_player.AddComponent(new Animation(content.Load<Texture2D>("Textures/s_player_atlas"), (new SpriteSheetData
			(
				54,
				35,
				(new List<string> { "Idle", "Walk", "Jump", "Fall", "Duck", "Attack", "WallJump", "DuckAttack", "WallAttack", "Hurt"}),
				(new List<int[]> { new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 23, 22 }, new[] { 1, 2, 3, 4, 5, 6 }, new[] { 7 }, new[] { 8 }, new[] { 9 }, new[] { 11, 12, 12 }, new[] { 18 }, new[] { 15, 16, 17 }, new[] { 19, 20, 21 }, new[] { 8 } }),
				(new List<float> { 0.1f, 0.1f, 0.2f, 0.2f, 0.2f, 0.1f, 0.2f, 0.1f, 0.1f, 0.3f }),
				(new List<bool> { true, true, true, true, true, false, true, false, false, false }

			)))));
			_player.AddComponent(new Collision(manageMap, new Rectangle(0, 0, 13, 24), new Vector2(21, 11), content.Load<Texture2D>("Textures/s_pixel")));
			_player.AddComponent(new Damage(entities, _player));
			_player.AddComponent(new Health(entities, _player, HUD.MaxHealth, HUD.PlayerCurrentHealth));
			_player.AddComponent(new EfxGenerator(content.Load<Texture2D>("Textures/s_efx")));
			_player.AddComponent(new PlayerController(entities, _player));
			entities.AddEntities(_player);

			outEntities = entities;
		}
	}
}
