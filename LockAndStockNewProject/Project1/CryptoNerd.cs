using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace LockAndStock
{
    class CryptoNerd : enemy
    {
        private Color color = Color.White;
        private int health = 2;
        public CryptoNerd(Texture2D texture, SoundEffect voiceLine, Rectangle position) : base(true, 3, texture, voiceLine, false, position)
        {

        }

        public override void hitCheck(Bullet bullet, Player player)
        {
            if (bullet.Position.Intersects(position) && bullet.IsActive)
            {
                health--;
                bullet.IsActive = false;
            }

        }

        public override void Update(Player target, Random rng)
        {
            base.Update(target, rng);

            if (health == 1)
            {
                color = Color.Red;
            }

            if (health == 0)
            {
                this.isAlive = false;
                target.Score += 100;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, color);
        }
    }
}

