using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeatsoticEngine.Source.Manager;
using KeatsoticEngine.Source.UsrEventHandlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeatsoticEngine.Source.World.Components
{
	class PlayerInput : Component
	{
		private readonly float _speed = 4.0f;

		public override ComponentType ComponentType => ComponentType.PlayerInput;

		public PlayerInput()
		{
			ManageInput.NewInput += ManageInput_NewInput;
		}

		private void ManageInput_NewInput(object sender, NewEventInput e)
		{
			var sprite = GetComponent<Sprite>(ComponentType.Sprite);
			if (sprite == null)
				return;

			var collision = GetComponent<Collision>(ComponentType.Collision);

			var x = 0f;
			var y = 0f;

				switch(e.Input)
				{
					case Input.Up:
						y = -_speed;
						break;
					case Input.Down:
						y = _speed;
						break;
					case Input.Left:
						x = -_speed;
						break;
					case Input.Right:
						x = _speed;
						break;
				}

			if (collision == null || !collision.CheckCollision(new Rectangle((int)(sprite.Position.X + x),(int)(sprite.Position.Y + y), sprite.Height, sprite.Width)))
				sprite.Move(x, y);

		}

		public override void Update(double gameTime)
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}
