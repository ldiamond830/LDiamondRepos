using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
namespace EndGame
{
    //created by player to shoot
    public enum Direction
    {
        //at the moment they can only move in straight lines
        up,
        down,
        left,
        right,
        none,
        custom,
        homing,
    }
    class Bullet
    {
        private Rectangle position;
        private int damage;
        private int speed;
        private bool isActive;
        private Direction direction;
        private Texture2D texture;
        private Boss target;
        private bool hasHit = false;
        private Color color = Color.Red;

        public bool HasHit
        {
            get { return hasHit; }
        }

        public Rectangle Position
        {
            get { return position; }
        }

        public Bullet(Texture2D texture, Rectangle position, Direction direction, Boss target, int damage, int speed)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.target = target;
            this.damage = damage;
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

            if (position.Intersects(target.Position))
            {
                
                target.Health -= damage;
                hasHit = true;

            }

            if(position.X > 1920 || position.X < 0 - position.Width || position.Y > 1080 || position.Y < 0 - position.Height)
            {
                hasHit = true;
            }
        }
    }
}
