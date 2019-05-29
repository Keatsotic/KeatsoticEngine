using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class Collision : Component
	{
		private ManageMap _manageMap;

		public override ComponentType ComponentType => ComponentType.Collision;

		public Collision(ManageMap manageMap)
		{
			_manageMap = manageMap;
		}

		public bool CheckCollision(Rectangle rectangle, bool fixBox = true)
		{
			rectangle = new Rectangle((int)(rectangle.X + (rectangle.Width*.04)/2), (int)(rectangle.Y + rectangle.Height*0.5), (int)(rectangle.Width*0.6), (int)(rectangle.Height*0.5));
			return _manageMap.CheckCollision(rectangle);
		}

		public override void Update(double gameTime)
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}
