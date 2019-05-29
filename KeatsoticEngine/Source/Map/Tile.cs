using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Map
{
	public class Tile
	{
		public int XPos { get; set; }
		public int YPos { get; set; }
		public int ZPos { get; set; }

		public List<TileFrame> TileFrames { get; set; }
		public int AnimationSpeed { get; set; }
		private int _animationIndex;
		private double _counter;

		public static readonly int _tileSize = 16 * Game1.Scale;

		public string TextureName { get; set; }
		private Texture2D _texture;

		public Tile()
		{
		}

		public Tile(int xPos, int yPos, int zPos, List<TileFrame> tileFrames, int animationSpeed, string textureName)
		{
			XPos = xPos;
			YPos = yPos;
			TileFrames = tileFrames;
			AnimationSpeed = animationSpeed;
			_animationIndex = 0;
			TextureName = textureName;
		}

		public void LoadContent(ContentManager content)
		{
			_texture = content.Load<Texture2D>(TextureName);
		}

		public void Update(double gameTime)
		{
			if (TileFrames.Count <= 1)
				return;

			_counter += gameTime;
			if (_counter > AnimationSpeed)
			{
				_counter = 0;
				_animationIndex++;
				if(_animationIndex >= TileFrames.Count)
				{
					_animationIndex = 0;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, new Rectangle(XPos * _tileSize, YPos * _tileSize, _tileSize, _tileSize),
										new Rectangle(TileFrames[_animationIndex].TextureXPos * (_tileSize + 1) + 1,
														TileFrames[_animationIndex].TextureYPos * (_tileSize + 1) + 1, _tileSize, _tileSize),
										Color.White);
		}
	}
}
