using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Discarded.Components
{
    public class Collider : Component
    {
        public Rectangle Bounds;
        public bool Active = true;
        public Rectangle RelativeBounds;

        public Collider(GameObject gameObject, Rectangle relativeBounds)
            : base(gameObject)
        {
            if (gameObject.Transform == null)
            {
                this.Bounds = relativeBounds;
            }
            else
            {
                Vector2 pos = gameObject.Transform.Position;
                this.Bounds = new Rectangle((int)pos.X + relativeBounds.X, (int)pos.Y + relativeBounds.Y, relativeBounds.Width, relativeBounds.Height);
            }
            this.RelativeBounds = relativeBounds;
            
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 pos = GameObject.Transform.Position;
            this.Bounds = new Rectangle((int)pos.X + RelativeBounds.X, (int)pos.Y + RelativeBounds.Y, RelativeBounds.Width, RelativeBounds.Height);
        }

        public void CheckCollision(GameObject otherObject)
        {
            if (Bounds.Intersects(otherObject.Collider.Bounds))
            {
                GameObject.OnCollision(otherObject);
                otherObject.OnCollision(GameObject);
            } 
            
        }

    }
}
