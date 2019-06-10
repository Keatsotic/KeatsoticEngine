using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Map;
using KeatsoticEngine.Source.Prefabs;
using KeatsoticEngine.Source.World;
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
	public class ManageMap
	{
		private List<TileCollision> _tileCollisions;
		private List<TileCollisionDoor> _doors;
		private TiledMap _tiledMap;
		private TiledMapRenderer _tiledMapRenderer;

		public static string RoomNumber = "1";

		private GraphicsDeviceManager _graphics;
		private Vector2 _roomMin;
		private Vector2 _roomMax;

		public string MapName { get; private set; }

		public ManageMap(string mapName, GraphicsDeviceManager graphics)
		{
			_tileCollisions = new List<TileCollision>();
			_doors = new List<TileCollisionDoor>();
			MapName = mapName;
			_graphics = graphics;
		}

		public void LoadMap(Entities entities, ContentManager content, out Entities outEntities)
		{

			_tiledMap = content.Load<TiledMap>("Tilesets/" + MapName);
			_tiledMapRenderer = new TiledMapRenderer(_graphics.GraphicsDevice);

			//access object layer in map
			var _objectLayer = _tiledMap.GetLayer<TiledMapObjectLayer>("Room_" + RoomNumber);
			var _entitiesFromMap = new List<GameObject>();



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
					if (_objectLayer.Objects[i].Type == "Player") //add the player
					{
						if (_objectLayer.Objects[i].Name == "PlayerStart")
						{
							var createPlayer = new PlayerPrefab(entities,
																this,
																content,
																_objectLayer.Objects[i].Position,
																out entities);
							
						}
					}

					if (_objectLayer.Objects[i].Type == "Enemy") //add enemies
					{
						switch(_objectLayer.Objects[i].Name)
						{
							case "Crawler":
								var createPlayer = new CrawlerPrefab(entities, this, content, _objectLayer.Objects[i].Position, out entities);
								break;
						}
					}


					// add doors
					if (_objectLayer.Objects[i].Type == "Door") //add enemies
					{
						_doors.Add(new TileCollisionDoor((int)_objectLayer.Objects[i].Position.X,
														 (int)_objectLayer.Objects[i].Position.Y,
													 	 (int)_objectLayer.Objects[i].Size.Width,
														 (int)_objectLayer.Objects[i].Size.Height, 
														 _objectLayer.Objects[i].Name));
					}
				}
			}
			outEntities = entities;

			//access walls in map
			var tiledMapWallsLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Wall");

			for (int i = 0; i < _tiledMap.Width; i++)
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

		public bool CheckCollisionDoor(Rectangle rectangle)
		{
			return _doors.Any(door => door.Intersect(rectangle));
		}
	}
}
