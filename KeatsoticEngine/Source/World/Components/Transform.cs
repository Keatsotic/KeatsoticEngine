using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class Transform : Component
	{

		public Vector2 Position;
		public bool IsOnGround { get; set; }
		public Direction Direction { get; private set; }
		public int IsOnWall { get; private set; }
		public Vector2 Velocity = Vector2.Zero;
		private float _accel;
		private float _friction;
		private float _gravity;
		private int _terminalVelocity = 8;

		public override ComponentType ComponentType => ComponentType.Transform;

		public Transform(Vector2 position, int direction = 0, float acceleration = 0.1f, float friction = 0.2f, float gravity = 0.4f)
		{
			Position = position;
			_accel = acceleration;
			_friction = friction;
			_gravity = gravity;
			Direction = (Direction)direction;
			IsOnGround = false;
		}
		public override void Update(GameTime gameTime)
		{
			ApplyGravity();
			var collision = GetComponent<Collision>(ComponentType.Collision);
			CheckWall(collision);
			//apply friction
			Velocity.X = TendToZero(Velocity.X, _friction);

			//check for ground
			IsOnGround = CheckGround(collision);
			//move the player
			collision.CollisionManager(Velocity.X, Velocity.Y, this);
			
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{}

		public void Move(float x, float y)
		{
			//acts like a rigidbody, velocity and gravity handled here
			if (x != 0)
			{
				Velocity.X += (_accel + _friction) * Math.Sign(x);
				if (Math.Abs(Velocity.X) > Math.Abs(x))
				{
					Velocity.X = x;
				}
				if (x > 0) 
				{
					Direction = Direction.Right;
				}
				else 
				{
					Direction = Direction.Left;
				}
			}
			if (y != 0)
			{
				Velocity.Y += (_accel + _friction) * Math.Sign(y);
				if (Math.Abs(Velocity.Y) > Math.Abs(y))
				{
					Velocity.Y = y;
				}
				if (!Game1.SideScroller)
				{
					if (y < 0) 
					{
						Direction = Direction.Up;
					}
					else 
					{
						Direction = Direction.Down;
					}
				}
			}
		}

		//move function for platforms transistions ladders
		public void Move(Direction direction, float x, float y)
		{
			switch(direction)
			{
				case Direction.Right:
					Position.X += x;
					break;
				case Direction.Left:
					Position.X -= x;
					break;
				case Direction.Up:
					Position.Y -= y;
					break;
				case Direction.Down:
					Position.Y += y;
					break;
			}
		}

		//knock back movement
		public void Move(Direction direction, Vector2 knockback)
		{
		//directions are reversed
			switch (direction)
			{
				case Direction.Right:
					Velocity.X = -knockback.X;
					break;
				case Direction.Left:
					Velocity.X = knockback.X;
					break;
				case Direction.Up:
					Velocity.Y = knockback.X;
					break;
				case Direction.Down:
					Velocity.Y = knockback.Y;
					break;
			}
		}

		public void ApplyGravity()
		{
			if (!IsOnGround && ManageInput.CanPressButtons)
			{
				Velocity.Y += _gravity;

				if (Velocity.Y > _terminalVelocity)
				{
					Velocity.Y = _terminalVelocity;
				}
			}
		}

		public bool CheckGround(Collision collision)
		{
			if (collision.CheckCollision(new Rectangle((int)(Position.X + collision.BoundingBoxSetter.X), 
					(int)(Position.Y + collision.BoundingBoxSetter.Y +1), collision.BoundingBoxSetter.Width,
					collision.BoundingBoxSetter.Height)))
			{ return true; }
			else 
			{ return false; }
		}

		public bool CheckCeiling(Collision collision)
		{
			if (collision.CheckCollision(new Rectangle((int)(Position.X + collision.BoundingBoxSetter.X),
					(int)(Position.Y + collision.BoundingBoxSetter.Y - 1), collision.BoundingBoxSetter.Width,
					collision.BoundingBoxSetter.Height)))
			{ return true; }
			else
			{ return false; }
		}

		public int CheckWall(Collision collision)
		{
			if (collision.CheckCollision(new Rectangle((int)(Position.X + collision.BoundingBoxSetter.X + 1), 
					(int)(Position.Y + collision.BoundingBoxSetter.Y), collision.BoundingBoxSetter.Width, 
					collision.BoundingBoxSetter.Height/2)))
			{ 
				return 1; 
			}

			else if (collision.CheckCollision(new Rectangle((int)(Position.X + collision.BoundingBoxSetter.X - 1), 
					(int)(Position.Y + collision.BoundingBoxSetter.Y), collision.BoundingBoxSetter.Width, 
					collision.BoundingBoxSetter.Height/2)))
			{ 
				return -1; 
			}

			else
			{ 
				return 0 ; 
			}
		}

		public float TendToZero(float val, float amount)
		{
			if (val > 0.0f && (val -= amount) < 0.0f) return 0f;
			if (val < 0.0f && (val += amount) > 0.0f) return 0f;
			return val;
		}
	}
}
