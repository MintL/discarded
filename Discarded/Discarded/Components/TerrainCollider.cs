using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Discarded.Components
{
    class TerrainCollider : Component
    {
        public Vector2 RelativePoint;
        public Vector2 Point;
        public bool Active = true;

        public TerrainCollider(GameObject gameObject, Vector2 relativePoint)
            : base(gameObject)
        {
            this.RelativePoint = relativePoint;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameObject.Transform == null)
            {
                this.Point = RelativePoint;
            }
            else
            {
                this.Point = GameObject.Transform.Position + RelativePoint;
            }
        }

        public void CheckCollision(TerrainLine terrainLine)
        {
            Vector2 a = terrainLine.A;
            Vector2 b = terrainLine.B;
            Vector2 dir = terrainLine.Direction;
            float distanceDir = -(Vector2.Dot((a - Point), dir));
            float distance = ((a - Point) + distanceDir * dir).Length();

            if (distanceDir >= 0 && distanceDir <= (b - a).Length() && distance <= 20)
            {
                //Console.WriteLine("Collision: " + distanceDir);
                GameObject.OnCollision(terrainLine);
            }
            //Console.WriteLine(distance + " | " + distanceDir);


        }
    }
}
