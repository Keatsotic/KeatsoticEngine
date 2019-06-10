using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.World.Components;
using KeatsoticEngine.Source.World.Components.Enemy;
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
	public class CrawlerPrefab
	{
		public CrawlerPrefab(Entities entities, ManageMap manageMap, ContentManager content, Vector2 position, out Entities enemyAdded)
		{
			//add enemy
			var _enemy = new GameObject { Id = "Enemy" };
			_enemy.AddComponent(new Transform(position));
			_enemy.AddComponent(new SpriteRenderer(content.Load<Texture2D>("Textures/s_crawler"), 16, 16));
			_enemy.AddComponent(new Collision(manageMap, new Rectangle(0, 0, 16, 16), new Vector2(0, 0), content.Load<Texture2D>("Textures/s_pixel")));
			_enemy.AddComponent(new Animation(content.Load<Texture2D>("Textures/s_crawler"), (new SpriteSheetData
			(
				16,
				16,
				(new List<string> { "Idle", "Walk" }),
				(new List<int[]> { new[] { 0 }, new[] { 0, 1 } }),
				(new List<float> { 0.2f, 0.2f }),
				(new List<bool> { true, true }

			)))));
			_enemy.AddComponent(new MoveRandomAI(600, 1));
			_enemy.AddComponent(new Damage(entities, _enemy));
			_enemy.AddComponent(new Health(entities, _enemy, 2));
			_enemy.AddComponent(new EfxGenerator(content.Load<Texture2D>("Textures/s_efx")));
			entities.AddEntities(_enemy);
			enemyAdded = entities;

		}
	}
}
