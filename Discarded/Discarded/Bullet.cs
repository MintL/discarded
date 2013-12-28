using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Discarded.Components;
using Discarded.Levels;

namespace Discarded
{
    public class Bullet : GameObject
    {
        private Game game;
        private Level level;
        private Vector2 direction;
        private float speed = 1.2f;
        private Rectangle screen;

        public Bullet(Game game, Vector2 direction, Vector2 position, float speed)
            : base("Bullet")
        {
            this.game = game;
            this.direction = direction;
            this.speed = speed;
            this.level = (Level)game.Services.GetService(typeof(Level));

            screen = new Rectangle((int)position.X - level.ScreenWidth / 2, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            AddComponent(new Transform(this, position));
            AddComponent(new Sprite(this,
                (SpriteBatch)game.Services.GetService(typeof(SpriteBatch)),
                game.Content.Load<Texture2D>("bullet")));
            AddComponent(new Collider(this, new Rectangle(0, 0, Sprite.Texture.Width, Sprite.Texture.Height)));
        }

        public override void Update(GameTime gameTime)
        {
            Transform.Position += direction * speed * gameTime.ElapsedGameTime.Milliseconds;
            
            if (!Collider.Bounds.Intersects(screen))
            {
                Remove = true;
                Log.I("Bullet", "Update", "Bullet has gone outside the screen");
            }

            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            Log.I("Bullet", "OnCollision", Transform.Position.ToString());
            Remove = true;
        }

    }
}
