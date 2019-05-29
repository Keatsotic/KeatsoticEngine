using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.UsrEventHandlers
{
	class NewEventInput
	{
		public Input Input { get; set; }
		public NewEventInput(Input input)
		{
			Input = input;
		}
	}
}
