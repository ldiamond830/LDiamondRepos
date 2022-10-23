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
    class enemy
    {
        protected bool isAlive;
        protected int speed;
        protected Texture2D texture;
        protected SoundEffect voiceLine;
        protected bool hasHit;
        protected Rectangle position;
        double timeTillNextHit = 0;
        protected Vector2 path;
        public int hitCount = 0;
        public bool hasSpoken;
        private bool isHit = false;


        public bool IsAlive
        {
            get { return isAlive; }
        }
        public bool IsHit
        {
            get { return isHit; }
        }

        public Rectangle Position
        {
            get { return position; }
        }
        public enemy(bool isAlive, int speed, Texture2D texture, SoundEffect voiceLine, bool hasHit, Rectangle position)
        {
            this.isAlive = isAlive;
            this.speed = speed;
            this.texture = texture;
            this.voiceLine = voiceLine;
            this.hasHit = hasHit;
            this.position = position;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        public virtual void Update(Player target, Random rng)
        {
            if (position.X != target.Position.X && position.Y != target.Position.Y)
            {
                path = new Vector2(position.X - target.Position.X, position.Y - target.Position.Y);
                path.Normalize();
                position.X -= (int)(speed * path.X);
                position.Y -= (int)(speed * path.Y);
            }




            CheckCollision(target);
            //has a small chance to give combat dialouge
            SayVoiceLine(rng, target);
        }

        public virtual void hitCheck(Bullet bullet, Player player)
        {
            if (bullet.Position.Intersects(position))
            {
                if (IsAlive)
                {
                    isAlive = false;
                    player.Score += 100;
                    bullet.IsActive = false;
                    isHit = true;
                }

            }
        }

        private void SayVoiceLine(Random rng, Player target)
        {
            int chance = rng.Next(1, 2000);

            if (chance == 1)
            {
                //changes the speaker the voice line plays out of depending on which side of the player the enemy is on
                if (position.X < target.Position.X)
                {
                    voiceLine.Play(1, 0, -1);
                }
                else
                {
                    voiceLine.Play(1, 0, 1);
                }

            }
        }
        
        protected virtual void CheckCollision(Player target)
        {
            
            if (position.Intersects(target.Position) && hasHit == false && target.IsInvincible == false)
            {
                target.Health -= 1;
                hitCount++;
                hasHit = true;
            }
            //adds 1/12 of a second to the timeTillNextHit timer every frame
            else if (position.Intersects(target.Position) && hasHit == true && target.IsInvincible == false)
            {
                timeTillNextHit += (0.016666666666);
            }
            //enemies are able to do damage every 0.4 seconds
            if (timeTillNextHit > 0.4)
            {
                hasHit = false;
                timeTillNextHit = 0;
            }
        }
    }
}
