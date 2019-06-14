using KeatsoticEngine.Source.World.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeatsoticEngine.Source.World
{
	public class GameObject
	{
		public string Id { get; set; }
		private readonly List<Component> _components;
		protected State CurrentState { get; set; }

		public GameObject()
		{
			_components = new List<Component>();
		}


		public TComponentType GetComponent<TComponentType>(ComponentType componentType) where TComponentType : Component
		{
			return _components.Find(c => c.ComponentType == componentType) as TComponentType;
		}


		public void AddComponent(Component component)
		{
			_components.Add(component);
			component.Initialize(this);
			component.Awake(this);
		}

		public void AddComponent(List<Component> components)
		{
			_components.AddRange(components);
			foreach (var component in components)
			{
				component.Initialize(this);
				component.Awake(this);
			}
		}

		public void RemoveComponent(Component component)
		{
			_components.Remove(component);
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (var components in _components)
			{
				components.Update(gameTime);
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			foreach (var components in _components)
			{
				components.Draw(spriteBatch);
			}
		}
	}
}
