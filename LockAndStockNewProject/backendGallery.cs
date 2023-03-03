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

class CryptoNerd : enemy
    {
        private Color color = Color.White;
        private int health = 2;
        
        
        public CryptoNerd(Texture2D texture, SoundEffect voiceLine, Rectangle position) : base(true, 3, texture, voiceLine, false, position)
        {

        }
        
        //overrides parent class to allow enemies built from this class can take multiple hits 
        public override void hitCheck(Bullet bullet, Player player)
        {
            if (bullet.Position.Intersects(position) && bullet.IsActive)
            {
                health--;
                bullet.IsActive = false;
            }

        }
        public override void Update(Player target, Random rng)
        {
            base.Update(target, rng);

            //turns sprite red when it has 1 health remaining
            if (health == 1)
            {
                color = Color.Red;
            }

            if (health == 0)
            {
                this.isAlive = false;
                target.Score += 100;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, color);
        }
    }
}

//game loop
case GameState.game:
    player.Update(fireRate, mouse, kbState);

    //updates all current powerups
    foreach (PowerUp powerUp in powerUpList)
    {

        powerUp.Update(player, inviciblityTimer, enemyList);

        if (powerUp.EndInvincible)
        {
            inviciblityTimer = 2;
        }
    }

    for (int i = 0; i < powerUpList.Count; i++)
    {
        if (!powerUpList[i].IsActive)
        {
            powerUpList.RemoveAt(i);
        }
    }
//spawns new enemy
    if (spawnTimer > spawnTime)
    {
    //enemy type is randomly selected
        SpawnEnemies(rng);
        spawnTimer = 0;
        spawnTime = SetSpawnTimer();
    }

    //updates all active bullets
    foreach (Bullet bullet in player.BulletList)
    {
        if (bullet.IsActive)
        {
            bullet.Update();
        }
    }

    //creates interval between player shots
    if (player.HasShot == false)
    {
        fireRate += gameTime.ElapsedGameTime.TotalSeconds;
    }
    else
    {
        fireRate = 0;
        player.HasShot = false;
    }

    //updates all enemies
    foreach (enemy enemy in enemyList)
    {
        if (enemy.IsAlive && !(enemy is CryptoNerd))
        {
            enemy.Update(player, rng);
        }
        else if (enemy.IsAlive && enemy is CryptoNerd)
        {
            enemy.Update(player, rng);
        }

        //checks if any enemies are being hit by one of the player's bullets
        foreach (Bullet bullet in player.BulletList)
        {

            if (enemy.IsAlive && !(enemy is CryptoNerd))
            {
                enemy.hitCheck(bullet, player);
            }
            else if (enemy.IsAlive && enemy is CryptoNerd)
            {
                enemy.hitCheck(bullet, player);
            }

        }

        if (enemy.IsHit == true)
        {
            //1 in 10 chance of spawning a power up
            int chance = rng.Next(1, 11);
            if (chance == 1)
            {
                GeneratePowerUp(enemy.Position);
            }
        }
    }

    //removes dead enemies from update list
    for (int i = 0; i < enemyList.Count; i++)
    {
        if (!enemyList[i].IsAlive)
        {
            enemyList.RemoveAt(i);
        }
    }

    //removes spent bullets from update list
    for (int i = 0; i < player.BulletList.Count; i++)
    {
        if (!player.BulletList[i].IsActive)
        {
            player.BulletList.RemoveAt(i);
        }
    }

    spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

    if (player.Health == 0)
    {
        currentState = GameState.gameover;
    }

    if (player.IsInvincible)
    {
        inviciblityTimer -= gameTime.ElapsedGameTime.TotalSeconds;
    }
    break;

  //sets time between enemies being spawned, enemies spawn faster the more the player has killed
        public double SetSpawnTimer()
        {
            if (player.Score <= 1500)
            {
                return 1;
            }
            else if (player.Score > 1500 && player.Score <= 3000)
            {
                return 0.9;
            }
            else if (player.Score > 3000 && player.Score <= 4500)
            {
                return 0.8;
            }
            else if (player.Score > 4500 && player.Score <= 6000)
            {
                return 0.7;
            }
            else if (player.Score > 6000 && player.Score <= 7500)
            {
                return 0.6;
            }
            else
            {
                return 0.5;
            }
        }

 //shooting behavior, there is a 0.5 second cool down between when the player can fire
            if (gametime >= 0.5 && mouse.LeftButton == ButtonState.Pressed)
            {
                shotDirection = new Vector2((mouse.X - position.X), (mouse.Y - position.Y));
                shotDirection.Normalize();

                bulletList.Add(new Bullet(shotDirection, projectileTexture, new Rectangle(position.X, position.Y, 50, 50)));
                shotSound.Play(0.1f, 0, 0);

                hasShot = true;
                
                //moves the player in the opposite direction of their shot
                Recoil(shotDirection);
            }
