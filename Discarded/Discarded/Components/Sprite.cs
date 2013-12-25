using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Discarded.Components
{
    public class Sprite : Component 
    {
        protected SpriteBatch spriteBatch;
        
        public Texture2D Texture;
        public Vector2 Center;

        public Sprite(GameObject gameObject, SpriteBatch spriteBatch, Texture2D texture)
            : this(gameObject, spriteBatch, texture, Vector2.Zero)
        {
        }

        public Sprite(GameObject gameObject, SpriteBatch spriteBatch, Texture2D texture, Vector2 center) 
            : base(gameObject) 
        {
            this.spriteBatch = spriteBatch;
            this.Texture = texture;
            this.Center = center;
        }

        public override void Draw() 
        {
            if (Texture != null)
            {
                Transform trans = GameObject.Transform;
                if (trans != null)
                {
                    SpriteEffects effect = (trans.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                    spriteBatch.Draw(Texture, trans.Position, null, Color.White, trans.Rotation, Center, trans.Scale, effect, 0);
                }
            }
            else
            {
                Log.E("Sprite", "Draw", "Sprite has not loaded any content yet");
            }
        }

    }
}
