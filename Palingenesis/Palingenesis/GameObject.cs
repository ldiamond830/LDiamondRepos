using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Name: G-Force
//Date: 3/16/21
//Updated: 5/3/21
//Professor Mesh
//Purpose: To initialize player traits and movement.

namespace Palingenesis
{
    //parent class for player and boss
    abstract class GameObject
    {
        protected int health;
        protected int maxHealth;
        protected int moveSpeed;
        protected int attackSpeed;
        protected int damage;
        protected int windowWidth;
        protected int windowHeight;

        protected Texture2D texture;
        protected Rectangle position;

        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for "source" rectangle (inside the image)
        const int WalkFrameCount = 3;       // The number of frames in the animation
        const int MarioRectOffsetY = 116;   // How far down in the image are the frames?
        const int MarioRectHeight = 72;     // The height of a single frame
        const int MarioRectWidth = 44;      // The width of a single frame

        //properties
        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Rectangle Position 
        {
            get { return position; }

        }

        /// <summary>
        /// gameobject constructor
        /// </summary>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="attackSpeed"></param>
        /// <param name="Damage"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowWidth"></param>
        public GameObject(int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, int windowHeight, int windowWidth)
        {
            this.health = health;
            maxHealth = health;
            this.moveSpeed = moveSpeed;
            this.attackSpeed = attackSpeed;
            this.damage = Damage;
            this.texture = texture;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.position = position;
            this.texture = texture;
        }

        //draws object
        public virtual void Draw(SpriteBatch sb, Color color)
        { 
             sb.Draw(texture, position, color);
        }

        /// <summary>
        /// checks if the character is overlaping with a different rectangle, can be used by all inheriting classes
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public virtual bool CheckCollision(Rectangle collider)
        {
            if (position.Intersects(collider))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract void Update();

        /// <summary>
        /// can center either the player or boss on screen
        /// </summary>
        public void Center()
        {
            position.X = (windowWidth / 2) - (position.Width / 2);
            position.Y = (windowHeight / 2) - (position.Height / 2);
        }

        /// <summary>
        /// moves to the next frame of the spritesheet
        /// </summary>
        /// <param name="gameTime"></param>
        //public void UpdateAnimation(GameTime gameTime)
        //{

        //    // Handle animation timing
        //    // - Add to the time counter
        //    // - Check if we have enough "time" to advance the frame

        //    // How much time has passed?  
        //    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

        //    // If enough time has passed:
        //    if (timeCounter >= timePerFrame)
        //    {
        //        frame += 1;                     // Adjust the frame to the next image

        //        if (frame > WalkFrameCount)     // Check the bounds - have we reached the end of walk cycle?
        //            frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

        //        timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
        //                                        // This keeps the time passed 
        //    }
        //}
    }
}
