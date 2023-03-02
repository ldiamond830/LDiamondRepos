
private void OnKick(InputValue value)
    {
        if(!isAttacking && stamina >= kickStaminaCost)
        {
            //updates stamina value and UI
            stamina -= kickStaminaCost;
            staminFill = stamina / 100f;
            staminaSlider.value = staminFill;
            playerStaminaText.text = "Stamina: " + (int)stamina;
            staminaRechargeTimer = staminaRechargeInterval;

            //sets state 
            state = State.isKicking;

            animationAttacking = true;
            animator.SetBool("isKicking", true);

            kick.EnemyList.Clear();

            //checks for orientation and moves hitbox based on player direction
            switch (orientation)
            {
                case Orientation.up:
                    kick.gameObject.transform.position = new Vector2(position.x, position.y + 0.5f);
                    break;
                    
                case Orientation.down:
                    kick.gameObject.transform.position = new Vector2(position.x, position.y - 0.5f);
                    break;
                    
                case Orientation.left:
                    kick.gameObject.transform.position = new Vector2(position.x -0.5f, position.y);
                    break;
                    
                case Orientation.right:
                    kick.gameObject.transform.position = new Vector2(position.x + 0.5f, position.y);
                    break;
            }

            //plays sound effect
            kickSound.enabled = true;
            if (kickSound != null)
            {
                kickSound.Play();
                Debug.Log("Kick Sound Played");
            }
            
            //prevents multiple attacks from being used simultaniously 
            isAttacking = true;

            //sets the kick hitbox to be able to do damage
            kick.IsActive = true;

            //adds each enemy to list so that the attack collision can check for collisions
            for (int i = 0; i < gameManager.EnemyList.Count; i++)
            {
                kick.EnemyList.Add(gameManager.EnemyList[i].GetComponent<BoxCollider2D>());
            }
        }        
    }


  case State.isMoving:
    //reads in direction and updates player position
    Movement();

    //when recharge timer is zero and stamina is below max recharges stamina
    if(staminaRechargeTimer <= 0 && stamina < maxStamina)
    {
        stamina += 7 * Time.deltaTime;
        staminFill = stamina / 50.0f;
        staminaSlider.value = staminFill;

        if (stamina > maxStamina)
        {
            stamina = maxStamina;
            staminFill = stamina / 100f;
            staminaSlider.value = staminFill;
            playerStaminaText.text = "Stamina: " + (int)stamina;

        }
    }

    //uses else if so if stamina is maxed recharge timer doesn't change
    else if(staminaRechargeTimer > 0)
    {
        staminaRechargeTimer -= Time.deltaTime;
    }
    break;

case State.isDodging:
    //returns to normal morment after 0.2 seconds
    if(wait >= 0.2f)
    {
        state = State.isMoving;

        damageAble = true;

        wait = 0;
        //resets player z value so they don't appear above enemies, z is changed while player is dashing to prevent issues when player dodges through an enemy
        position.z++;

    }
    else
    {
        wait += Time.deltaTime;

        //gets player direction from control component
        direction = playerControls.ReadValue<Vector2>();
        velocity = new Vector3(direction.x * moveSpeed, direction.y * moveSpeed, 0);

        //accelerates player to create dashing effect
        velocity *= 2.5f;
        position += velocity * Time.deltaTime;
        transform.position = position;
        collider.enabled = true;
    }
    break;


case State.isThrowing:
    if (wait >= 0.5f)
    {
        wait = 0;
        state = State.isMoving;
        
        thrown.gameObject.transform.position = position;
        thrown.IsActive = false;
        isAttacking = false;
    }
    else
    {
        wait += Time.deltaTime;
    }
    break;
    
    
 //Systems for spawning in enemies from spawning pools based on wave number
    private void Spawning()
    {
        int rng = Random.Range(0, 6);
        //random chance to spawn either a basic enemy or sheild enemy
        if(rng <= 4)
        {
            SpawnBasic();
        }
        else
        {
            SpawnShield();
        }
        
    }

    private void SpawnBasic()
    {
        if (basicEnemySpawnPool.Count > 0)
        {
                EnemyAI newEnemy = basicEnemySpawnPool[0];

                
                enemyList.Add(newEnemy);
                basicEnemySpawnPool.Remove(newEnemy);

                
                int doorSelect = Random.Range(0, 4);

                //positions enemy at chosen spawnpoint
                if (doorSelect == 0)
                {
                    newEnemy.Position = new Vector3(0, cameraHeight / 2 + 5, 0);
                }
                else if (doorSelect == 1)
                {
                    newEnemy.Position = new Vector3(0, cameraHeight / -2 - 5, 0);
                }
                else if (doorSelect == 2)
                {
                    newEnemy.Position = new Vector3(cameraWidth / -2 - 5, 0, 0);
                }
                else
                {
                    newEnemy.Position = new Vector3(cameraWidth / 2 + 5, 0, 0);
                }
                newEnemy.gameObject.SetActive(true);
        }
    }

    private void SpawnShield()
    {

        if (shieldEnemySpawnPool.Count > 0)
        {
            if (!firstShieldSpawn)
            {
                firstShieldSpawn = true;
                tutorialText.ShowShieldTutorial();

                if (endingWavesSound.isPlaying == false)
                {
                    beginningWavesSound.Stop();
                    endingWavesSound.enabled = true;
                    currentMusic = endingWavesSound;
                    endingWavesSound.Play();
                }
            }

            ShieldEnemy newEnemy = shieldEnemySpawnPool[0];

            enemyList.Add(newEnemy);
            shieldEnemySpawnPool.Remove(newEnemy);

           
            int doorSelect = Random.Range(0, 4);

            if (doorSelect == 0)
            {
                //constant value makes it so enemy doesnt pop in on screen ll
                newEnemy.Position = new Vector3(0, cameraHeight / 2 + 5, 0);
            }
            else if (doorSelect == 1)
            {
                newEnemy.Position = new Vector3(0, cameraHeight / -2 - 5, 0);
            }
            else if (doorSelect == 2)
            {
                newEnemy.Position = new Vector3(cameraWidth / -2 - 5, 0, 0);
            }
            else
            {
                newEnemy.Position = new Vector3(cameraWidth / 2 + 5, 0, 0);
            }
            newEnemy.gameObject.SetActive(true);
        }
    }
}

//Behavior for attack hitboxes
void Update()
    {
        //if false prevents collision hitboxes from colliding while eneties are not actively attack
        if (isActive)
        {
            if (isPlayer)
            {
                //checks collisions on each enemy
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i] != null)
                    {
                        if (collider.IsTouching(enemyList[i]))
                        {
                            //prevents player stun locking enemy
                            manager.EnemyList[i].WindUpTimer = 0;

                            if (isThrow)
                            {
                                throwObject.ThrowEnemy(enemyList[i], player.ReturnOrientation, playerCollider, damage);
                            }
                            else
                            {
                                manager.EnemyList[i].TakeDamage(damage);
                                isActive = false;
                            }
                        }
                    }

                }
            }
            else
            {
                if (collider.IsTouching(playerCollider) && manager.Player.DamageAble)
                {
                    if (isThrow)
                    {
                        enemyThrowObject.ThrowPlayer(playerCollider, parentEnemyCollider, damage);
                        isActive = false;
                    }
                    else
                    {
                        player.Damage(damage);
                        isActive = false;
                        this.transform.position = Vector3.zero;
                    }
                    
                }
            }
        }
        //prevents attack hit box from being offset when its parent enemy gets thrown
        else
        {
            if (!isPlayer && this.transform.position != parentEnemy.transform.position)
            {
                this.transform.position = parentEnemy.transform.position;
            }
        }
    }
    
    //Methods for toggleing pause functionality
    public void ShowPauseScreen()
    {
        isPaused = true;
        //greys out game content
        greyFilter.color = new Color(190, 190, 190, 0.7f);

        pauseContent.SetActive(true);

        pauseMusic.Play();
        manager.currentMusic.Pause();
    }

    public void HidePauseScreen()
    {
        isPaused = false;
        //makes the filter transparent
        greyFilter.color = new Color(190, 190, 190, 0.0f);
        pauseContent.SetActive(false);

        pauseMusic.Stop();
        manager.currentMusic.Play();
    }
