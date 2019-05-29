using KeatsoticEngine.Source.UsrEventHandlers;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Manager
{
	class ManageInput
	{
		private KeyboardState _keyboardstate;
		private KeyboardState _lastKeyboardState;
		private Keys _lastKey;
		private static event EventHandler<NewEventInput> _newInput;
		private double _counter;
		private static double _cooldown;

		public static event EventHandler<NewEventInput> NewInput
		{
			add { _newInput += value; }
			remove{ _newInput -= value; }
		}

		public static bool ThrottleInput { get; set; }
		public static bool LockMovement { get; set; }

		public ManageInput()
		{
			ThrottleInput = false;
			LockMovement = false;
			_counter = 0;
		}

		public void Update(double gameTime)
		{
			if (_cooldown > 0)
			{
				_counter += gameTime;
				if (_counter > gameTime)
				{
					_cooldown = 0;
					_counter = 0;
				}
				else
				{
					return;
				}
			}
			ComputerControls(gameTime);
		}

		public void ComputerControls(double gameTime)
		{
			_keyboardstate = Keyboard.GetState();
			if (_keyboardstate.IsKeyUp(_lastKey) && _lastKey != Keys.None)
			{
				_newInput?.Invoke(this, new NewEventInput(Input.None));
			}

			CheckKeyState(Keys.Left, Input.Left);
			CheckKeyState(Keys.Right, Input.Right);

			if (!Game1.SideScroller)
			{
				CheckKeyState(Keys.Up, Input.Up);
				CheckKeyState(Keys.Down, Input.Down);
			}

			_lastKeyboardState = _keyboardstate;
		}

		private void CheckKeyState(Keys keys, Input newInput)
		{
			if (_keyboardstate.IsKeyDown(keys))
			{
				if(!ThrottleInput || (ThrottleInput && _lastKeyboardState.IsKeyUp(keys)))
				{
					if(_newInput != null)
					{
						_newInput(this, new NewEventInput(newInput));
						_lastKey = keys;
					}
				}
			}
		}
	}
}
