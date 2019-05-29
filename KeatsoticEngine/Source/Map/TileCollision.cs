using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Map
{
	public class TileCollision
	{
		public int XPos { get; set; }
		public int YPos { get; set; }

		public Rectangle Rectangle { get { return new Rectangle(XPos * Tile._tileSize, YPos * Tile._tileSize, Tile._tileSize, Tile._tileSize); } }

		public bool Intersect(Rectangle rectangle)
		{
			return Rectangle.Intersects(rectangle);
		}

		public TileCollision()
		{

		}
	}
}
