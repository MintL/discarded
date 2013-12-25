using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Discarded.Components;

namespace Discarded
{
    public class RangeWeapon : Weapon
    {
        private Vector2 bulletOffset;
        private float bulletSpeed;
        private Vector2 aimDirection;

        public RangeWeapon(Game game, string asset, Vector2 originalCenter, float offsetRotation, 
                Vector2 bulletOffset, float bulletSpeed, TimeSpan reloadTime, Vector2 position)
            : base(game, asset, originalCenter, offsetRotation, reloadTime, position, "Range")
        {
            this.bulletOffset = bulletOffset;
            this.bulletSpeed = bulletSpeed;
        }

        public override void Update(Vector2 pos, bool flipped, GameTime gameTime)
        {
            base.Update(pos, flipped, gameTime);

            MouseState mouse = Mouse.GetState();
            aimDirection = new Vector2(mouse.X, mouse.Y) - pos;

            // Aiming
            Transform.Rotation = (float)Math.Acos(aimDirection.X / aimDirection.Length());
            if (aimDirection.Y < 0)
            {
                Transform.Rotation = -Transform.Rotation;
            }
            if (Transform.Flipped)
                Transform.Rotation -= offsetRotation;
            else
                Transform.Rotation += offsetRotation;

            if (Transform.Flipped) Transform.Rotation += MathHelper.Pi;

        }

        public override void Attack()
        {
            aimDirection.Normalize();
            ((DiscardedGame)game).AddBullet((new Bullet(game, aimDirection,
                Transform.Position +
                aimDirection * bulletOffset.X +
                new Vector2(-aimDirection.Y, aimDirection.X) * (Transform.Flipped ? bulletOffset.Y : -bulletOffset.Y), bulletSpeed)));
        }

    }
}
