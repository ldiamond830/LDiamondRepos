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
    public enum type
    {
        healthUP,
        invincibility,
        speedUP,
        clearScreen

    }
    class PowerUp
    {
        private Rectangle position;
        private type powerUpType;
        private Texture2D texture;
        private bool isActive = true;
        private bool isVisible = true;
        private SoundEffect sfx;
        private bool endInvincible;

        public bool EndInvincible
        {
            get { return endInvincible; }
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        public bool IsVisible
        {
            get { return isVisible; }
        }
        public PowerUp(Rectangle position, type powerUpType, Texture2D texture, SoundEffect sfx)
        {
            this.position = position;
            this.powerUpType = powerUpType;
            this.texture = texture;
            this.sfx = sfx;
        }

        public void Update(Player player, double timer, List<enemy> enemyList)
        {
            if (player.Position.Intersects(position))
            {
                if (powerUpType != type.invincibility)
                {
                    sfx.Play(1, 0, 0);
                }

                if (powerUpType == type.healthUP)
                {
                    player.Health++;
                    isActive = false;
                }
                else if (powerUpType == type.invincibility)
                {
                    endInvincible = false;
                    if (player.IsInvincible == false)
                    {
                        sfx.Play(1, 0, 0);
                    }
                    isVisible = false;
                    player.IsInvincible = true;




                }

                else if (powerUpType == type.speedUP)
                {
                    player.Speed++;
                    isActive = false;
                }

                else if (powerUpType == type.clearScreen)
                {
                    enemyList.Clear();
                    isActive = false;
                }
            }

            if (timer < 0)
            {
                player.IsInvincible = false;
                isActive = false;
                endInvincible = true;

            }
        }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

    }
}