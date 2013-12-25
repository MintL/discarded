using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Discarded
{
    public class TerrainLine : GameObject
    {
        public Vector2 A, B;
        public Vector2 Direction;
        public Vector2 Normal;

        public TerrainLine(Vector2 a, Vector2 b)
            : base("Terrain")
        {
            this.A = a;
            this.B = b;
            this.Direction = b - a;
            this.Direction.Normalize();
            this.Normal = new Vector2(-Direction.Y, Direction.X);
            this.Normal.Normalize();
        }
    }
}
