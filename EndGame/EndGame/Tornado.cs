using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
namespace EndGame
{
    enum direction
    {
        towardX,
        towardY,
        awayX,
        awayY,
    }

    class Tornado
    {
        //fields
        private Rectangle position;
        private Texture2D texture;
        private int damage;
        private bool isActive = false;
        private Player target;
        private int speed;
        private GameTime gameTime;
        private double timer = 0;
        private direction currentDirection = direction.towardX;
        private Vector2 path;
        
        //constructor
        public Tornado(Rectangle position, Texture2D texture, int damage, Player target, int speed, GameTime gameTime)
        {
            this.position = position;
            this.texture = texture;
            this.damage = damage;
            this.target = target;
            this.speed = speed;
            this.gameTime = gameTime;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        public void Update()
        {
            path = new Vector2(target.Position.X - position.X, target.Position.Y - position.Y);
            path.Normalize();

            timer += gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= 0.75)
            {
                timer = 0;
                changeDirection();
            }

            if (position.Intersects(target.Position))
            {
                isActive = true;
                target.Health -= damage;
            }

            
            switch (currentDirection)
            {
                case direction.towardX:
                    position.X += (int)(speed * path.X);
                    break;

                case direction.towardY:
                    position.Y += (int)(speed * path.X);
                    break;

                    case direction.awayX:
                    position.X -= (int)(speed * path.X);
                    break;

                case direction.awayY:
                    position.Y -= (int)(speed * path.X);
                    break;

            }

            foreach(Bullet bullet in target.ProjectileList)
            {
                if (position.Intersects(bullet.Position))
                {
                    isActive = false;
                }
            }
        }

        private void changeDirection()
        {
            Random rng = new Random();
            int pick = rng.Next(1, 11);

            if(pick <= 4)
            {
                currentDirection = direction.towardY;
            }
            else if(pick > 4 && pick <= 9)
            {
                currentDirection = direction.towardX;
            }
            //has a low chance of moving away from the player
            else if(pick > 9 && pick != 11)
            {
                currentDirection = direction.awayX;
            }
            else
            {
                currentDirection = direction.awayY;
            }
            
        }
    }
}
