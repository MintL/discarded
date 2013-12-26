using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Discarded.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Discarded.Levels
{
    public class Layer
    {
        public float ScrollRate { get; private set; }


        private SpriteBatch spriteBatch;
        private Texture2D texture;

        public Layer(float scrollRate, SpriteBatch spriteBatch, ContentManager content, string basePath)
        {
            this.ScrollRate = scrollRate;

            this.spriteBatch = spriteBatch;
            this.texture = content.Load<Texture2D>(basePath);
        }

        public void Draw(Vector2 cameraPosition)
        {
            Vector2 position = Vector2.UnitX * 300 + cameraPosition * ScrollRate;
            position.Y = 50;
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
