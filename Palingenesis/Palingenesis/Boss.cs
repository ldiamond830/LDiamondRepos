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
    /// <summary>
    /// boss class enumerator, used to define each boss's unique behavior and look 
    /// </summary>
    public enum bossName
    {
        RiceGoddess,
        NagaBoss,
        
    }
    
    class Boss : GameObject
    {
        //fields
        private Random rng = new Random();

        private SoundEffect takeDamage;

        private Color color = Color.White;

        private bossName type;

        private Point playerPosition;

        private Vector2 positionVector;
        private Vector2 ChargeUpdateVector;

        private bool chargeEnded;
        private bool chargeHit;
        private bool isCharging;
        private bool specialActive;
        private bool ringActive;
       
        private List<Bullet> projectileList = new List<Bullet>();
        private List<Bullet> specialList = new List<Bullet>();
        List<Bullet> ringList = new List<Bullet>();

        private Texture2D bulletTexture;
        private Texture2D teleportTexture;
        private Texture2D primaryTexture;

        //properties
        public bool ChargeEnded
        {
            get { return chargeEnded; }
        }

        public bool SpecialActive
        {
            get { return specialActive; }
            set { specialActive = value; }
        }

        public bool RingActive
        {
            get { return ringActive; }
            set { specialActive = value; }
        }

        public bool IsCharging
        {
            get { return isCharging; }
        }
        
        public List<Bullet> ProjectileList
        {
            get { return projectileList; }
        }

        public List<Bullet> SpecialList
        {
            get { return specialList; }
        }

        public Texture2D TeleportTexture
        {
            set { teleportTexture = value; }
        }

        /// <summary>
        /// Boss class constructor
        /// </summary>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="attackSpeed"></param>
        /// <param name="Damage"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="takeDamage"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowWidth"></param>
        /// <param name="type"></param>
        /// <param name="bulletTexture"></param>
        public Boss (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, SoundEffect takeDamage, int windowHeight, int windowWidth, bossName type, Texture2D bulletTexture, Texture2D teleportTexture): 
                     base(health, moveSpeed, attackSpeed, Damage, texture, position, windowHeight, windowWidth)
        {
            this.type = type;
            this.bulletTexture = bulletTexture;
            this.texture = texture;
            this.takeDamage = takeDamage;
            primaryTexture = texture;
            this.teleportTexture = teleportTexture;

            chargeEnded = false;
            chargeHit = false;
            isCharging = false;
            specialActive = false;
            ringActive = false;
          
        }

        public override void Update()
        {
            //nothing
        }

        public void Update(Player target, double timer)
        {
            
          
            // if the boss is charging
            if (isCharging == true)
            {
                for (int i = 0; i < ringList.Count; i++)
                {
                    projectileList.Remove(ringList[i]);
                }

                //sets color to red
                color = Color.Red;

                //if the boss is not at the edge
                if(positionVector.X > 0 && positionVector.X < (windowWidth - position.Width) && positionVector.Y > 0 && positionVector.Y < (windowHeight - position.Height))
                {
                    //updates the boss's position once per frame based on a value calculated in the charge method
                    positionVector += ChargeUpdateVector;

                    //keeps the position up to date with the vector position
                    position.X = (int)positionVector.X;
                    position.Y = (int)positionVector.Y;

                    //checks if the boss makes contact with the player
                    if (position.Intersects(target.Position) && chargeHit == false)
                    {
                        target.Health -= 30;
                        chargeHit = true;
                    }
                   
                }
                //ends charge
                else
                {
                    isCharging = false;
                    chargeEnded = true;
                    color = Color.White;
                }
            }
            
            //when the charge has ended
            else if(isCharging == false && chargeEnded == true)
            {
                 //changes the boss's texture
                if (timer > 0.2 && timer < 0.5)
                {
                    texture = teleportTexture;
                }
                //moves the boss to the center
                else if (timer > 0.5 && timer < 0.75)
                {
                    Center();
                    chargeHit = false;
                }
                //resets the boss's texture
                else if (timer > 0.75 && timer < 1)
                {
                    texture = primaryTexture;
                }

            }

        }

        /// <summary>
        /// helper method fills bullet list
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="direction"></param>
        /// <param name="position"></param>
        /// <param name="xSpacing"></param>
        /// <param name="ySpacing"></param>
        /// <param name="target"></param>
        public void CreateProjectiles(int amount, BulletType direction, Rectangle position, int xSpacing, int ySpacing, Player target)
        {
            for(int i = 0; i < amount; i++)
            {
               
                projectileList.Add(new Bullet(bulletTexture, position, takeDamage, this.windowHeight, this.windowWidth, direction, target, this.damage, this.attackSpeed));
                
                position.X += xSpacing;
                position.Y += ySpacing;                
            }
        }

        /// <summary>
        /// used to decide what attack the boss will use with a random
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="target"></param>
        /// <param name="attackTimer"></param>
        /// <param name="gameTime"></param>
        public void AI(int tmp, Player target, double attackTimer, GameTime gameTime)
        {
            //uses a random number generator to select what attack the boss uses
            if (tmp == 0)
            {
                Line(target);
            }
            else if (tmp == 1)
            {
                MegaShot(target);
            }
            else if(tmp == 2)
            {
                Circle(target);
                    
            }
            else if(tmp == 3)
            {
                SpecialAttack(target);
                specialActive = true;
            }
            else if(tmp == 4)
            {
                Charge(target, gameTime);
            }
            else if (tmp == 5)
            {
                BigRing(target, attackTimer);
            }
            else if (tmp==6)
            {
                Ring(target, attackTimer);
            }
               
        }

        /// <summary>
        /// method for standard attacks
        /// </summary>
        /// <param name="target"></param>
        private void Line(Player target) // TODO: currently busted 
        {
            //if the player is to the Right of the boss
            if(target.Position.X > this.position.X)
            {
                //creates between 1 and 5 projectiles that fly to the Right
                //each is set 50 pixels to the Right of the boss
                //each bullet spawns 75 pixels apart on the y axis
                CreateProjectiles(rng.Next(5, 11), BulletType.Right, new Rectangle((this.Position.X + position.Width/2), 0, 25, 25), 0, 100, target);
            }

            else
            {
                CreateProjectiles(rng.Next(5, 11), BulletType.Left, new Rectangle((this.Position.X - position.Width / 2), 0, 25, 25), 0, 100, target);
            }
          
        }

        /// <summary>
        /// spawns a ring of shots around the boss to make the player retreat
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timer"></param>
        private void Ring(Player target, double timer)
        {
            //creates a ring of 9 bullets that don't move offset by 100 (in the y x or both directions) which will damage the player if they make contact
            Bullet topLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y - 100, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet LeftMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet bottomLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y + 100, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);

            Bullet bottom = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + 100, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet top = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y - 100, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);

            int changeAmount = position.Width;

            Bullet topRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + changeAmount, this.position.Y - 100, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet RightMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X + changeAmount, this.position.Y, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet BottomRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + changeAmount, this.position.Y + 100, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);


            ringList.Add(topLeftCorner);
            ringList.Add(LeftMiddle);
            ringList.Add(bottomLeftCorner);

            ringList.Add(bottom);
            ringList.Add(top);

            ringList.Add(topRightCorner);
            ringList.Add(RightMiddle);
            ringList.Add(BottomRightCorner);

            for (int i = 0; i < ringList.Count; i++)
            {
                projectileList.Add(ringList[i]);
            }            
                           
        }

        /// <summary>
        /// spawns a BIG ring of shots around the boss to make the player retreat
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timer"></param>
        private void BigRing(Player target, double timer)
        {
            //creates a ring of 9 bullets that don't move offset by 100 (in the y x or both directions) which will damage the player if they make contact
            Bullet topLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - position.Width*2, this.position.Y - position.Width*2, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet LeftMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X - position.Width*2, this.position.Y, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet bottomLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - position.Width*2, this.position.Y + position.Width*2, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);

            Bullet bottom = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + position.Width*2, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet top = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y - position.Width*2, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);

            int changeAmount = position.Width;

            Bullet topRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width*2 + changeAmount), this.position.Y - position.Width*2, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet RightMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width*2 + changeAmount), this.position.Y, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);
            Bullet BottomRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width*2 + changeAmount), this.position.Y + position.Width*2, 60, 60), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage, this.attackSpeed);


            ringList.Add(topLeftCorner);
            ringList.Add(LeftMiddle);
            ringList.Add(bottomLeftCorner);

            ringList.Add(bottom);
            ringList.Add(top);

            ringList.Add(topRightCorner);
            ringList.Add(RightMiddle);
            ringList.Add(BottomRightCorner);

            for (int i = 0; i < ringList.Count; i++)
            {
                projectileList.Add(ringList[i]);
            }

        }
        /// <summary>
        /// fires out shots in a circle around the boss
        /// </summary>
        /// <param name="target"></param>
        private void Circle(Player target)
        {
            //Left
            CreateProjectiles(2, BulletType.Left, new Rectangle(position.X -position.Width/4, position.Y - position.Width / 2, 25, 25), 0, 75, target);

            //bottom
            CreateProjectiles(2, BulletType.Down, new Rectangle(position.X + position.Width / 2, position.Y + position.Width / 4, 25, 25), 75, 0, target);

            //Right 
            CreateProjectiles(2, BulletType.Right, new Rectangle(position.X + position.Width / 4, position.Y + position.Width / 2, 25, 25), 0, 75, target);

            //top
            CreateProjectiles(2, BulletType.Up, new Rectangle(position.X - position.Width / 2, position.Y - position.Width / 4, 25, 25), 75, 0, target);

        }

        /// <summary>
        /// boss charges once at the player  and if it makes contact with the player deals damage
        /// </summary>
        /// <param name="target"></param>
        /// <param name="gameTime"></param>
        private void Charge(Player target, GameTime gameTime)
        {
            //sets the current position 
            positionVector = new Vector2(position.X, position.Y);

            //sets bools used to track the state of the charge
            chargeEnded = false;           
            isCharging = true;

            //finds the vector of difference between the boss and the player's positions
            Vector2 direction = new Vector2(target.Position.X - position.X, target.Position.Y - position.Y);
            direction.Normalize();

            //calculates the amount to update the boss's position per frame
            ChargeUpdateVector.X = (float)(direction.X * (500 + moveSpeed) * gameTime.ElapsedGameTime.TotalSeconds);
            ChargeUpdateVector.Y = (float)(direction.Y * (500 + moveSpeed) * gameTime.ElapsedGameTime.TotalSeconds);
           
        }


        /// <summary>
        /// fires a single large shot that does 2x damage at the player
        /// </summary>
        /// <param name="target"></param>
        private void MegaShot(Player target)
        {
            if (target.Position.X > this.position.X)
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X + 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.Right, target, this.damage * 2, this.attackSpeed));
            }

            else
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X - 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.Left, target, this.damage * 2, this.attackSpeed));
            }
        }

        /// <summary>
        /// Each boss's unique attack
        /// </summary>
        /// <param name="target"></param>
        private void SpecialAttack(Player target)
        {
            //spawns shots around the screen randomly that start doing damge after a certain period
            if(type == bossName.RiceGoddess)
            {
                for (int i = 0; i < rng.Next(10, 16); i++)
                {
                    specialList.Add(new Bullet(bulletTexture, new Rectangle(rng.Next(0, windowWidth), rng.Next(0, windowHeight), 30, 30), takeDamage, windowHeight, windowWidth, BulletType.RiceGoddessSpecial, target, damage + 5,  this.attackSpeed));

                    //color is set to green so that they player knows that they won't be damage by the projectile yet 
                    specialList[i].Color = Color.Green;
                    projectileList.Add(specialList[i]);

                }
            }
            //spawns a large shot that moves based on a sign wave
            else if(type == bossName.NagaBoss)
            {
                if (target.Position.X > this.position.X)
                {
                    Bullet specialBullet = new Bullet(bulletTexture, new Rectangle((this.Position.X + 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.SinRight, target, this.damage * 2, this.attackSpeed);
                    specialBullet.InitialPosition = new Vector2(specialBullet.Position.X, specialBullet.Position.Y);
                    projectileList.Add(specialBullet);
                }

                else
                {
                    Bullet specialBullet = new Bullet(bulletTexture, new Rectangle((this.Position.X - 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.SinLeft, target, this.damage * 2, this.attackSpeed);
                    specialBullet.InitialPosition = new Vector2(specialBullet.Position.X, specialBullet.Position.Y);
                    projectileList.Add(specialBullet);
                }
            }
        }

        public void Reset()
        {
            health = maxHealth;
            position.X = 960;
            position.Y = 540;
            projectileList.Clear();
        }

        /// <summary>
        /// boss draw method
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb, Color color)
        {
            if (!isCharging)
            {
                sb.Draw(texture, Position, Color.White);
            }
            else
            {
                Point test = new Point((int)positionVector.X, (int)positionVector.Y);
                sb.Draw(texture, new Rectangle(test, Position.Size), color);
            }
            
        }

        
    }
}
