using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Discarded.Components;

namespace Discarded
{
    public class GameObject
    {

        public bool Remove = false;

        public string Tag;

        public List<Component> Components
        {
            get;
            private set;
        }

        public Transform Transform
        {
            get;
            private set;
        }

        public Sprite Sprite
        {
            get;
            private set;
        }

        public Collider Collider
        {
            get;
            private set;
        }

        public GameObject()
            : this("Empty")
        {
        }

        public GameObject(string tag)
        {
            Components = new List<Component>();
            this.Tag = tag;
        }

        public void AddComponent(Component component)
        {
            Components.Add(component);
            if (component is Transform)
                Transform = (Transform)component;
            if (component is Sprite)
                Sprite = (Sprite)component;
            if (component is Collider)
                Collider = (Collider)component;
        }

        public Component GetComponent<T>()
        {
            return Components.Find(c => c is T);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var comp in Components)
            {
                comp.Update(gameTime);
            }
        }

        public virtual void Draw()
        {
            foreach (var comp in Components)
            {
                comp.Draw();
            }
        }

        public virtual void OnCollision(GameObject other)
        {
        }

    }
}
