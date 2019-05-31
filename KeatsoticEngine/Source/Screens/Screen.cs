using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Screens
{
	public abstract class Screen
	{
		protected ManageScreens ManageScreens;

		public Screen(ManageScreens manageScreens)
		{
			ManageScreens = manageScreens;
		}
		
		public virtual void Initialize() { }
		public virtual void Uninitialize() { }
		public abstract void LoadContent(ContentManager content);
		public abstract void Update(GameTime gameTime);
		public abstract void Draw(SpriteBatch spriteBatch);
	}
}
