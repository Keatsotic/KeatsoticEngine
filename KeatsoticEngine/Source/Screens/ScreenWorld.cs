using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.World.Components;
using KeatsoticEngine.Source.World.Components.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KeatsoticEngine.Source.Screens
{
	class ScreenWorld : Screen
	{
		private ManageMap _manageMap;
		private Entities _entities;


		public ScreenWorld(ManageScreens manageScreens) : base(manageScreens)
		{
			_manageMap = new ManageMap("m_level_1", manageScreens._graphics);
			_entities = new Entities();
		}

		public override void Initialize()
		{

		}

		public override void LoadContent(ContentManager content)
		{
			_manageMap.LoadContent(content);

			var _player = new GameObject("Player");
			var _enemy = new GameObject("Enemy");

			_player.AddComponent(new Transform(new Vector2(100.0f, 100.0f)));
			_player.AddComponent(new SpriteRenderer(content.Load<Texture2D>("Textures/s_player_atlas"), 54, 35));
			_player.AddComponent(new PlayerController());
			_player.AddComponent(new Animation(content.Load<Texture2D>("Textures/s_player_atlas"), (new SpriteSheetData
			(
				54,
				35,
				(new List<string> { "Idle", "Walk", "Jump", "Fall", "Duck", "Attack", "WallJump", "DuckAttack", "WallAttack" }),
				(new List<int[]> { new[] { 0 }, new[] { 1, 2, 3, 4, 5, 6 }, new[] { 7 }, new[] { 8 }, new[] { 9 }, new[] { 10, 11, 12 }, new[] { 18 }, new[] { 15, 16, 17 }, new[] { 19, 20, 21 } }),
				(new List<float> { 0.2f, 0.1f, 0.2f, 0.2f, 0.2f, 0.1f, 0.2f, 0.1f, 0.1f }),
				(new List<bool> { true, true, true, true, true, false, true, false, false }

			)))));
			_player.AddComponent(new Collision(_manageMap, new Rectangle(0, 0, 13, 24), new Vector2(21, 11), content.Load<Texture2D>("Textures/s_pixel")));

			_enemy.AddComponent(new Transform(new Vector2(100, 100)));
			_enemy.AddComponent(new SpriteRenderer(content.Load<Texture2D>("Textures/s_enemy"), 32, 32));
			_enemy.AddComponent(new Collision(_manageMap, new Rectangle(0, 0, 32, 32), new Vector2(0, 0), content.Load<Texture2D>("Textures/s_pixel")));
			_enemy.AddComponent(new EnemyBase());
			_enemy.AddComponent(new EnemyMoveAI(1.0f, PatrolType.WallPatrol));

			
			_entities.AddEntities(_enemy);
			_entities.AddEntities(_player);
		}

		public override void Update(GameTime gameTime)
		{
			_manageMap.Update(gameTime);
			_entities.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			_manageMap.Draw(spriteBatch);
			_entities.Draw(spriteBatch);
		}
	}
}
