using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Discarded.Components;
using Discarded.Levels;

namespace Discarded
{
    public class Player : GameObject
    {
        private Game game;
        private Level level;
        private KeyboardState keyboard;
        private MouseState mouse;

        private float speed = 0.45f;
        private float jumpSpeed = 1.2f;
        private float gravity = 0.08f;
        private Vector2 velocity = Vector2.Zero;

        public bool Grounded = false;
        public TerrainLine TerrainLine;

        private Weapon weapon;
        private RangeWeapon pistol;
        private RangeWeapon shotgun;
        private MeleeWeapon sword;

        public Player(Game game, Vector2 position)
        {
            this.game = game;
            this.level = (Level)game.Services.GetService(typeof(Level));
            Texture2D tex = game.Content.Load<Texture2D>("player");
            AddComponent(new Transform(this, position));
            AddComponent(new Sprite(this, 
                (SpriteBatch)game.Services.GetService(typeof(SpriteBatch)), 
                tex));
            AddComponent(new TerrainCollider(this, new Vector2(tex.Width / 2, tex.Height)));

            pistol = new RangeWeapon(game, "pistol", new Vector2(8, 12), 0.3f, new Vector2(0, 5), 1.2f, TimeSpan.FromSeconds(0.2), Vector2.Zero);
            shotgun = new RangeWeapon(game, "shotgun", new Vector2(10, 11), 0.1f, new Vector2(25, 0), 2f, TimeSpan.FromSeconds(1), Vector2.Zero);
            sword = new MeleeWeapon(game, "sword", new Vector2(12, 72), 45, TimeSpan.FromSeconds(0.2), Vector2.Zero);
            level.ColliderObjects.Add(sword);

            weapon = sword;
        }

        public override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();

            #region Movement
            // Right
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X = speed;
                //flipped = false;
            }
            // Left
            else if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X = -speed;
                //flipped = true;
            }
            else 
            {
                velocity.X = 0;
            }

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
            if (keyboard.IsKeyDown(Keys.Space) && Grounded)
            {
                Grounded = false;
                velocity.Y = -jumpSpeed;
            }
            
            Transform.Position += velocity * gameTime.ElapsedGameTime.Milliseconds;

            // Slope walking
            if (Grounded)
            {
                Vector2 pos = Transform.Position + new Vector2(Sprite.Texture.Width/2, 0);
                Vector2 slope = TerrainLine.B - TerrainLine.A;
                float per = (pos.X - TerrainLine.A.X) / slope.X;

                Transform.Position.Y = (TerrainLine.A.Y + per * slope.Y) - Sprite.Texture.Height;
            }

            #endregion

            // Flip based on aim
            Transform.Flipped = mouse.X + level.CameraPosition.X < Transform.Position.X + Sprite.Texture.Width / 2;

            // Change weapon
            if (keyboard.IsKeyDown(Keys.D1)) weapon = pistol;
            else if (keyboard.IsKeyDown(Keys.D2)) weapon = shotgun;
            else if (keyboard.IsKeyDown(Keys.D3)) weapon = sword;

            // Calculate weapon
            Vector2 gunPosition = Transform.Position + (Transform.Flipped ? new Vector2(20, 58) : new Vector2(40, 58));
            weapon.Update(gunPosition, Transform.Flipped, gameTime);

            Sprite.Texture = game.Content.Load<Texture2D>(Grounded ? "player" : "player_jump");

            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            if (other.Tag.Equals("Terrain"))
            {
                TerrainLine = (TerrainLine)other;
                Grounded = true;
            }
        }

        public override void Draw()
        {
            //if (texture != null)
            //{
                //SpriteEffects effect = (flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                //spriteBatch.Draw(texture, Position, null, Color.White, 0, new Vector2(0, texture.Bounds.Height), 1f, effect, 0);
                //spriteBatch.Draw(gun, gunPos, null, Color.White, gunRotation, gunOrigin, 1f, effect, 0);
                weapon.Draw();
            //}
            //else
            //{
            //    Log.E("Player", "Draw", "Player texture has not been loaded yet");
            //}

                base.Draw();
        }
    }
}
