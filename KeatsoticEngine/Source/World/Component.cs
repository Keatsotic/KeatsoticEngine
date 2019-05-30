using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source
{
	abstract class Component
	{
		private GameObject _gameObject;
		public abstract ComponentType ComponentType { get; }

		public void Initialize(GameObject gameObject)
		{
			_gameObject = gameObject;
		}

		public int GetOwnerId()
		{
			return _gameObject.Id;
		}

		public void Remove()
		{
			_gameObject.RemoveComponent(this);
		}

		public TComponentType GetComponent<TComponentType>(ComponentType componentType) where TComponentType : Component
		{
			return _gameObject.GetComponent<TComponentType>(componentType);
		}

		public abstract void Update(GameTime gameTime);
		public abstract void Draw(SpriteBatch spriteBatch);
	}
}
