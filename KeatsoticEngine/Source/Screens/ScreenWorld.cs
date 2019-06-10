using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.World.Components;
using KeatsoticEngine.Source.World.Components.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KeatsoticEngine.Source.Prefabs;

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
	
			_manageMap.LoadMap(_entities, content, out _entities);

			HUD.LoadContent(content);
		}

		public override void Update(GameTime gameTime)
		{
			HUD.Update(gameTime);
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
