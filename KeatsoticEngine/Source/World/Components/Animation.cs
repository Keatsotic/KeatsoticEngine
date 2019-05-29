using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class Animation : Component
	{
		public override ComponentType ComponentType => ComponentType.Animation;

		private int _width;
		private int _height;
		public Rectangle TextureRectangle{ get; set; }
		private Direction _direction;
		private State _currentState;
		private double _counter;
		private int _animationIndex;
		private int _numOfFrames;

		public Animation(int width, int height, int frames)
		{
			_width = width;
			_height = height;
			_counter = 0;
			_animationIndex = 0;
			_currentState = State.Idle;
			_numOfFrames = frames;
		}
		
		public override void Update(double gameTime)
		{
			switch (_currentState)
			{
				case State.Idle:
					_animationIndex = 0;
					ChangeState();
					break;
				case State.Walk:
					_counter += gameTime;
					if (_counter > 70)
					{
						ChangeState();
						_counter = 0;
					}
					
				break;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}

		private void ChangeState()
		{
			if (!Game1.SideScroller)
			{
				switch (_direction)
				{
					case Direction.Up:
						TextureRectangle = new Rectangle(_width * _animationIndex, 0, _width, _height);
						break;
					case Direction.Down:
						TextureRectangle = new Rectangle(_width * _animationIndex, 0, _width, _height);
						break;
					case Direction.Left:
						TextureRectangle = new Rectangle(_width * _animationIndex, _height, _width, _height);
						break;
					case Direction.Right:
						TextureRectangle = new Rectangle(_width * _animationIndex, 0, _width, _height);
						break;
				}
			}
			else
			{
				switch (_direction)
				{
					case Direction.Right:
						TextureRectangle = new Rectangle(_width * _animationIndex, 0, _width, _height);
						break;
					case Direction.Left:
						TextureRectangle = new Rectangle(_width * _animationIndex, _height, _width, _height);
						break;
				}
			}

			//advance frames
			if (_animationIndex >= _numOfFrames)
			{
				_animationIndex = 1;
			} 
			else 
			{
				_animationIndex++;
			}
			_currentState = State.Idle;
			
		}

		public void ResetCounter(State state, Direction direction)
		{
			if (_direction != direction)
			{
				_counter = 800;
				_animationIndex = 1;
			}
			_currentState = state;
			_direction = direction;
		}
	}
}
