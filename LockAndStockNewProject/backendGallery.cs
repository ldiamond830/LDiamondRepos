//power up update loop
public void Update(Player player, double timer, List<enemy> enemyList)
        {
            //calls effect when player intersects power up object
            if (player.Position.Intersects(position))
            {
                if (powerUpType != type.invincibility)
                {
                    sfx.Play(1, 0, 0);
                }

                //effect is controlled by type enum
                if (powerUpType == type.healthUP)
                {
                    player.Health++;
                    isActive = false;
                }
                else if (powerUpType == type.invincibility)
                {
                    endInvincible = false;
                    if (player.IsInvincible == false)
                    {
                        sfx.Play(1, 0, 0);
                    }
                    //invicible power ups are set to be invisible rather than removed so they can hold the functionality for ending the period of invicitbility as well
                    isVisible = false;
                    player.IsInvincible = true;




                }

                else if (powerUpType == type.speedUP)
                {
                    player.Speed++;
                    isActive = false;
                }

                else if (powerUpType == type.clearScreen)
                {
                    enemyList.Clear();
                    isActive = false;
                }
            }

            //ends invicibility when timer calculated in main goes below 0
            if (timer < 0)
            {
                player.IsInvincible = false;
                isActive = false;
                endInvincible = true;

            }
        }
