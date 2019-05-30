using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeatsoticEngine.Source.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeatsoticEngine.Source.World.Components
{
	class PlayerController : Component
	{
		private readonly float _speed = 3.0f;
		private readonly int _jumpHeight = 6;
		private bool _isJumping;

		public Direction Direction { get; private set; }
		public State CurrentState { get; set; }

		//player controls
		private bool _playerLeft;
		private bool _playerRight;
		private bool _playerUp;
		private bool _playerDown;
		private bool _playerJump;
		private bool _playerJumpCancel;
		private bool _playerAttack;
		private bool _playerMenu;
		private bool _playerSpecial;


		public override ComponentType ComponentType => ComponentType.PlayerController;

		public PlayerController()
		{
		}

		public override void Update(GameTime gameTime)
		{
			PlayerInput();

			var transform = GetComponent<Transform>(ComponentType.Transform);
			var collision = GetComponent<Collision>(ComponentType.Collision);
			if (transform == null)
				return;

			StateMachine(CurrentState, transform, collision);

			if (_playerAttack)
			{
				CurrentState = State.Attack;
			}

		}
		public override void Draw(SpriteBatch spriteBatch)
		{ }

		#region Finite States

		private void StateMachine(State currentState, Transform transform, Collision collision)
		{
			switch (currentState)
			{
				case State.Idle:
					Idle(transform);
					break;
				case State.Walk:
					Walk(transform);
					break;
				case State.Jump:
					Jump(transform);
					break;
				case State.WallJump:
					//WallJump();
					break;
				case State.Fall:
					Fall(transform);
					break;
				case State.Attack:
					Attack(transform);
					break;
				case State.DuckAttack:
					//DuckAttack();
					break;
				case State.Duck:
					Duck(collision);
					break;
				case State.Hurt:
					//Hurt();
					break;
				case State.Dead:
					//Dead();
					break;
				default:
					CurrentState = State.Idle;
					break;
			}
		}

		private void Idle(Transform transform)
		{
			//switch to walk
			if (_playerLeft || _playerRight)
			{
				CurrentState = State.Walk;
			}
			//switch to jump
			if (_playerJump && transform.IsOnGround)
			{
				transform.Velocity.Y = 0;
				_isJumping = true;
				CurrentState = State.Jump;
			}
			//switch to duck
			if (_playerDown)
			{
				CurrentState = State.Duck;
			}
			if (!transform.IsOnGround)
			{
				//transform.Velocity.Y = 0;
				CurrentState = State.Fall;
			}
		}

		private void Walk(Transform transform)
		{
			if (_playerLeft)
			{
				transform.Move(-_speed, transform.Velocity.Y);
				Direction = Direction.Left;
			}
			if (_playerRight)
			{
				transform.Move(_speed, transform.Velocity.Y);
				Direction = Direction.Right;
			}
			if (!transform.IsOnGround)
			{
				transform.Velocity.Y = 0;
				CurrentState = State.Fall;
			}

			//check to see if we are moving
			if (!_playerLeft && !_playerRight)
			{
				CurrentState = State.Idle;
			}

			//switch to jump
			if (_playerJump && transform.IsOnGround)
			{
				transform.Velocity.Y = 0;
				CurrentState = State.Jump;
			}
		}

		private void Jump(Transform transform)
		{
			if(_isJumping)
			{
				transform.Velocity = new Vector2(transform.Velocity.X, -_jumpHeight);
				_isJumping = false;
			}
			if (_playerJumpCancel)
			{
				transform.Velocity = new Vector2(transform.Velocity.X, -1);
				CurrentState = State.Fall;
			}

			if (transform.Velocity.Y > 0)
			{
				CurrentState = State.Fall;

			}

			if (_playerLeft)
			{
				transform.Move(-_speed, transform.Velocity.Y);
				Direction = Direction.Left;
			}
			if (_playerRight)
			{
				transform.Move(_speed, transform.Velocity.Y);
				Direction = Direction.Right;
			}
		}

		private void Fall(Transform transform)
		{
			if (transform.IsOnGround == true)
			{
				CurrentState = State.Idle;
				_isJumping = true;
			}

			if (_playerLeft)
			{
				transform.Move(-_speed, transform.Velocity.Y);
				Direction = Direction.Left;
			}
			if (_playerRight)
			{
				transform.Move(_speed, transform.Velocity.Y);
				Direction = Direction.Right;
			}
		}

		private void Attack(Transform transform)
		{
			if (!transform.IsOnGround)
			{
				if (_playerLeft)
				{
					transform.Move(-_speed, transform.Velocity.Y);
					Direction = Direction.Left;
				}
				if (_playerRight)
				{
					transform.Move(_speed, transform.Velocity.Y);
					Direction = Direction.Right;
				}

				transform.Move(transform.Velocity.X, transform.Velocity.Y);
				_isJumping = true;
			}
		}

		private void Duck(Collision collision)
		{

			collision.BoundingBoxSetter = new Rectangle(20, 17, 13, 18);

			if (!_playerDown)
			{
				collision.BoundingBoxSetter = new Rectangle(20, 11, 13, 24);
				CurrentState = State.Idle;
			}
		}

		//controller inputs

		private void PlayerInput()
		{
			ManageInput.Update();

			// check for directional movement
			_playerLeft = ManageInput.IsKeyDown(Keys.Left) == true || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed;
			_playerRight = ManageInput.IsKeyDown(Keys.Right) == true || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed;

			_playerUp = ManageInput.IsKeyDown(Keys.Up) == true || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed;
			_playerDown = ManageInput.IsKeyDown(Keys.Down) == true || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed;


			//check for button presses
			_playerAttack = ManageInput.KeyPressed(Keys.V) == true || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed;
			_playerJump = ManageInput.KeyPressed(Keys.Space) == true || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed;
			_playerJumpCancel = ManageInput.IsKeyUp(Keys.Space) == true || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed;

			_playerSpecial = ManageInput.KeyPressed(Keys.B) == true || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed;
			_playerMenu = ManageInput.KeyPressed(Keys.Enter) == true || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed;
		}
	}
	#endregion
}
