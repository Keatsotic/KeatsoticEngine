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
	class Collision : Component
	{
		private ManageMap _manageMap;
		private Rectangle _boundingBoxPre; 
		private Vector2 _offset;
		public Rectangle BoundingBoxSetter { get; set; }
		public Rectangle CollisionBoundingBox { get; private set; }
		public Texture2D _bbTexture;
		private Color bbColor = new Color(Color.Red, 0);
		private string _roomNumber;

		public bool StartOnLadder { get; set; }

		public override ComponentType ComponentType => ComponentType.Collision;

		public Collision(ManageMap manageMap, Rectangle boundingBox, Vector2 offset, Texture2D bbTexture)
		{
			_manageMap = manageMap;
			_boundingBoxPre = boundingBox;
			_offset = offset;
			_bbTexture = bbTexture;
			BoundingBoxSetter = new Rectangle((int)(_boundingBoxPre.X + _offset.X), 
											  (int)(_boundingBoxPre.Y + _offset.Y), 
											  _boundingBoxPre.Width, 
											  _boundingBoxPre.Height);
		}


		public bool CheckCollision(Rectangle rectangle)
		{
			return _manageMap.CheckCollision(rectangle);
		}

		public Rectangle CheckCollisionDoor(Rectangle rectangle)
		{
			return _manageMap.CheckCollisionDoor(rectangle, out _roomNumber);
		}

		public Rectangle CheckCollisionLadder(Rectangle rectangle)
		{
			return _manageMap.CheckCollisionLadder(rectangle);
		}

		public override void Update(GameTime gameTime)
		{
			var transform = GetComponent<Transform>(ComponentType.Transform);
			CollisionBoundingBox = new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X),
												 (int)(transform.Position.Y + BoundingBoxSetter.Y),
												 BoundingBoxSetter.Width,
												BoundingBoxSetter.Height);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
#if DEBUG
			var transform = GetComponent<Transform>(ComponentType.Transform);
			CollisionBoundingBox = new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X),
												 (int)(transform.Position.Y + BoundingBoxSetter.Y),
												 BoundingBoxSetter.Width,
												BoundingBoxSetter.Height);
			
			spriteBatch.Draw(_bbTexture, CollisionBoundingBox, new Color(112,111,111,3));
#endif
		}

		public void CollisionManager(float x, float y, Transform transform)
		{

			//check horizontal collisions
			var _hspd = transform.Velocity.X;

			if (CheckCollision(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X + transform.Velocity.X), 
											 (int)(transform.Position.Y + BoundingBoxSetter.Y), 
											 BoundingBoxSetter.Width, 
											 BoundingBoxSetter.Height)))
			{

				while (transform.Velocity.X != 0 && !CheckCollision(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X + Math.Sign(_hspd)), 
																				  (int)(transform.Position.Y + BoundingBoxSetter.Y), 
																				  BoundingBoxSetter.Width, 
																				  BoundingBoxSetter.Height)))
				{
					transform.Position.X += Math.Sign(_hspd);
				}
				_hspd = 0;
			}

			transform.Position.X += _hspd;



			//check vertical collisions
			var _vspd = transform.Velocity.Y;

			if (CheckCollision(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X),
											 (int)(transform.Position.Y + BoundingBoxSetter.Y + transform.Velocity.Y), 
											 BoundingBoxSetter.Width, 
											 BoundingBoxSetter.Height)))
			{
				while (transform.Velocity.Y != 0 && !CheckCollision(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X), 
																				  (int)(transform.Position.Y + BoundingBoxSetter.Y + Math.Sign(_vspd)), 
																				  BoundingBoxSetter.Width, 
																				  BoundingBoxSetter.Height)))
				{
					transform.Position.Y += Math.Sign(_vspd);
				}
				_vspd = 0;
			}
			transform.Position.Y += _vspd;


			#region PLAYER SPECIFIC COLLISIONS

			//player specific collisions
			var owner = GetOwnerId();
			var doorRect = CheckCollisionDoor(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X),
													 (int)(transform.Position.Y + BoundingBoxSetter.Y),
													 BoundingBoxSetter.Width,
													 BoundingBoxSetter.Height));
			if (owner != "Player")
				return;

			if (ManageInput.CanPressButtons)
			{
				//check for door
				if (_roomNumber != "")
				{
					if (_roomNumber.Length < 3)
					{

						if (doorRect.Y < transform.Position.Y + BoundingBoxSetter.Y && doorRect.X + doorRect.Width > transform.Position.X && doorRect.Y + doorRect.Height < transform.Position.Y + BoundingBoxSetter.Height)
						{
							_manageMap.StartTransition(ManageMap.Level, _roomNumber, "SlidingUp");
						}
						else if (transform.Position.X + BoundingBoxSetter.X < doorRect.X && transform.Position.Y + BoundingBoxSetter.Y > doorRect.Y)
						{
							_manageMap.StartTransition(ManageMap.Level, _roomNumber, "SlidingRight");
						}
						else if (transform.Position.X + BoundingBoxSetter.X > doorRect.X && transform.Position.Y + BoundingBoxSetter.Y > doorRect.Y)
						{
							_manageMap.StartTransition(ManageMap.Level, _roomNumber, "SlidingLeft");
						}
						else if (transform.Position.X + BoundingBoxSetter.X > doorRect.X && transform.Position.Y + BoundingBoxSetter.Y < doorRect.Y)
						{
							_manageMap.StartTransition(ManageMap.Level, _roomNumber, "SlidingDown");
						}
					}
					else if (_roomNumber.Length > 3)
					{
						_manageMap.StartTransition(_roomNumber, "1", "Fading");
					}
				}

				//check for ladder
				var ladderRect = CheckCollisionLadder(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X),
													 (int)(transform.Position.Y + BoundingBoxSetter.Y),
													 BoundingBoxSetter.Width,
													 BoundingBoxSetter.Height/2));

				var player = GetComponent<PlayerController>(ComponentType.PlayerController);

				if ((ladderRect != Rectangle.Empty && ManageInput.playerUp ) || (StartOnLadder && ladderRect != Rectangle.Empty))
				{
					transform.Position.X = ladderRect.X + (ladderRect.Width / 2) - 27;
					player.isOnLadder = true;
					StartOnLadder = false;
				} 
				else if (ladderRect == Rectangle.Empty && player.CurrentState == State.Ladder)
				{
					player.CurrentState = State.Fall;
				}

				var ladderExitRect = CheckCollisionLadder(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X),
													 (int)(transform.Position.Y + BoundingBoxSetter.Y),
													 BoundingBoxSetter.Width,
													 BoundingBoxSetter.Height));

				if (ladderExitRect != Rectangle.Empty && ladderRect == Rectangle.Empty)
				{
					while(CheckCollisionLadder(new Rectangle((int)(transform.Position.X + BoundingBoxSetter.X),
													 (int)(transform.Position.Y + BoundingBoxSetter.Y),
													 BoundingBoxSetter.Width,
													 BoundingBoxSetter.Height)) != Rectangle.Empty)
					{
						transform.Position.Y -= 1;
					}
				}
			}
			#endregion
		}
	}
}
