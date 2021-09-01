using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    class Castle_sMadeOfSand : Boss
    {
        private Rectangle killBox;
        private bool killBoxActive = false;
        private Texture2D killBoxTexture;
        private double timer = 0;
        private Random rng = new Random();
        private double hitCoolDown = 0;

        public Castle_sMadeOfSand(Texture2D projectileTexture, Texture2D texture, Player player, Texture2D killBoxTexture) : base(300, 10, 5, 10, new Rectangle(860, 440, 200, 200), projectileTexture, texture, player)
        {
            killBox = new Rectangle(position.X + position.Width/2, position.Y + position.Height/2, 10, 10);
            this.killBoxTexture = killBoxTexture;
        }

        public void Update(GameTime gametime)
        {
            timer += gametime.ElapsedGameTime.TotalSeconds;

            if(timer >= 3)
            {
                timer = 0;
                ChooseAttack(rng.Next(1,5));
            }

            if (killBoxActive)
            {
                killBox.Inflate(5, 5);


                if(killBox.Width >= 900)
                {
                    killBoxActive = false;
                    killBox = new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 10, 10);
                }

                if (killBox.Intersects(player.Position) && hitCoolDown == 0)
                {
                    player.Health -= damage;
                    hitCoolDown = 1;
                }

                if(hitCoolDown > 0)
                {
                    hitCoolDown -= gametime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public override void Draw(SpriteBatch sb, Color color)
        {
            if (killBoxActive)
            {
                sb.Draw(killBoxTexture, killBox, Color.White);
            }
            base.Draw(sb, color);


        }

        private void ChooseAttack(int choice)
        {
            if(choice == 1)
            {
                CrossShot();
            }
            else if (choice == 2)
            {
                HomingShot();
            }
            else if (choice == 3)
            {
                Line();
            }
            else
            {
                if (!killBoxActive)
                {
                    KillBox();
                }
                else
                {
                    HomingShot();
                }
            }
        }
        private void CrossShot()
        {
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.up, player, damage, projectileSpeed));
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.left, player, damage, projectileSpeed));
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.right, player, damage, projectileSpeed));
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.down, player, damage, projectileSpeed));
        }

        private void HomingShot()
        {
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.homing, player, damage, projectileSpeed));
        }

        private void Line()
        {
            int constant = 0;
            if(player.Position.X < position.X)
            {
                for (int i = 0; i < rng.Next(5, 10); i++)
                {
                    bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X - 10, constant, 50, 50), Direction.left, player, damage, projectileSpeed));
                    constant += 100;
                }
            }
            else
            {
                for (int i = 0; i < rng.Next(5, 10); i++)
                {
                    bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X - 10, constant, 50, 50), Direction.right, player, damage, projectileSpeed));
                    constant += 100;
                }
            }
        }

        private void KillBox()
        {
            killBoxActive = true;
        }
    }
}
