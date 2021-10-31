using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace EndGame
{
    
    class BossBullet
    {
        private Rectangle position;
        private int damage;
        private int speed;
        private bool isActive;
        private Direction direction;
        private Texture2D texture;
        private Player target; //separate class for enemy projectiles and player projectiles 
        private bool hasHit = false;
        private Color color = Color.Red;
        private Vector2 path;
        private double maxDistance = 400;

        public bool HasHit
        {
            get { return hasHit;  }
        }

        public BossBullet(Texture2D texture, Rectangle position, Direction direction, Player target, int damage, int Speed)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.target = target;
            this.damage = damage;
            this.speed = Speed;
            if(direction == Direction.homing)
            {
                color = Color.Purple;
            }
        }

        public BossBullet(Texture2D texture, Rectangle position, Direction direction, Player target, int damage, Vector2 path, int speed)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.target = target;
            this.damage = damage;
            this.path = path;
            this.speed = speed;
        }

        public virtual void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, position, color);
        }

        public void Update()
        {
            if (direction == Direction.up)
            {
                //moves up the screen by the set amount movespeed
                position.Y -= speed;
            }
            else if (direction == Direction.down)
            {
                position.Y += speed;
            }
            else if (direction == Direction.left)
            {
                position.X -= speed;
            }
            else if (direction == Direction.right)
            {
                position.X += speed;
            }
            else if(direction == Direction.custom)
            {
                position.X += (int)path.X;
                position.Y += (int)path.Y;
            }
            else if (direction == Direction.homing)
            {
                Vector2 path = new Vector2(target.Position.X - position.X, target.Position.Y - position.Y);
                path.Normalize();

                position.X += (int)(path.X * speed);
                position.Y += (int)(path.Y * speed);

                maxDistance--;

                if(maxDistance == 1)
                {
                    maxDistance = 1000;
                    hasHit = true;
                }

            }

            //hit detection
            if (position.Intersects(target.Position))
            {

                target.Health -= damage;
                hasHit = true;

            }

            //removes the bullet if it goes out of bounds
            if (position.X > 1920 || position.X < 0 - position.Width || position.Y > 1080 || position.Y < 0 - position.Height)
            {
                hasHit = true;
            }
        }
    }
}
