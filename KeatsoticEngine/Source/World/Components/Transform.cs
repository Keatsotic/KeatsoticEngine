﻿using System;
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
		public Vector2 Velocity = Vector2.Zero;
		private float _accel;
		private float _friction;
		private float _gravity;
		private int _terminalVelocity = 8;

		public override ComponentType ComponentType => ComponentType.Transform;

		public Transform(Vector2 position, float acceleration = 0.1f, float friction = 0.2f, float gravity = 0.3f)
		{
			Position = position;
			_accel = acceleration;
			_friction = friction;
			_gravity = gravity;
			IsOnGround = false;
		}
		public override void Update(GameTime gameTime)
		{
			ApplyGravity();
			var collision = GetComponent<Collision>(ComponentType.Collision);

			//check for ground
			IsOnGround = CheckGround(collision);
			//move the player
			collision.CollisionManager(Velocity.X, Velocity.Y, this);
			//apply friction
			Velocity.X = MathHelper.Lerp(Velocity.X, 0, _friction);
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
			}
			if (y != 0)
			{
				Velocity.Y += (_accel + _friction) * Math.Sign(y);
				if (Math.Abs(Velocity.Y) > Math.Abs(y))
				{
					Velocity.Y = y;
				}
			}
		}

		public void ApplyGravity()
		{
			if (!IsOnGround)
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
			if (collision.CheckCollision(new Rectangle((int)(Position.X + collision.BoundingBoxSetter.X), (int)(Position.Y + collision.BoundingBoxSetter.Y +1), collision.BoundingBoxSetter.Width, collision.BoundingBoxSetter.Height)))
			{ return true; }
			else 
			{ return false; }
		}
	}
}