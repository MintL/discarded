using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Discarded.Components;
using Microsoft.Xna.Framework.Graphics;

namespace Discarded
{
    public abstract class Weapon : GameObject
    {
        protected Vector2 originalCenter;
        protected float offsetRotation;
        protected Game game;

        protected TimeSpan reloadTime = TimeSpan.FromSeconds(0.2f);
        protected TimeSpan reload;

        public Weapon(Game game, string asset, Vector2 originalCenter, float offsetRotation, TimeSpan reloadTime, Vector2 position, string tag)
            : base(tag)
        {
            this.game = game;
            this.originalCenter = originalCenter;
            this.offsetRotation = offsetRotation;
            this.reloadTime = reloadTime;

            AddComponent(new Transform(this, position));
            AddComponent(new Sprite(this,
                (SpriteBatch)game.Services.GetService(typeof(SpriteBatch)),
                game.Content.Load<Texture2D>(asset),
                originalCenter));
        }

        public virtual void Update(Vector2 pos, bool flipped, GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState mouse = Mouse.GetState();
            Transform.Position = pos;
            Transform.Flipped = flipped;

            Sprite.Center = (Transform.Flipped ? new Vector2(Sprite.Texture.Width - originalCenter.X, originalCenter.Y) : originalCenter);

            reload -= gameTime.ElapsedGameTime;

            // Shooting
            if (mouse.LeftButton == ButtonState.Pressed && reload.TotalSeconds < 0)
            {
                reload = reloadTime;
                Attack();
            }
        }

        public abstract void Attack();
    }
}
