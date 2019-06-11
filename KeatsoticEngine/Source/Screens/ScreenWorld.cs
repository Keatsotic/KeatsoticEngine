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
		private bool _killPlayer;
		public ScreenWorld(ManageScreens manageScreens, bool killPlayer) : base(manageScreens)
		{
			_manageMap = new ManageMap(ManageMap.Level, manageScreens._graphics, manageScreens);
			_entities = new Entities();
			_killPlayer = killPlayer;
		}

		public override void Initialize()
		{

		}

		public override void LoadContent(ContentManager content)
		{
			_manageMap.LoadMap(_entities, content, _killPlayer, out _entities);
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
