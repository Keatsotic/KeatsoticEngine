using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components.Enemy
{
	class EnemyMoveAI : Component
	{
		private float _speed;
		private int _timer;
		private int _timerMax = 10;
		private PatrolType PatrolType;
		private int _counter;
		private Direction _currentDirection;
		private int _frequency;

		public override ComponentType ComponentType => ComponentType.MovementAI;
		
		public EnemyMoveAI(float speed, PatrolType patrolType)
		{
			_speed = speed;
			PatrolType = patrolType;
			_frequency = 200;
		}

		public override void Update(GameTime gameTime)
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var collision = GetComponent<Collision>(ComponentType.Collision);
			if (transform == null)
				return;

			switch(PatrolType)
			{
				case PatrolType.WallPatrol:
					WallPatrol();
					break;
				case PatrolType.Follow:
					Follow();
					break;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}

		public void WallPatrol()
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var collision = GetComponent<Collision>(ComponentType.Collision);

			if (transform.CheckWall(collision) != 0 && _timer <= 0)
			{
				_speed = -_speed;
				_timer = _timerMax;
			}
			_timer--;
			transform.Move(_speed, transform.Velocity.Y);
		}

		public void Follow()
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var player = PlayerController.Player;

			if (player == null)
				return;

			var playerPos =player.GetComponent<Transform>(ComponentType.Transform);
			var _x = 0f;

			if (playerPos.Position.X > transform.Position.X)
			{
				_x = _speed;
			}
			else
			{
				_x = -_speed;
			}

			transform.Move(_x, transform.Velocity.Y);
		}

		public void MoveRandom()
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var collision = GetComponent<Collision>(ComponentType.Collision);

			if (transform == null || collision == null)
				return;

			_counter += 1;

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

			switch (_currentDirection)
			{
				case Direction.None:
					_x = 0f;
					break;
				case Direction.Right:
					_x = _speed;
					break;
				case Direction.Left:
					_x = -_speed;
					break;
			}


			transform.Move(_x, transform.Velocity.Y);

		}

		public void ChangeDirection()
		{
			_counter = 0;
			_currentDirection = (Direction)ManageFunction.Random(0, 2);
		}
	}
}
