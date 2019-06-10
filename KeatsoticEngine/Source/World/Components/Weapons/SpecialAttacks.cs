using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World.Components.Weapons
{
	abstract class SpecialAttacks : GameObject
	{
		GameObject _owner;

		public SpecialAttacks(GameObject owner)
		{
			_owner = owner; 
		}

		public abstract void Action();

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}
	}
}
