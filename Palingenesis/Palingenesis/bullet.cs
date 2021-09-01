using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Name: G-Force
//Date: 3/16/21
//Updated: 5/3/21
//Professor Mesh
//Purpose: To initialize bullet traits and behaviors.

namespace Palingenesis
{
    public enum BulletType
    {
        //at the moment they can only move in straight lines
        Up,
        Down,
        Left,
        Right,
        None,
        SinLeft,
        SinRight,
        RiceGoddessSpecial,
        ring
    }

    //projectiles to be fired by bosses
    class Bullet : GameObject
    {
        //use 1,2,3,4 for up down left right
        private BulletType Type;
        private Texture2D texture;
        private GameObject target;
        private SoundEffect takeDamadge;
        private Color color = Color.White;
        private bool hasHit = false;
        private double timer = 0;
        private bool CanDamage;
        private Vector2 initialPosition;
        private bool goingUp = true;
        private bool goingDown = false;

        //properties
        public BulletType BulletType
        {
            get { return this.Type; }
            set { Type = value; }
        }

        public bool HasHit
        {
            get { return hasHit; }
            set { hasHit = value; }
        }

        public Color Color
        {
            set { color = value; }
        }

        public Vector2 InitialPosition
        {
            set { initialPosition = value; }
        }
        /// <summary>
        /// Bullet class constructor
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="takeDamadge"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowWidth"></param>
        /// <param name="direction"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        public Bullet(Texture2D texture, Rectangle position, SoundEffect takeDamadge, int windowHeight, int windowWidth, BulletType Type, GameObject target, int damage, int moveSpeed) : base(0, moveSpeed, 0, damage, texture, position, windowHeight, windowWidth)
        {
            this.texture = texture;
            this.Type = Type;
            this.target = target;
            this.takeDamadge = takeDamadge;

            if(Type == BulletType.ring || Type == BulletType.RiceGoddessSpecial)
            {
                CanDamage = false;
            }
            else
            {
                CanDamage = true;
            }
        }

        //player can only shoot in the 4 cardinal directions so they use this method without the parameters
        public override void Update()
        {
            if (Type == BulletType.Up)
            {
                //moves up the screen by the set amount movespeed
                position.Y -= moveSpeed;
            }
            else if (Type == BulletType.Down)
            {
                position.Y += moveSpeed;
            }
            else if (Type == BulletType.Left)
            {
                position.X -= moveSpeed;
            }
            else if (Type == BulletType.Right)
            {
                position.X += moveSpeed;
            }

            if (position.Intersects(target.Position) && CanDamage == true)
            {
                takeDamadge.Play();
                target.Health -= damage;
                hasHit = true;

            }
        }

        public void Update(double specialTimer, double ringTimer, Boss boss)
        {
            if(Type == BulletType.Up)
            {
                //moves up the screen by the set amount movespeed
                position.Y -= moveSpeed;
            }
            else if(Type == BulletType.Down)
            {
                position.Y += moveSpeed;
            }
            else if (Type == BulletType.Left)
            {
                position.X -= moveSpeed;
            }
            else if (Type == BulletType.Right)
            {
                position.X += moveSpeed;
            }
            else if(Type == BulletType.SinLeft)
            {
                

                position.X -= moveSpeed;

                if(position.Y > initialPosition.Y - 70 && goingUp == true)
                {
                    position.Y -= 3;
                    
                }
                else if(position.Y < initialPosition.Y -70 && goingUp == true)
                {
                  
                        goingUp = false;
                        goingDown = true;
                    
                }

               if((position.Y < initialPosition.Y + 70) && goingDown == true)
               {
                    position.Y += 3;
                  
               }
               else if((position.Y > initialPosition.Y + 70) && goingDown == true) 
               {

                    goingDown = false;
                    goingUp = true;
                }
                
              
            }
            else if (Type == BulletType.SinRight)
            {
                position.X += moveSpeed;

                if (position.Y > initialPosition.Y - 70 && goingUp == true)
                {
                    position.Y -= 3;

                }
                else if (position.Y < initialPosition.Y - 70 && goingUp == true)
                {

                    goingUp = false;
                    goingDown = true;

                }

                if ((position.Y < initialPosition.Y + 70) && goingDown == true)
                {
                    position.Y += 3;

                }
                else if ((position.Y > initialPosition.Y + 70) && goingDown == true)
                {

                    goingDown = false;
                    goingUp = true;
                }
            }
            else if(Type == BulletType.ring)
            {
                if(specialTimer < 2)
                {
                    boss.RingActive = true;
                    color = Color.Blue;
                }
                else
                {
                    color = Color.White;
                    CanDamage = true;
                }
            }
            else if(Type == BulletType.RiceGoddessSpecial)
            {
                if(color == Color.Green)
                {
                    CanDamage = false;
                }
               if(specialTimer > 2 && specialTimer < 4)
               {
                    color = Color.Red;
                    CanDamage = true;
                    
               }
               //after 4 seconds. 2 seconds after they become active
               else if(specialTimer > 4)
               {
                    boss.SpecialActive = false;
                    hasHit = true;
                    
               }
            }


            //for player intersection
            if (position.Intersects(target.Position) && CanDamage == true)
            {
                takeDamadge.Play();
                target.Health -= damage;
                hasHit = true;

            }

        }

        public override void Draw(SpriteBatch sb, Color test)
        {
            //only drawn while on screen
            if(position.X > (0 - position.Width) && position.X < (windowWidth + 10) && position.Y > (0 - position.Height) && position.Y < (windowHeight + 10))
            {
                sb.Draw(texture, position, color);
            }
           
        }

    }
}
