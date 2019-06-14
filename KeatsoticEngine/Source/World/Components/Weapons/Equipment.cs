using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World.Components.Weapons
{
	 abstract class Equipment : Component
	{

		public abstract void ItemAction(Transform transform, Direction direction, Vector2 position, out int timer);
	}
}
