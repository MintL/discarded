using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Discarded.Components;

namespace Discarded
{
    public class Monster : GameObject
    {
        private Game game;
        private float speed = 0.03f;
        private float jumpSpeed = 1.2f;
        private float gravity = 0.08f;
        private Vector2 velocity = Vector2.Zero;

        public bool Grounded = false;
        public TerrainLine TerrainLine;

        public Monster(Game game, Vector2 position)
        {
            this.game = game;
            Texture2D tex = game.Content.Load<Texture2D>("monster");
            AddComponent(new Transform(this, position));
            AddComponent(new Sprite(this,
                (SpriteBatch)game.Services.GetService(typeof(SpriteBatch)),
                tex));
            AddComponent(new Collider(this, new Rectangle(0, 0, tex.Width, tex.Height)));
            AddComponent(new TerrainCollider(this, new Vector2(tex.Width / 2, tex.Height)));
        }

        public override void Update(GameTime gameTime)
        {
            velocity.X = -speed;

            // Gravity
            if (!Grounded)
            {
                velocity.Y += gravity;
            }
            else
            {
                velocity.Y = 0;
            }

            // Jumping
            //if (keyboard.IsKeyDown(Keys.Space) && grounded)
            //{
            //    grounded = false;
            //    velocity.Y = -jumpSpeed;
            //}

            Transform.Position += velocity * gameTime.ElapsedGameTime.Milliseconds;

            // Slope walking
            if (Grounded)
            {
                Vector2 pos = Transform.Position + new Vector2(Sprite.Texture.Width / 2, 0);
                Vector2 slope = TerrainLine.B - TerrainLine.A;
                float per = (pos.X - TerrainLine.A.X) / slope.X;

                Transform.Position.Y = (TerrainLine.A.Y + per * slope.Y) - Sprite.Texture.Height;
            }

            // Reset grounded and check ground again next update
            Grounded = false;

            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            if (other.Tag.Equals("Bullet"))
            {
                Remove = true;
                Log.I("Monster", "OnCollision", "Bullet: " + Transform.Position.ToString());
            }
            else if (other.Tag.Equals("Melee"))
            {
                Remove = true;
                Log.I("Monster", "OnCollision", "Melee: " + Transform.Position.ToString());
            }
            else if (other.Tag.Equals("Terrain"))
            {
                TerrainLine = (TerrainLine)other;
                Grounded = true;
            }
        }

    }
}
