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
        //fields
        private Rectangle killBox;
        private bool killBoxActive = false;
        private Texture2D killBoxTexture;
        private double timer = 0;
        private Random rng = new Random();
        private double hitCoolDown = 0;

        //constructor
        public Castle_sMadeOfSand(Texture2D projectileTexture, Texture2D texture, Player player, Texture2D killBoxTexture) : base(300, 10, 5, 10, new Rectangle(860, 440, 200, 200), projectileTexture, texture, player)
        {
            killBox = new Rectangle(position.X + position.Width/2, position.Y + position.Height/2, 10, 10);
            this.killBoxTexture = killBoxTexture;
        }

        public void Update(GameTime gametime)
        {
            //timer that controls when the boss attacks
            timer += gametime.ElapsedGameTime.TotalSeconds;

            //picks an attack every 3 seconds
            if(timer >= 3)
            {
                timer = 0;
                ChooseAttack(rng.Next(1,5));
            }

            if (killBoxActive)
            {
                //expands the killbox rectangle out from the center
                killBox.Inflate(5, 5);

                //when the kill box's width becomkes 900 it disapears and gets reset
                if(killBox.Width >= 900)
                {
                    killBoxActive = false;
                    killBox = new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 10, 10);
                }

                //killbox damage check
                if (killBox.Intersects(player.Position) && hitCoolDown == 0)
                {
                    player.Health -= damage;
                    hitCoolDown = 1;
                }

                //timer for kill box damage
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
            //randomly generates a choice for an attack
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
                //prevents multimple killboxes from being created in a row
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

        //spawns 4 shots 1 for each cardinal direction
        private void CrossShot()
        {
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.up, player, damage, projectileSpeed));
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.left, player, damage, projectileSpeed));
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.right, player, damage, projectileSpeed));
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.down, player, damage, projectileSpeed));
        }

        //spawns a shot that follows the player's movement
        private void HomingShot()
        {
            bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X + position.Width / 2, position.Y + position.Height / 2, 50, 50), Direction.homing, player, damage, projectileSpeed));
        }

        //spawns between 5 and 10 projectiles in a line
        private void Line()
        {
            //variable used separate where each bullet spawns
            int interval = 0;

            //checks if the player is to the left or right of the boss
            if(player.Position.X < position.X)
            {
                for (int i = 0; i < rng.Next(5, 10); i++)
                {
                    bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X - 10, interval, 50, 50), Direction.left, player, damage, projectileSpeed));
                    interval += 100;
                }
            }
            else
            {
                for (int i = 0; i < rng.Next(5, 10); i++)
                {
                    bulletList.Add(new BossBullet(projectileTexture, new Rectangle(position.X - 10, interval, 50, 50), Direction.right, player, damage, projectileSpeed));
                    interval += 100;
                }
            }
        }

        //turns on the killbox behavior in the update method
        private void KillBox()
        {
            killBoxActive = true;
        }
    }
}
