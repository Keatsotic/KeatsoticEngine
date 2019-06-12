using KeatsoticEngine.Source.Data;
using KeatsoticEngine.Source.Map;
using KeatsoticEngine.Source.Prefabs;
using KeatsoticEngine.Source.Screens;
using KeatsoticEngine.Source.World;
using KeatsoticEngine.Source.World.Components;
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
		private List<TileCollisionLadder> _ladders;
		private TiledMap _tiledMap;
		private TiledMapRenderer _tiledMapRenderer;
		private ManageScreens _manageScreens;

		public static string RoomNumber = "1";
		public static string Level = Game1.startLevel;

		private GraphicsDeviceManager _graphics;
		private Vector2 _roomMin;
		private Vector2 _roomMax;

		public string MapName { get; private set; }

		public ManageMap(string mapName, GraphicsDeviceManager graphics, ManageScreens manageScreens)
		{
			_tileCollisions = new List<TileCollision>();
			_ladders = new List<TileCollisionLadder>();
			_doors = new List<TileCollisionDoor>();
			MapName = mapName;
			_graphics = graphics;
			_manageScreens = manageScreens;
		}

		public void LoadMap(Entities entities, ContentManager content, bool killplayer, out Entities outEntities)
		{

			_tiledMap = content.Load<TiledMap>("Tilesets/" + MapName);
			_tiledMapRenderer = new TiledMapRenderer(_graphics.GraphicsDevice);

			//access object layer in map
			var _objectLayer = _tiledMap.GetLayer<TiledMapObjectLayer>("Room_" + RoomNumber);
			var _entitiesFromMap = new List<GameObject>();

			if (!killplayer)
			{
				var createPlayer = new PlayerPrefab(entities,
													this,
													content,
													HUD.PlayerCurrentPosition,
													out entities);
			}


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
					if (_objectLayer.Objects[i].Type == "Player" && killplayer) //add the player
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

					// add doors
					if (_objectLayer.Objects[i].Type == "Ladder") //add enemies
					{
						_ladders.Add(new TileCollisionLadder((int)_objectLayer.Objects[i].Position.X,
														 (int)_objectLayer.Objects[i].Position.Y,
													 	 (int)_objectLayer.Objects[i].Size.Width,
														 (int)_objectLayer.Objects[i].Size.Height));
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
					if ((i >= (_roomMin.X - tiledMapWallsLayer.TileWidth) / tiledMapWallsLayer.TileWidth &&
						j >= (_roomMin.Y - tiledMapWallsLayer.TileHeight) / tiledMapWallsLayer.TileHeight) &&
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


		//collision methods

		public bool CheckCollision(Rectangle rectangle)
		{
			return _tileCollisions.Any(tile => tile.Intersect(rectangle));
		}

		public Rectangle CheckCollisionDoor(Rectangle rectangle, out string roomNumber)
		{
			for (var i = 0; i < _doors.Count; i++)
			{
				if (_doors[i].Intersect(rectangle))
				{
					roomNumber = _doors[i].RoomNumber;
					return _doors[i].Rectangle;
				}
			}
			roomNumber = "";
			return Rectangle.Empty;
		}

		public Rectangle CheckCollisionLadder(Rectangle rectangle)
		{
			for (var i = 0; i < _ladders.Count; i++)
			{
				if (_ladders[i].Intersect(rectangle))
				{
					return _ladders[i].Rectangle;
				}
			}
			return Rectangle.Empty;
		}

		public void StartTransition(string levelName, string roomNumber, string transitionType)
		{
			switch (transitionType)
			{
				case "SlidingRight":
					RoomNumber = roomNumber;
					_manageScreens.LoadNewScreen(new ScreenWorld(_manageScreens, false), "SlidingRight");
					break;
				case "SlidingLeft":
					RoomNumber = roomNumber;
					_manageScreens.LoadNewScreen(new ScreenWorld(_manageScreens, false), "SlidingLeft");
					break;
				case "SlidingUp":
					RoomNumber = roomNumber;
					_manageScreens.LoadNewScreen(new ScreenWorld(_manageScreens, false), "SlidingUp");
					break;
				case "SlidingDown":
					RoomNumber = roomNumber;
					_manageScreens.LoadNewScreen(new ScreenWorld(_manageScreens, false), "SlidingDown");
					break;

				case "Fading":
					RoomNumber = roomNumber;
					_manageScreens.LoadNewScreen(new ScreenWorld(_manageScreens, true), "Fading");
					break;
			}


		}
	}
}
