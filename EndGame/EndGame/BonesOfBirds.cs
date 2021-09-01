using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    enum movement
    {
        up,
        down,
        left,
        right,
        upRight,
        upLeft,
        downRight,
        downLeft,
    }

    class BonesOfBirds : Boss
    {
        //remeber to change values
        private double timer = 0;
        private Random rng = new Random();
        private bool flyActive = false;
        private SoundEffect screechSound;
        private Texture2D tornadoTexture;
        private int xPositionAtStartOfFly;
        private List<Tornado> tornadoList = new List<Tornado>();
        private bool inPosition = false;
        private double contactDamageTimer = 0;
        private int distanceToDirectionChange = 250;
        private movement movementDirection;
        private double movementTimer;

        public BonesOfBirds(Texture2D projectileTexture, Texture2D texture, Player player, SoundEffect screechSound, Texture2D tornadoTexture) : base(500, 10, 10, 5, new Rectangle(960, 540, 100, 100), projectileTexture, texture, player)
        {
            this.screechSound = screechSound;
            this.tornadoTexture = tornadoTexture;

        }


        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            movementTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(movementTimer >= 2)
            {
                DirectionSelector();
                movementTimer = 0;
            }

            if(timer >= 1.25)
            {
                ChooseAttack(rng.Next(1, 6), gameTime);
                timer = 0;
            }

            foreach(Tornado tornado in tornadoList)
            {
                tornado.Update();
            }

            //contact damage
            if (position.Intersects(player.Position) && contactDamageTimer <= 0)
            {
                player.Health -= 10;
                contactDamageTimer = 1;
            }
            //delays time between each contact hit
            if(contactDamageTimer > 0)
            {
                contactDamageTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (flyActive)
            {
                if(position.X < 1930 + position.Width)
                {
                    position.X += moveSpeed;
                }
                else
                {
                    position.Y = player.Position.Y;
                    position.X = -10 - position.Width;
                    inPosition = true;
                }


                if(inPosition = true)
                {
                    position.X += moveSpeed * 2;

                    if(position.X >= xPositionAtStartOfFly)
                    {
                        inPosition = false;
                        flyActive = false;
                    }
                }

            }

            //moves the boss based on the randomly selected direction
            switch (movementDirection)
            {
                case movement.down:
                    position.Y -= moveSpeed;
                    break;

                case movement.up:
                    position.Y += moveSpeed;
                    break;

                case movement.left:
                    position.X -= moveSpeed;
                    break;

                case movement.right:
                    position.X += moveSpeed;
                    break;

                case movement.downLeft:
                    position.Y -= moveSpeed;
                    position.X -= moveSpeed;
                    break;

                case movement.downRight:
                    position.Y -= moveSpeed;
                    position.X += moveSpeed;
                    break;

                case movement.upLeft:
                    position.Y += moveSpeed;
                    position.X -= moveSpeed;
                    break;


                case movement.upRight:
                    position.Y += moveSpeed;
                    position.X += moveSpeed;
                    break;
            }
        }

        private void ChooseAttack(int choice, GameTime gameTime)
        {
            if (!flyActive)
            {
                if (choice == 1)
                {
                    Fly();
                }
                else if (choice == 2)
                {
                    Screech();
                }
                //increases the chance of triple shot slightly to hopefully avoid fly getting picked over and over
                else if (choice == 3 || choice == 6)
                {
                    TripleShot();
                }
                else
                {
                    Tornado(gameTime);
                }
            }
           
        }

        //like in pokemon
        private void Fly()
        {
            flyActive = true;
            xPositionAtStartOfFly = position.X;

        }

        private void Screech()
        {
            //plays a screetching sfx
            screechSound.Play(1, 0, 0);
            //calculates distance between player and the boss
            double distanceBetween = Math.Sqrt(Math.Pow((player.Position.X - position.X),2) - Math.Pow((player.Position.Y - position.Y),2));
            //uses % so that as the plaeyr gets further away the knockback force is reduced
            int castedKnockBackDistance = (int)(200 % distanceBetween);

            player.KnockBack(position, castedKnockBackDistance);
        }

        private void TripleShot()
        {
            //player is on the left of the boss
            if(player.Position.X < position.X)
            {
                //middle
                bulletList.Add(new BossBullet(projectileTexture, position, Direction.left, player, damage, projectileSpeed));
                //angled down
                bulletList.Add(new BossBullet(projectileTexture, position, Direction.custom, player, damage, new Vector2(-projectileSpeed, 5), projectileSpeed));
                //angled up
                bulletList.Add(new BossBullet(projectileTexture, position, Direction.custom, player, damage, new Vector2(-projectileSpeed, -5), projectileSpeed));
            }
            //player is on the right of the boss
            else
            {
                //middle
                bulletList.Add(new BossBullet(projectileTexture, position, Direction.right, player, damage, projectileSpeed));
                //angled down
                bulletList.Add(new BossBullet(projectileTexture, position, Direction.custom, player, damage, new Vector2(projectileSpeed, 5), projectileSpeed));
                //angled up
                bulletList.Add(new BossBullet(projectileTexture, position, Direction.custom, player, damage, new Vector2(projectileSpeed, -5), projectileSpeed));
            }
            
        }

        private void Tornado(GameTime gameTime)
        {
            tornadoList.Add(new Tornado(new Rectangle(this.Position.X, this.Position.Y, 50, 100), tornadoTexture, damage + 5, player, projectileSpeed, gameTime));
        }

        private void DirectionSelector()
        {
            int randomizer = rng.Next(1, 21);

            //keeps boss in bounds
            if(position.X <= 0 || position.X >= 1920 - position.Width || position.Y <= 0 ||  position.Y >= 1080 - position.Height)
            {
               if(position.X <= 0)
               {
                    movementDirection = movement.right;
               }
               else if(position.X >= 1920 - position.Width)
               {
                    movementDirection = movement.left;
               }
               else if(position.Y <= 0)
               {
                    movementDirection = movement.down;
               }
               //initial if statement confirms that the boss is on one border, so this doesn't need an additioal check 
               else
               {
                    movementDirection = movement.up;
               }
            }
            //uses else so that the direction doesn't get reset after going through the first check
            else
            {
                if (randomizer == 1)
                {
                    movementDirection = movement.upLeft;
                }
                else if (randomizer == 2)
                {
                    movementDirection = movement.downLeft;
                }
                else if (randomizer == 3)
                {
                    movementDirection = movement.upRight;
                }
                else if (randomizer == 4)
                {
                    movementDirection = movement.downRight;
                }
                else if (randomizer > 4 && randomizer <= 8)
                {
                    movementDirection = movement.up;
                }
                else if (randomizer > 8 && randomizer <= 12)
                {
                    movementDirection = movement.down;
                }
                else if (randomizer > 12 && randomizer <= 16)
                {
                    movementDirection = movement.right;
                }
                else
                {
                    movementDirection = movement.left;
                }
            }

           




        }
    }
}
