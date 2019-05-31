using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class PlayerController : Component
	{
		private readonly float _speed = 2f;
		private readonly int _jumpHeight = 6;
		private bool _isJumping;
		private int _wallDir;
		private readonly int _attackTimerMax = 10;
		private int _attackTimer;

		public Direction Direction { get; private set; }
		public State CurrentState { get; set; }
		public State ReturnState { get; private set; }



		public override ComponentType ComponentType => ComponentType.PlayerController;

		public PlayerController()
		{
		}

		public override void Update(GameTime gameTime)
		{
			 
			//access components
			var sprite = GetComponent<SpriteRenderer>(ComponentType.SpriteRenderer);
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var collision = GetComponent<Collision>(ComponentType.Collision);
			if (transform == null)
				return;

			Camera.Update(new Vector2(GetComponent<Transform>(ComponentType.Transform).Position.X + sprite.Width/2,
								  GetComponent<Transform>(ComponentType.Transform).Position.Y));

			StateMachine(CurrentState, transform, collision);

			if (ManageInput.playerAttack && _attackTimer <= 0)
			{
				switch(CurrentState)
				{
					case State.Duck:
						CurrentState = State.DuckAttack;
						break;
					case State.WallJump:
						CurrentState = State.WallAttack;
						break;
					default:
						CurrentState = State.Attack;
						break;
				}
			}
			_attackTimer--;
		}
		public override void Draw(SpriteBatch spriteBatch)
		{ }

		#region Finite States

		private void StateMachine(State currentState, Transform transform, Collision collision)
		{
			switch (currentState)
			{
				case State.Idle:
					Idle(transform, collision);
					break;
				case State.Walk:
					Walk(transform);
					break;
				case State.Jump:
					Jump(transform, collision);
					break;
				case State.WallJump:
					WallJump(transform, collision);
					break;
				case State.Fall:
					Fall(transform, collision);
					break;
				case State.Attack:
					Attack(transform);
					break;
				case State.DuckAttack:
					DuckAttack(transform, collision);
					break;
				case State.WallAttack:
					WallAttack(transform, collision);
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

		private void Idle(Transform transform, Collision collision)
		{
			//switch to walk
			if (ManageInput.playerLeft || ManageInput.playerRight)
			{
				CurrentState = State.Walk;
			}
			//switch to jump
			if (ManageInput.playerJump && transform.IsOnGround)
			{
				transform.Velocity.Y = 0;
				_isJumping = true;
				CurrentState = State.Jump;
			}
			//switch to duck
			if (ManageInput.playerDown)
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
			if (ManageInput.playerLeft)
			{
				transform.Move(-_speed, transform.Velocity.Y);
				Direction = Direction.Left;
			}
			if (ManageInput.playerRight)
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
			if (!ManageInput.playerLeft && !ManageInput.playerRight)
			{
				CurrentState = State.Idle;
			}

			//switch to jump
			if (ManageInput.playerJump && transform.IsOnGround)
			{
				transform.Velocity.Y = 0;
				CurrentState = State.Jump;
			}
		}

		private void Jump(Transform transform, Collision collision)
		{
			if(_isJumping)
			{
				transform.Velocity = new Vector2(transform.Velocity.X, -_jumpHeight);
				_isJumping = false;
			}
			if (ManageInput.playerJumpCancel)
			{
				transform.Velocity = new Vector2(transform.Velocity.X, -1);
				CurrentState = State.Fall;
			}

			if (transform.Velocity.Y >= 0 || transform.CheckCeiling(collision))
			{
				transform.Velocity.Y = 0;
				CurrentState = State.Fall;
			}

			if (ManageInput.playerLeft)
			{
				transform.Move(-_speed, transform.Velocity.Y);
				Direction = Direction.Left;
			}
			if (ManageInput.playerRight)
			{
				transform.Move(_speed, transform.Velocity.Y);
				Direction = Direction.Right;
			}

		}

		private void Fall(Transform transform, Collision collision)
		{
			if (transform.IsOnGround)
			{
				CurrentState = State.Idle;
				_isJumping = true;
			}

			if (ManageInput.playerLeft)
			{
				transform.Move(-_speed, transform.Velocity.Y);
				Direction = Direction.Left;
			}
			if (ManageInput.playerRight)
			{
				transform.Move(_speed, transform.Velocity.Y);
				Direction = Direction.Right;
			}

			//walljump
			if (transform.CheckWall(collision) != 0 && (ManageInput.playerLeft || ManageInput.playerRight) && !transform.IsOnGround)
			{
				if ((Direction == Direction.Right && transform.CheckWall(collision) == 1) ||
				(Direction == Direction.Left && transform.CheckWall(collision) == -1))
				{
					_wallDir = -transform.CheckWall(collision);
					CurrentState = State.WallJump;
					transform.Velocity.Y = 0;
				}
			}
		}

		private void Duck(Collision collision)
		{
			ReturnState = State.Duck;

			collision.BoundingBoxSetter = new Rectangle(21, 17, 13, 18);

			if (!ManageInput.playerDown)
			{
				collision.BoundingBoxSetter = new Rectangle(21, 11, 13, 24);
				CurrentState = State.Idle;
			}
		}

		private void Attack(Transform transform)
		{
			ReturnState = State.Idle;
			_attackTimer = _attackTimerMax;

			if (!transform.IsOnGround)
			{
				if (ManageInput.playerLeft)
				{
					transform.Move(-_speed, transform.Velocity.Y);
					Direction = Direction.Left;
				}
				if (ManageInput.playerRight)
				{
					transform.Move(_speed, transform.Velocity.Y);
					Direction = Direction.Right;
				}

				transform.Move(transform.Velocity.X, transform.Velocity.Y);
				_isJumping = true;
			}
		}

		private void DuckAttack(Transform transform, Collision collision)
		{
			_attackTimer = _attackTimerMax;
		}

		private void WallAttack(Transform transform, Collision collision)
		{

			ReturnState = State.WallJump;
			_attackTimer = _attackTimerMax;
			transform.Velocity.Y -= 0.4f;
			_isJumping = true;
		}

		private void WallJump(Transform transform, Collision collision)
		{
			transform.Velocity.Y -= 0.4f;
			_isJumping = true;

			if ((Direction == Direction.Right && transform.CheckWall(collision) != 1) ||
				(Direction == Direction.Left && transform.CheckWall(collision) != -1))
			{
				CurrentState = State.Fall;
			}

			if (ManageInput.playerJump)
			{
				transform.Velocity = Vector2.Zero;
				transform.Velocity = new Vector2( 2 * _wallDir, 5f);
				CurrentState = State.Jump;

				Direction = Direction == Direction.Right ? Direction.Left : Direction.Right;
			}

			if ((!ManageInput.playerLeft && !ManageInput.playerRight) || (transform.CheckWall(collision) == _wallDir) || (transform.CheckWall(collision) == 0))
			{
				CurrentState = State.Fall;
			}

			if (transform.IsOnGround)
			{
				CurrentState = State.Idle;
			}
		}

		private void Hurt(Transform transform)
		{ }

		private void Dead(Transform transform)
		{ }

	}
	#endregion


}
