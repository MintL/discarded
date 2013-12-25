using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Discarded.Components
{
    public class Transform : Component
    {
        public Vector2 Position = Vector2.Zero;
        public bool Flipped = false;
        public float Rotation = 0;
        public float Scale = 1;

        public Transform(GameObject gameObject, Vector2 position) 
            : base(gameObject) 
        {
            this.Position = position;
        }

    }
}
