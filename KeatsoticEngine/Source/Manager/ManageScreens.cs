using KeatsoticEngine.Source.Screens;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Manager
{
	public class ManageScreens
	{
		private Screen _lastScreen;
		private Screen _currentScreen;
		private ContentManager _content;
		private Texture2D _fadeRect;

		public GraphicsDeviceManager _graphics { get; private set; }
		private Screen _screen;

		//fade
		private bool _fading;
		private bool _fadingIn;
		private bool _fadingOut;
		private int _alpha;

		//slide
		private Direction _direction;
		private bool _sliding;
		private Vector2 _cameraSlide;
		private Vector2 _slideDir;

		private bool _transitionComplete = false;
		private int _slideTimerMax = 120;
		private int _slideTimer = -1;
		private readonly int _waitTimerMax = 12;
		private int _waitTimer;

		public bool RemovePlayer { get; set; }

		public ManageScreens(ContentManager content, GraphicsDeviceManager graphics)
		{
			_graphics = graphics;
			_content = content;
			_fadeRect = _content.Load<Texture2D>("Textures/s_pixel");
			_waitTimer = _waitTimerMax;
		}

		public void LoadNewScreen(Screen screen, string transitionType)
		{
			ManageInput.CanPressButtons = false;

			_lastScreen = _currentScreen;
			_screen = screen;

			if (_lastScreen != null)
			{
				_lastScreen.Uninitialize();
			}

			switch (transitionType)
			{
				case "Fading":
					FadeTransition();
					break;

				case "SlidingRight":
					_direction = Direction.Right;
					SlideTransition();
					break;

				case "SlidingLeft":
					_direction = Direction.Left;
					SlideTransition();
					break;

				case "SlidingUp":
					_direction = Direction.Up;
					SlideTransition();
					break;

				case "SlidingDown":
					_direction = Direction.Down;
					SlideTransition();
					break;

				default:
					_currentScreen = _screen;
					_currentScreen.Initialize();
					_currentScreen.LoadContent(_content);
					ManageInput.CanPressButtons = true;
					break;

			}
		}

		public void GoBackOneScreen()
		{
			if (_lastScreen == null)
				return;
			_currentScreen.Uninitialize();
			_currentScreen = _lastScreen;
			_currentScreen.Initialize();
		}

		public void Update(GameTime gameTime)
		{
			_currentScreen.Update(gameTime);

			//fade transition
			if (_fading && _waitTimer <=0)
			{
				if (_fadingIn)
				{
					_alpha += 20;
					_waitTimer = _waitTimerMax;
				}
				if (_fadingOut)
				{
					_alpha -= 20;
					_waitTimer = _waitTimerMax;
				}

				if (_alpha >= 255 || _alpha <= 0)
				{

					FadeTransition();
					_waitTimer = _waitTimerMax;

					if (_alpha <= 0)
					{
						_alpha = 0;
					}
					
				}
			}
			_waitTimer--;
			//sliding transition
			if (_sliding)
			{
				if (_slideTimer > 0)
				{
					var playerTransform = PlayerController.Player.GetComponent<Transform>(ComponentType.Transform);
					var playerVelocity = PlayerController.Player.GetComponent<Transform>(ComponentType.Transform).Velocity = Vector2.Zero;

					Camera.cameraMin.X = Camera.position.X;
					Camera.cameraMax.X = Camera.position.X;
					Camera.cameraMin.Y = Camera.position.Y;
					Camera.cameraMax.Y = Camera.position.Y;

					playerTransform.Move(_direction, _slideDir.X, _slideDir.Y);
					HUD.PlayerCurrentPosition = playerTransform.Position;
					Camera.cameraMax += _cameraSlide;
					Camera.cameraMin += _cameraSlide;
					Camera.position += _cameraSlide;
					_slideTimer--;
				} 
				else 
				{
					_transitionComplete = true;
					SlideTransition();
				}
			}


		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.BackToFront,
									BlendState.AlphaBlend,
									SamplerState.PointClamp,
									null,
									null,
									null,
									Camera.GetTransformMatrix());
			_currentScreen.Draw(spriteBatch);
			spriteBatch.End();

			HUD.Draw(spriteBatch);

			spriteBatch.Begin(SpriteSortMode.BackToFront,
									BlendState.AlphaBlend,
									SamplerState.PointClamp,
									null,
									null,
									null,
									Camera.GetTransformMatrix());
			if (_fading)
			{
				spriteBatch.Draw(_fadeRect, Camera.ScreenRect, new Color(Color.Black, _alpha));
			}
			spriteBatch.End();
		}


		public void SlideTransition()
		{
			if (!_transitionComplete)
			{
				if (!_sliding)
				{
					_sliding = true;
					_slideTimer = _slideTimerMax;
					_transitionComplete = false;
				}

				switch (_direction)
				{
					case Direction.Right:
						_cameraSlide = new Vector2(4, 0);
						_slideDir = new Vector2(.25f, 0);
						break;
					case Direction.Left:
						_cameraSlide = new Vector2(-4, 0);
						_slideDir = new Vector2(.25f, 0);
						break;
					case Direction.Down:
						_cameraSlide = new Vector2(0, 2f);
						_slideDir = new Vector2(0, 0.2f);
						break;
					case Direction.Up:
						_cameraSlide = new Vector2(0, -2f);
						_slideDir = new Vector2(0, 0.2f);
						break;
				}
			}
			else 
			{
				_sliding = false;
				_transitionComplete = false;
				_currentScreen = _screen;
				_currentScreen.Initialize();
				_currentScreen.LoadContent(_content);

				if (_direction == Direction.Up || _direction == Direction.Down)
				{
					var collision = PlayerController.Player.GetComponent<Collision>(ComponentType.Collision);
					collision.StartOnLadder = true;
				}

				ManageInput.CanPressButtons = true;
			}
		}

		public void FadeTransition()
		{
			_fading = true;

			if (_alpha <= 0)
			{
				_fadingIn = true;
				if (_transitionComplete)
				{
					_fading = false;
					_fadingIn = false;
					_fadingOut = false;
					_transitionComplete = false;
				}
			}

			if (_alpha > 255)
			{
				_fadingOut = true;
				_fadingIn = false;
				_transitionComplete = true;
				PlayerController.Player = null;
				_currentScreen = _screen;
				_currentScreen.Initialize();
				_currentScreen.LoadContent(_content);
				ManageInput.CanPressButtons = true;
			}
		}
	}
}
