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
        public int Depth { get; private set; }

        public float ScrollRate { get; private set; }

        public List<GameObject> StaticSprites = new List<GameObject>();

        private SpriteBatch spriteBatch;
        private ContentManager content;

        public Layer(int depth, float scrollRate, SpriteBatch spriteBatch, ContentManager content)
        {
            this.Depth = depth;
            this.ScrollRate = scrollRate;

            this.spriteBatch = spriteBatch;
            this.content = content;
        }

        public void AddStaticSprite(Vector2 position, string asset)
        {
            GameObject sprite = new GameObject();
            sprite.AddComponent(new Transform(sprite, position));
            sprite.AddComponent(new Sprite(sprite, spriteBatch, content.Load<Texture2D>("TerrainProps/" + asset)));

            StaticSprites.Add(sprite);
        }


        public void Draw()
        {
            // Draw every static background sprite
            foreach (var sprite in StaticSprites)
            {
                sprite.Draw();
            }
        }
    }
}
