using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Discarded.Components
{
    public class Component
    {
        public GameObject GameObject;

        public Component(GameObject gameObject)
        {
            this.GameObject = gameObject;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw()
        {

        }
    }
}
