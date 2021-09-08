using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    class What_sInMyHead : Boss
    {
        //fields
        private bool phaseChanged = false;
        private Texture2D texture2;
        private double timer = 0;
        private Random rng = new Random();
        private Rectangle axe;
        private Texture2D axeTexture;
        private int timeTillAttack = 3;
        private bool pitVisible = false;
        private bool pitCanDamage = false;
        private Rectangle Pit;
        private Texture2D pitTexture;
        private Vector2 axePath;
        private bool isThrowing = false;
        private bool isReturning = false;
        private double axeDamageTimer = 0;
        private bool axeHit = false;
        private bool grabActive = false;
        private bool hasGrabbed = false;
        private Vector2 grabVector;
        private double grabDistance = 0;
        private bool axeSlamActive = false;
        private bool axeReadyToSlam = false;
        private int playerCurrentX;
        
        //constructor
        public What_sInMyHead(Texture2D projectileTexture, Texture2D texture, Player player, Texture2D texture2, Texture2D axeTexture, Texture2D pitTexture) : base(500, 10, 5, 5, new Rectangle(960, 540, 300, 300), projectileTexture, texture, player)
        {
            this.texture2 = texture2;
            axe = new Rectangle(position.X + 5, position.Y + position.Height / 2, 100, 200);
            this.axeTexture = axeTexture;
            this.pitTexture = pitTexture;
        }

        public void Update(GameTime gameTime)
        {
            //times attack interval
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            //moves the boss closer to the player on either the x or y axis
            MoveCloser();

            if(timer >= timeTillAttack)
            {
                ChooseAttack(rng.Next(1, 3));
                timer = 0;
            }

            //when the boss's health hits 200 it gets a damage increase and attack's more frequently
            if(health <= 200 && phaseChanged == false)
            {
                phaseChanged = true;
                damage *= 2;
                timeTillAttack--;
            }

            //control's behavior for the grab attack
            if (grabActive)
            {
                position.X += (int)(grabVector.X * moveSpeed);

                position.Y += (int)(grabVector.Y * moveSpeed);

                grabDistance += (grabVector.X * moveSpeed) + (grabVector.Y * moveSpeed);

                if(grabDistance >= 900)
                {
                    grabActive = false;
                }

                if (position.Intersects(player.Position))
                {
                    player.CanMove = false;
                    grabActive = false;
                    hasGrabbed = true;
                    
                }
            }
            
            //moves the axe forward on it's path toward the edge of the screen
            if (isThrowing)
            {
                axe.X += (int)(axePath.X * 9);
                axe.Y += (int)(axePath.Y * 9);

                //stops the axe moving at the edge of the screen and starts it moving backwards
                if(axe.X <= 0 || axe.X >= 1920 - axe.Width || axe.Y <= 0 || axe.Y >= 1080 - axe.Height)
                {
                    isThrowing = false;
                    isReturning = true;
                    ReturnAxe();
                }
            }


            if (isReturning)
            {
                
                axe.X += (int)(axePath.X * 9);
                axe.Y += (int)(axePath.Y * 9);
                //stops when it is between 20 pixels a 0 pixels away from the left edge of the boss's position
                if(axe.X <= position.X + 20 && axe.X >= position.X )
                {
                    isReturning = false;
                }
                //recalibrates vector to compensate for boss moving at the same time
                ReturnAxe();
            }

            //axe hit detection
            if(isReturning || isThrowing)
            {
                if (axe.Intersects(player.Position) && axeDamageTimer == 0)
                {
                    player.Health -= damage;
                    axeHit = false;
                    
                    //knocks player back
                    
                        player.KnockBack(position, 45);
                    
                }
                else if(axeDamageTimer >= 1)
                {
                    axeDamageTimer = 0;
                }
                if (axeHit)
                {
                    axeDamageTimer += gameTime.ElapsedGameTime.TotalSeconds;
                }
                
            }
            //moves the axe along with the boss when it's stationary
            else if(!axeSlamActive)
            {
                axe = new Rectangle(position.X + 5, position.Y + position.Height / 2, 100, 200);
            }

            if (pitVisible)
            {
                //increase's size of the pit
                Pit.Inflate(1, 1);

                if (pitCanDamage && Pit.Intersects(player.Position))
                {
                    //can only hit the player once
                    player.Health -= 40;
                    pitVisible = false;
                    pitCanDamage = false;
                }

                //player can take damage from being inside the pit when it has an area of at least 500 (starting area is 100)
                if(Pit.Width * Pit.Height >= 10000)
                {
                    pitCanDamage = true;
                }

                //despawns when area is 1000
                if(Pit.Width * Pit.Height >= 50000)
                {
                    pitVisible = false;
                    pitCanDamage = false;
                }
            }

            if (axeSlamActive)
            {
                //makes sure path compensates for player movement
                AxeSlam();
                axe.X += (int)(axePath.X * 11);
                axe.Y += (int)(axePath.Y * 11);
                

                if (axe.Y < 30)
                {
                    playerCurrentX = player.Position.X;
                    axeReadyToSlam = true;
                }

                //stops axe before it reaches the bottom
                if(axe.Y > 1000)
                {
                    axeSlamActive = false;
                    axeReadyToSlam = false;
                    ReturnAxe();
                }
            }
            
        }

        private void ChooseAttack(int choice)
        {
            //if the boss is already in the process of grabbing deals damage to the player rather than picking a new attack, this was to save having to create a separate timer
            if (hasGrabbed)
            {
                player.Health -= damage;
                hasGrabbed = false;
                
                player.CanMove = true;

            }
            else
            {
                //different attack choices based on which phase the boss is in
                if (!phaseChanged)
                {
                    if (choice == 1)
                    {
                        if (!isThrowing && !isReturning)
                        {
                            ThrowAxe();
                        }
                        else
                        {
                            CreatePit();
                        }

                    }
                    else
                    {
                        CreatePit();
                    }
                }
                else
                {
                    if (choice == 1)
                    {
                        Grab();
                    }
                    else
                    {
                        AxeSlam();
                    }
                }
            }
            
        }

        private void MoveCloser()
        {
            Vector2 moveVector = new Vector2(player.Position.X - position.X, player.Position.Y - position.Y);
            moveVector.Normalize();
            if (rng.Next(1, 3) == 1)
            {
                position.X += (int)(moveVector.X * moveSpeed / 2);

            }
            else
            {
                position.Y += (int)(moveVector.Y * moveSpeed / 2);
            }
        }

        private void ThrowAxe()
        {
            axePath = new Vector2(player.Position.X - axe.X, player.Position.Y - axe.Y);
            axePath.Normalize();
            isThrowing = true;
            
        }

        private void CreatePit()
        {
            //spawns a rectangle under the player, which expands out (this is controlled elsewhere) and after getting to a certain size cna damage the player
            Pit = new Rectangle(player.Position.X, player.Position.Y, 10,10);
            pitVisible = true;
        }

        private void Grab()
        {
            grabVector = new Vector2(player.Position.X - position.X, player.Position.Y - position.Y);
            grabVector.Normalize();
            grabActive = true;
        }

        private void AxeSlam()
        {
            if(!axeReadyToSlam)
            {
                //creates a path aiming the axe above the player
                axePath = new Vector2(player.Position.X - axe.X, 20 - axe.Y);
                axePath.Normalize();
                axeSlamActive = true;
            }
            else 
            {
                //moves down at 15 and towards the player 
                axePath = new Vector2(playerCurrentX - axe.X, 15);
                axePath.Normalize();
            }
        }


        private void ReturnAxe()
        {
            axePath = new Vector2(((position.X + 5) - axe.X),((position.Y + position.Height / 2) - axe.Y));
            axePath.Normalize();
        }

        public override void Draw(SpriteBatch sb, Color color)
        {
            //uses a different texture for the boxx depending on  the phase
            if(!phaseChanged)
            {
                
                sb.Draw(texture, position, color);
            }
            else
            {
                if (!grabActive)
                {
                    sb.Draw(texture2, position, color);
                }
                else
                {
                    sb.Draw(texture2, position, Color.Red);
                }
                    
            }

            sb.Draw(axeTexture, axe, Color.White);
            //pit changes color when it is able to do damage
            if (pitVisible)
            {
                if (pitCanDamage)
                {
                    sb.Draw(pitTexture, Pit, Color.Red);
                }
                else
                {
                    sb.Draw(pitTexture, Pit, Color.Teal);
                }
            }
        }
    }
}
