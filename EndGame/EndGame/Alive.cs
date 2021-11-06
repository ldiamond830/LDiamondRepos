using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    class Alive : Boss
    {
        //fields
        private double timer;
        private Texture2D texture1;
        private Texture2D texture2;
        private Texture2D texture3;
        private Texture2D texture4;
        private Texture2D tenticleTexture;
        private bool isWhiping = false;
        private bool isCharging = false;
        private Vector2 ChargeVector;
        private Random rng = new Random();
        private Rectangle Tenticle;
        private bool chargeHit = false;
        private int tenticleSpeed;

        //constructor
        public Alive(Texture2D projectileTexture, Texture2D texture, Texture2D texture2, Player player, Texture2D texture3, Texture2D texture4, Texture2D tenticleTexture) : base(100, 10, 10, 15, new Rectangle(960, 540 ,200, 100), projectileTexture, texture, player)
        {
            //I had intended to have the boss switch between several sprites to mimic a basic slither animation but that may end up being cut
            this.texture1 = texture;
            this.texture2 = texture2;
            this.texture3 = texture3;
            this.texture4 = texture4;
            this.tenticleTexture = tenticleTexture;

        }

        public void Update(double timer)
        {
            this.timer = timer;

            //every 1 second if not currently attacking picks a new attack
            if(timer >= 1 && !isCharging && !isWhiping)
            {
                attackSwitch = true;
                ChooseAttack(rng.Next(1,5));
            }
            else
            {
                //when not attacking and inside the screen area moves closer to the player
                if(position.X > 10 || position.X < 1900 - position.Width || position.Y > 10 || position.Y < 1069 - position.Height)
                {
                    MoveCloser();
                }
               
            }

            if (isCharging)
            {
                //charge movement
                position.X += (int)(ChargeVector.X * moveSpeed);
                position.Y += (int)(ChargeVector.Y * moveSpeed);

                //hit detection
                if (position.Intersects(player.Position) && chargeHit == false)
                {
                    player.Health -= 10;
                    chargeHit = true;
                }

                //stops at the edge of the screen area
                if (position.X < 10 || position.X > 1900 - position.Width || position.Y < 10 || position.Y > 1069 - position.Height)
                {
                    isCharging = false;
                    chargeHit = false;
                }

                
            }

            if (isWhiping)
            {
                //gets which side of the boss the player is on and attacks in that direction
                if(player.Position.X < position.X)
                {
                    Tenticle.X -= tenticleSpeed;

                    if (Tenticle.X <= position.X - 200)
                    {
                        isWhiping = false;
                       
                    }
                }
                else
                {
                    Tenticle.X += tenticleSpeed;

                    if (Tenticle.X >= position.X + 200)
                    {
                        isWhiping = false;
                        
                    }
                }
                //hit detector
                if(Tenticle.Intersects(player.Position) && isWhiping == true)
                {
                    player.Health -= damage;
                    isWhiping = false;
                }
            }

        }

        public override void Draw(SpriteBatch sb, Color color)
        {
            //HiddenDraw(sb, color, timer);

            sb.Draw(texture1, Position, color);

            if (isWhiping)
            {
                sb.Draw(tenticleTexture, Tenticle, Color.White);
            }
        }

        //potential animated draw method to impliment later
        /*
        private void HiddenDraw(SpriteBatch sb, Color color, double timer)
        {
            if(player.Position.X < position.X)
            {
                if (timer < 1)
                {
                    sb.Draw(texture1, position, color);
                }
                else
                {
                    sb.Draw(texture2, position, color);
                }
            }
            else
            {
                if (timer < 1)
                {
                    sb.Draw(texture3, position, color);
                }
                else
                {
                    sb.Draw(texture4, position, color);
                }
            }
            
        }
        */

        //randomly selects and attack to use
        private void ChooseAttack(int selector)
        {
            if(selector == 1)
            {
                Charge();
            }
            else if(selector == 2)
            {
                MegaShot();
            }
            else if(selector == 3)
            {
                BulletVomit();
            }
            else
            {
                TenticleWhip();
            }
        }

        private void Charge()
        {
            //activates the charge logic in update
            isCharging = true;
            ChargeVector = new Vector2(player.Position.X - position.X, player.Position.Y - position.Y);
            ChargeVector.Normalize();

        }

        private void BulletVomit()
        {
            for (int i = 0; i < 15; i++)
            {
                if(player.Position.X < position.X)
                {
                    //spawns a bullet with a random y path creating a sort of shot gun spread effect 
                    bulletList.Add(new BossBullet(projectileTexture, new Rectangle(this.Position.X, this.Position.Y, 20, 20), Direction.custom, player, damage/2, new Vector2(-projectileSpeed, rng.Next(-20, 20)), projectileSpeed));
                }
                else
                {
                    bulletList.Add(new BossBullet(projectileTexture, new Rectangle(this.Position.X, this.Position.Y, 20, 20), Direction.custom, player, damage/2, new Vector2(projectileSpeed, rng.Next(-20, 20)), projectileSpeed));
                }
            }
           
        }

        private void TenticleWhip()
        {
            //activates the tenicle whipping logic in update
            Tenticle = new Rectangle(position.X + position.Width/4 , position.Y + (position.Height / 4), 200, 50);
            isWhiping = true;
            tenticleSpeed = 8;
        }

        //spawns one large projectile and launches it at the side of the player
        private void MegaShot()
        {
            if(player.Position.X < position.X)
            {
                bulletList.Add(new BossBullet(projectileTexture, new Rectangle(this.Position.X, player.Position.Y, 50, 50), Direction.left, player, damage, projectileSpeed));
            }
            else
            {
                bulletList.Add(new BossBullet(projectileTexture, new Rectangle(this.Position.X, player.Position.Y, 50, 50), Direction.right, player, damage, projectileSpeed));
            }
            
        }

        //creates a type of movement where the boss will randomly approach the player on either the x or y axis, meant to be more unpredicatble than just having it move towards the player's position
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
    }
}
