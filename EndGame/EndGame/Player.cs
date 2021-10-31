using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    class Player
    {
        //fields
        private int bulletSize = 25;
        private int health;
        private int damage;
        private int moveSpeed;
        private int projectileSpeed;
        private const int windowWidth = 1920;
        private const int windowHeight = 1080;
        private Texture2D projectileTexture;
        private Texture2D playerTexture;
        private Rectangle position;
        private List<Bullet> bulletList = new List<Bullet>();
        private int fullHealth;
        private double fireRate;
        private bool hasShot = false;
        private bool hasDOT = false;
        private int dotDamge = 5;
        private bool canMove = true;

        //properties
        public bool CanMove
        {
            get { return canMove; }
            set { canMove = value; }
        }

        public int Damage
        {
            set { damage = value; }
            get { return damage; }
        }

        public int Speed
        {
            set { moveSpeed = value; }
            get { return moveSpeed; }

        }

        public int DotDamage
        {
            set { dotDamge = value; }
        }

        public bool HasDOT
        {
            get { return hasDOT; }
            set { hasDOT = value; }
        } 

        public bool HasShot
        {
            get { return hasShot; }
            set { hasShot = value; }
        }

        public int FullHealth
        {
            get { return fullHealth; }
            set { fullHealth = value; }
        }

        public double FireRate
        {
            get { return fireRate;  }
            set { fireRate = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public List<Bullet> ProjectileList
        {
            get { return bulletList; }
        }

        public int BulletSize
        {
            set { bulletSize = value; }
        }

        //constructor
        public Player (int health, int damage, int moveSpeed, int projectileSpeed, Texture2D projectileTexture, Texture2D playerTexture, Rectangle position, double FireRate)
        {
            this.health = health;
            this.damage = damage;
            this.moveSpeed = moveSpeed;
            this.projectileSpeed = projectileSpeed;
            this.projectileTexture = projectileTexture;
            this.playerTexture = playerTexture;
            this.position = position;
            fullHealth = health;
            this.fireRate = FireRate;
        }

        //update method called each frame and allows the player to shoot and move
        public void Update(KeyboardState kbState, Boss currentBoss, double timer)
        {
            shoot(kbState, currentBoss, timer);
            move(kbState);
        }

        private void shoot(KeyboardState kbState, Boss currentBoss, double timer)
        {
            //if enough time as passed in between shots
            if(timer >= fireRate)
            {
                //fires a bullet in one of the 4 cardinal directions
                if (kbState.IsKeyDown(Keys.Up))
                {
                    bulletList.Add(new Bullet(projectileTexture, new Rectangle(position.X, position.Y, bulletSize, bulletSize), Direction.up, currentBoss, damage, projectileSpeed));
                    hasShot = true;
                }
                else if (kbState.IsKeyDown(Keys.Down))
                {
                    bulletList.Add(new Bullet(projectileTexture, new Rectangle(position.X, position.Y, bulletSize, bulletSize), Direction.down, currentBoss, damage, projectileSpeed));
                    hasShot = true;
                }
                else if (kbState.IsKeyDown(Keys.Right))
                {
                    bulletList.Add(new Bullet(projectileTexture, new Rectangle(position.X, position.Y, bulletSize, bulletSize), Direction.right, currentBoss, damage, projectileSpeed));
                    hasShot = true;
                }
                else if (kbState.IsKeyDown(Keys.Left))
                {
                    bulletList.Add(new Bullet(projectileTexture, new Rectangle(position.X, position.Y, 25, 25), Direction.left, currentBoss, damage, projectileSpeed));
                    hasShot = true;
                }
            }
           
        }

        private void move(KeyboardState kbState)
        {
            //if the player isn't held in place by a boss effect
            if (canMove)
            {
                //moves in each direction depending on which of the WASD keys are being held
                if (kbState.IsKeyDown(Keys.W))
                {
                    position.Y -= moveSpeed;
                }
                if (kbState.IsKeyDown(Keys.S))
                {
                    position.Y += moveSpeed;
                }
                if (kbState.IsKeyDown(Keys.A))
                {
                    position.X -= moveSpeed;
                }
                if (kbState.IsKeyDown(Keys.D))
                {
                    position.X += moveSpeed;
                }
            }
            
        }

        //draw method
        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(playerTexture, position, color);
        }

        //resets health position and hasShot
        public void Reset()
        {
            health = fullHealth;
            position.X = 30;
            position.Y = 540;
            hasShot = false;
        }

        //called by some boss attacks to move the player in the opposite direction of where they get hit from
        public void KnockBack(Rectangle EnemyPosition, int knockbackQuantitiy)
        {
            if(EnemyPosition.X > position.X)
            {
                position.X += knockbackQuantitiy;
            }
            //uses else if so that if the two X values are equal there is no X knock back
            else if(EnemyPosition.X < position.X)
            {
                position.X -= knockbackQuantitiy;
            }


            if (EnemyPosition.Y > position.Y)
            {
                position.Y += knockbackQuantitiy;
            }
            //uses else if so that if the two X values are equal there is no X knock back
            else if (EnemyPosition.Y < position.Y)
            {
                position.Y -= knockbackQuantitiy;
            }
        }
    }
}
