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
//Professor Mesh
//Purpose: To initialize boss traits and behaviors.

namespace Palingenesis
{
    //player the player plays as, it's kinda self explanitory 
    class Player : GameObject
    {
        //player attributes
        private List<Bullet> shotList = new List<Bullet>();
        private KeyboardState keyboardState;
        private Texture2D shotTexture;
        private SoundEffect takeDamadge;


        /// <summary>
        /// enumerator for for drawing based on player input
        /// </summary>
        private enum PlayerState
        {
            FaceForward,
            FaceRight,
            FaceLeft,
            FaceBack,
            WalkForward,
            WalkRight,
            WalkLeft,
            WalkBack
        }

        /// <summary>
        /// Player bullet list property
        /// </summary>
        public List<Bullet> ShotList
        {
            get { return shotList; }
        }

        /// <summary>
        /// this is the constructor for the player
        /// </summary>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="attackSpeed"></param>
        /// <param name="Damage"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="takeDamadge"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowWidth"></param>
        /// <param name="shotTexture"></param>
        public Player (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, SoundEffect takeDamadge, int windowHeight, int windowWidth, Texture2D shotTexture) : base (health, moveSpeed, attackSpeed, Damage, texture, position, windowHeight, windowWidth)
        {
            this.shotTexture = shotTexture;
            this.takeDamadge = takeDamadge;
        }

        public override void Update()
        {
            //creates keyboard object
            KeyboardState keyboardState = Keyboard.GetState();

            //WASD movement
            //up
            if (keyboardState.IsKeyDown(Keys.W))
            {
                //stops player from going off screen
                if (position.Y > 0)
                {
                    position.Y -= moveSpeed;
                }
            }

            //down
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if(position.Y < (windowHeight - position.Height))
                {
                    position.Y += moveSpeed;
                }
            }

            //right
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if(position.X < (windowWidth - position.Width))
                {
                    position.X += moveSpeed;
                }
                
            }

            //Left
            if (keyboardState.IsKeyDown(Keys.A))
            {
                //stops player from going off screen
                if(position.X > 0)
                {
                    position.X -= moveSpeed;
                }
                
            }
        }

        /// <summary>
        /// When annimating draw something that will fit within the attack box
        /// </summary>
        /// <param name="target"></param>
        /// <param name="prevKeyboardState"></param>
        public void Attack(Boss target, KeyboardState prevKeyboardState)
        {
            keyboardState = Keyboard.GetState();

            //if statements for each arrow key direction
            if (keyboardState.IsKeyDown(Keys.Up) && prevKeyboardState.IsKeyUp(Keys.Up))
            {
                //creates a rectangle 10 pixels above the player
                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, BulletType.Up, target, damage, attackSpeed));
                
            }
            //use else if so the player can only attack in one direction at a time
            else if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down))
            {
                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, BulletType.Down, target, damage, attackSpeed));
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && prevKeyboardState.IsKeyUp(Keys.Right))
            {
                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, BulletType.Right, target, damage, attackSpeed));
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && prevKeyboardState.IsKeyUp(Keys.Left))
            {
                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, BulletType.Left, target, damage, attackSpeed));
            }

        }

        /// <summary>
        /// Method to reset the player
        /// </summary>
        public void Reset()
        {
            health = maxHealth;
            position.X = 200;
            position.Y = 200;
            shotList.Clear();
        }
    }
}
