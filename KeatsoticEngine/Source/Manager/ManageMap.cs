using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.Manager
{
	class ManageMap
	{
		private List<TileCollision> _tileCollisions;
		private readonly string _mapName;
		private TiledMap _tiledMap;
		private TiledMapRenderer _tiledMapRenderer;

		private GraphicsDeviceManager _graphics;
		private Vector2 _roomMin;
		private Vector2 _roomMax;

		public ManageMap(string mapName, GraphicsDeviceManager graphics)
		{
			_tileCollisions = new List<TileCollision>();
			_mapName = mapName;
			_graphics = graphics;
		}

		public void LoadContent(ContentManager content)
		{
			var tileCollision = new List<TileCollision>();

			_tiledMap = content.Load<TiledMap>("Tilesets/" + _mapName);
			_tiledMapRenderer = new TiledMapRenderer(_graphics.GraphicsDevice);

			//access walls in map
			var tiledMapWallsLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Wall");

			//access stairs in map
			var tiledMapInteractLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Interact");

			//access object layer in map
			var _objectLayer = _tiledMap.GetLayer<TiledMapObjectLayer>("Room_" + 1);

			if (_objectLayer != null)
			{
				for (int i = 0; i < _objectLayer.Objects.Length; i++)
				{
					// create camera and min max values
					if (_objectLayer.Objects[i].Type == "Camera") // set camera max and min
					{
						if (_objectLayer.Objects[i].Name == "cameraMin")
						{
							Camera.cameraMin = _objectLayer.Objects[i].Position + Camera.cameraOffset;
							_roomMin = _objectLayer.Objects[i].Position;
						}
						if (_objectLayer.Objects[i].Name == "cameraMax")
						{
							Camera.cameraMax = _objectLayer.Objects[i].Position - Camera.cameraOffset;
							_roomMax = _objectLayer.Objects[i].Position;
						}
					}
				}
			}

			//XMLSerialization.LoadXML(out tiles, string.Format("Content\\{0}_map.xml", _mapName));
			for (int i =0; i < _tiledMap.Width; i++)
			{
				for (int j = 0; j < _tiledMap.Height; j++)
				{
					if ((i > (_roomMin.X - tiledMapWallsLayer.TileWidth) / tiledMapWallsLayer.TileWidth &&
						j > (_roomMin.Y - tiledMapWallsLayer.TileHeight) / tiledMapWallsLayer.TileHeight) &&
						(i <= (_roomMax.X + tiledMapWallsLayer.TileWidth) / tiledMapWallsLayer.TileWidth &&
						j <= (_roomMax.Y + tiledMapWallsLayer.TileHeight) / tiledMapWallsLayer.TileHeight))
					{
						if (tiledMapWallsLayer.TryGetTile(i, j, out TiledMapTile? tile))
						{
							if (tile.Value.GlobalIdentifier == 1) // make walls
							{
								_tileCollisions.Add(new TileCollision(i, j));
							}
						}
					}
				}
			}
			 
		}

		public void Update(GameTime gameTime)
		{
		
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			_tiledMapRenderer.Draw(_tiledMap, Camera.GetTransformMatrix());
		}

		public bool CheckCollision(Rectangle rectangle)
		{
			return _tileCollisions.Any(tile => tile.Intersect(rectangle));
		}
	}
}
