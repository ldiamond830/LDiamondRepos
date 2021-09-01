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

        
        public Alive(Texture2D projectileTexture, Texture2D texture, Texture2D texture2, Player player, Texture2D texture3, Texture2D texture4, Texture2D tenticleTexture) : base(100, 10, 10, 15, new Rectangle(960, 540 ,200, 100), projectileTexture, texture, player)
        {
            this.texture1 = texture;
            this.texture2 = texture2;
            this.texture3 = texture3;
            this.texture4 = texture4;
            this.tenticleTexture = tenticleTexture;

        }

        public void Update(double timer)
        {
            this.timer = timer;

            if(timer >= 1 && !isCharging && !isWhiping)
            {
                attackSwitch = true;
                ChooseAttack(rng.Next(1,5));
            }
            else
            {
                if(position.X > 10 || position.X < 1900 - position.Width || position.Y > 10 || position.Y < 1069 - position.Height)
                {
                    MoveCloser();
                }
               
            }

            if (isCharging)
            {
                position.X += (int)(ChargeVector.X * moveSpeed);
                position.Y += (int)(ChargeVector.Y * moveSpeed);

                if (position.Intersects(player.Position) && chargeHit == false)
                {
                    player.Health -= 10;
                    chargeHit = true;
                }

                if (position.X < 10 || position.X > 1900 - position.Width || position.Y < 10 || position.Y > 1069 - position.Height)
                {
                    isCharging = false;
                    chargeHit = false;
                }

                
            }

            if (isWhiping)
            {
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
            Tenticle = new Rectangle(position.X + position.Width/4 , position.Y + (position.Height / 4), 200, 50);
            isWhiping = true;
            tenticleSpeed = 8;
        }

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
