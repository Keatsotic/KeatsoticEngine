using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Manager
{
	public static class ManageFunction
	{
		private static Random _rnd = new Random();


		public static int Random(int min, int max)
		{
			return _rnd.Next(min, max + 1);
		}
	}
}
