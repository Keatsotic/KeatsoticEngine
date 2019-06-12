using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Map
{
	class TileCollisionLadder
	{
		public int XPos { get; set; }
		public int YPos { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }


		public Rectangle Rectangle { get { return new Rectangle(XPos, YPos, Width, Height); } }

		public bool Intersect(Rectangle rectangle)
		{
			return Rectangle.Intersects(rectangle);
		}

		public TileCollisionLadder(int x, int y, int w, int h)
		{
			XPos = x;
			YPos = y;
			Width = w;
			Height = h;
		}
	}
}
