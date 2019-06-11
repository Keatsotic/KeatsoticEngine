using KeatsoticEngine.Source.World.Components.TempObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class PlayerController : Component
	{
		private Entities _entities;

		private readonly float _speed = 1.6f;

		private readonly int _jumpHeight = 6;
		private bool _isJumping;
		private int _wallDir;
		private int _smokeTimer;

		private readonly int _attackTimerMax = 30;
		private int _attackTimer;

		private Rectangle _damageRect;
		private readonly GameObject _damage = new GameObject { Id = "Damage" };

		public static GameObject Player { get; private set; }

		public Direction Direction { get; private set; }
		public State CurrentState { get; set; }
		public State ReturnState { get; private set; }


		public override ComponentType ComponentType => ComponentType.PlayerController;

		public PlayerController(Entities entities, GameObject owner)
		{
			_entities = entities;
			Player = owner;
			CurrentState = HUD.PlayerCurrentState;
		}

		public override void Update(GameTime gameTime)
		{
			//access components
			var sprite = GetComponent<SpriteRenderer>(ComponentType.SpriteRenderer);
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var collision = GetComponent<Collision>(ComponentType.Collision);
			var animation = GetComponent<Animation>(ComponentType.Animation);
			var damage = GetComponent<Damage>(ComponentType.Damage);

			if (transform == null || sprite == null || collision == null) 
				return;


			Camera.Update(new Vector2(GetComponent<Transform>(ComponentType.Transform).Position.X + sprite.Width/2,
								  GetComponent<Transform>(ComponentType.Transform).Position.Y));

			if (ManageInput.playerAttack && _attackTimer <= 0)
			{
				switch (CurrentState)
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
				_attackTimer = _attackTimerMax;
			}
			_attackTimer--;

			
			StateMachine(CurrentState, transform, collision);


			if (animation.AnimationFinished == true)
			{
				CurrentState = ReturnState;
			}
			if (damage.IstakingDamage)
			{
				CurrentState = State.Hurt;
			}
			animation.CurrentState = CurrentState;

			if (CurrentState == State.WallJump)
			{
				var smokeAnimation = GetComponent<EfxGenerator>(ComponentType.EfxGenerator).ObjectTexture;
				if (_smokeTimer <= 0)
				{
					if (Direction == Direction.Left)
					{ _entities.AddEntities(new WallSmoke(_entities, smokeAnimation, new Vector2(transform.Position.X + 3, transform.Position.Y + 4), sprite.Width, sprite.Height)); }
					else
					{ _entities.AddEntities(new WallSmoke(_entities, smokeAnimation, new Vector2(transform.Position.X + 15, transform.Position.Y + 4), sprite.Width, sprite.Height)); }
					_smokeTimer = 15;
				}
				_smokeTimer--;
			}
		}


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
			if ((ManageInput.playerLeft && !ManageInput.playerRight) || (ManageInput.playerRight && !ManageInput.playerLeft))
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
			if (ManageInput.playerRight && ManageInput.playerLeft)
				CurrentState = State.Idle;

			//check to see if we are moving
			if (!ManageInput.playerLeft && !ManageInput.playerRight)
			{
				CurrentState = State.Idle;
			}

			//switch to jump
			if (ManageInput.playerJump && transform.IsOnGround)
			{
				transform.Velocity.Y = 0;
				_isJumping = true;
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

			if (Direction == Direction.Right)
			{
				_damageRect = new Rectangle((int)transform.Position.X + 40, (int)transform.Position.Y + 8, 16, 16);
			}
			else 
			{
				_damageRect = new Rectangle((int)transform.Position.X, (int)transform.Position.Y + 8, 16, 16);
			}

			if (_attackTimer > _attackTimerMax/2)
			{
				if (_entities.CheckCollision(_damageRect, _damage, out Direction _directionOut, out GameObject _enemyOut))
				{
					var hitEnemy = _enemyOut.GetComponent<Damage>(ComponentType.Damage);
					hitEnemy.TakingDamage(2);
				}
			}

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
			ReturnState = State.Duck;

			if (Direction == Direction.Right)
			{
				_damageRect = new Rectangle((int)transform.Position.X + 40, (int)transform.Position.Y + 18, 16, 16);
			}
			else
			{
				_damageRect = new Rectangle((int)transform.Position.X, (int)transform.Position.Y + 18, 16, 16);
			}

			if (_attackTimer > _attackTimerMax / 2)
			{
				if (_entities.CheckCollision(_damageRect, _damage, out Direction _directionOut, out GameObject _enemyOut))
				{
					var hitEnemy = _enemyOut.GetComponent<Damage>(ComponentType.Damage);
					hitEnemy.TakingDamage(2);
				}
			}
		}

		private void WallAttack(Transform transform, Collision collision)
		{

			ReturnState = State.WallJump;
			transform.Velocity.Y -= 0.4f;

			if (Direction == Direction.Right)
			{
				_damageRect = new Rectangle((int)transform.Position.X, (int)transform.Position.Y + 8, 16, 16);
			}
			else
			{
				_damageRect = new Rectangle((int)transform.Position.X + 40, (int)transform.Position.Y + 8, 16, 16);
			}

			if (_attackTimer > _attackTimerMax / 2)
			{
				if (_entities.CheckCollision(_damageRect, _damage, out Direction _directionOut, out GameObject _enemyOut))
				{
					var hitEnemy = _enemyOut.GetComponent<Damage>(ComponentType.Damage);
					hitEnemy.TakingDamage(2);
				}
			}

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
		{
			ReturnState = State.Idle;
		}

		private void Dead(Transform transform)
		{ }
		#endregion


		public override void Draw(SpriteBatch spriteBatch)
		{
#if DEBUG
			if (_attackTimer > _attackTimerMax / 2)
			{
				var collision = GetComponent<Collision>(ComponentType.Collision);
				spriteBatch.Draw(collision._bbTexture, _damageRect, new Color(255, 0, 0, 2));
			}
#endif
		}
		
	}


}
