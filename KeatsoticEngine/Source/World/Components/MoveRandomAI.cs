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
	class MoveRandomAI : Component
	{

		private readonly int _frequency;
		private int _counter;
		private readonly float _speed;
		private Direction _currentDirection;
		private State _currentState;
		 
		public override ComponentType ComponentType => ComponentType.MovementAI;

		public MoveRandomAI(int frequency, float speed)
		{
			_frequency = frequency * ManageFunction.Random(1, 4);
			_speed = (speed * ManageFunction.Random(1, 2) * 0.5f);
			ChangeDirection();
		}

		public override void Update(GameTime gameTime)
		{
			var sprite = GetComponent<SpriteRenderer>(ComponentType.SpriteRenderer);
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var collision = GetComponent<Collision>(ComponentType.Collision);
			var animation = GetComponent<Animation>(ComponentType.Animation);
			var damage = GetComponent<Damage>(ComponentType.Damage);
			if (sprite == null || transform == null || collision == null || animation == null)
				return;

			_counter += gameTime.ElapsedGameTime.Milliseconds;

			if (transform.CheckWall(collision) != 0 && _counter > _frequency)
			{
				_counter = 0;
				_currentDirection = _currentDirection == Direction.Right ? Direction.Left : Direction.Right;

				return;
			}

			if (_counter > _frequency)
			{
				ChangeDirection();
			}

			var _x = 0.0f;

			switch(_currentDirection)
			{
				case Direction.None:
					_currentState = State.Idle;
					_x = 0f;
					break;
				case Direction.Right:
					_currentState = State.Walk;
					_x = _speed;
					break;
				case Direction.Left:
					_currentState = State.Walk;
					_x = -_speed;
					break;
			}

			if (damage.IsInvincible)
			{
				_x = 0;
			}

			animation.CurrentState = _currentState;
			transform.Move(_x, transform.Velocity.Y);

		}

		public void ChangeDirection()
		{
			_counter = 0;
			_currentDirection = (Direction)ManageFunction.Random(0, 2);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}

	}
}
