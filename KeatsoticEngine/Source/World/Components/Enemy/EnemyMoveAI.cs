using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


		public override ComponentType ComponentType => ComponentType.EnemyMoveAI;
		
		public EnemyMoveAI(float speed, PatrolType patrolType)
		{
			_speed = speed;
			PatrolType = patrolType;
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
	}
}
