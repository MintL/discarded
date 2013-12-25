using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Discarded.Components;

namespace Discarded
{
    public class MeleeWeapon : Weapon
    {
        bool attacking = false;
        float rotation;

        public MeleeWeapon(Game game, string asset, Vector2 originalCenter, float offsetRotation, TimeSpan reloadTime, Vector2 position)
            : base(game, asset, originalCenter, offsetRotation, reloadTime, position, "Melee")
        {
            rotation = offsetRotation;

            AddComponent(new Collider(this, new Rectangle(0, -Sprite.Texture.Height, Sprite.Texture.Height-10, Sprite.Texture.Height-10)));
            Collider.Active = false;
        }

        public override void Update(Vector2 pos, bool flipped, GameTime gameTime)
        {
            base.Update(pos, flipped, gameTime);

            if (attacking)
            {
                rotation += 10;
                if (rotation > 120)
                {
                    attacking = false;
                    rotation = offsetRotation;
                    Collider.Active = false;
                }
            }

            if (Transform.Flipped)
            {
                Collider.RelativeBounds = new Rectangle(-Sprite.Texture.Height+10, -Sprite.Texture.Height, Sprite.Texture.Height-10, Sprite.Texture.Height-10);
            }
            else
            {
                Collider.RelativeBounds = new Rectangle(0, -Sprite.Texture.Height, Sprite.Texture.Height - 10, Sprite.Texture.Height - 10);
            }
            
            Transform.Rotation = MathHelper.ToRadians(Transform.Flipped ? -rotation : rotation);
        }

        public override void Attack()
        { 
            attacking = true;
            // Swing melee weapon
            rotation = 0;
            Collider.Active = true;
        }

    }
}
