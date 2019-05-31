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
		public Rectangle NewBoundingBox { get { return BoundingBoxSetter; } }
		private Texture2D _bbTexture;
		private Color bbColor = new Color(Color.Red, 0);

		public override ComponentType ComponentType => ComponentType.Collision;

		public Collision(ManageMap manageMap, Rectangle boundingBox, Vector2 offset, Texture2D bbTexture)
		{
			_manageMap = manageMap;
			_boundingBoxPre = boundingBox;
			_offset = offset;
			_bbTexture = bbTexture;
			BoundingBoxSetter = new Rectangle((int)(_boundingBoxPre.X + _offset.X), 
											  (int)(_boundingBoxPre.Y + offset.Y), 
											  _boundingBoxPre.Width, 
											  _boundingBoxPre.Height);
		}

		public bool CheckCollision(Rectangle rectangle, bool fixBox = true)
		{
			return _manageMap.CheckCollision(rectangle);
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
#if DEBUG
			var transform = GetComponent<Transform>(ComponentType.Transform);
			var drawRect = new Rectangle((int)(NewBoundingBox.X + transform.Position.X), 
										 (int)(NewBoundingBox.Y + transform.Position.Y), 
										 NewBoundingBox.Width, 
										 NewBoundingBox.Height);
			spriteBatch.Draw(_bbTexture, drawRect, new Color(112,111,111,3));
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
		}
	}
}
