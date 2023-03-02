//behavior selector for the boss enemy, resets state whenever the enemy takes damage 
protected override void SetState()
    {
        int selector;
        //sets state based on a combination of random chance and the enemy's remaining health
        if(Health <= 3)
        {
            currentState = State.enraged;
        }
        else if(Health> 3 && Health >= 10)
        {
            selector = Random.Range(0, 5);
            if(selector == 0)
            {
                currentState = State.enraged;
            }
            else if(selector >0 && selector >= 3)
            {
                currentState = State.aggressive;
            }
            else
            {
                currentState = State.defensive;
            }
        }
        else
        {
            selector = Random.Range(0,1);
            if(selector == 0)
            {
                currentState = State.aggressive;
            }
            else
            {
                currentState = State.aggressive;
            }    
        }
        
        //adjusts speed based on chosen state
        if(currentState == State.enraged)
        {
            speed = enragedSpeed;
        }
        else
        {
            speed = standardSpeed;
        }
    }


//Boss enemy update loop
void Update()
    {
        if (isAgro)
        {     
            base.Update();

            //points both spears at the players
            spear.AdjustRotation();
            spear2.AdjustRotation();

            switch (currentState)
            {
                case State.enraged:
                    //while enraged wil charge frequent and throw out projectiles at the same time
                    if (!isCharging)
                    {
                        StartCharge();
                    }
                    else
                    { 
                        if (engragedChargeTimer <= 0)
                        {
                            //no wait time between charges when enraged
                            engragedChargeTimer = enragedChargeTime;
                            
                            isCharging = false;
                        }
                        else
                        {
                            engragedChargeTimer -= Time.deltaTime;
                        }
                    }

                    if (rangedAttackTimer <= 0)
                    {
                            RangedAttack();
                            rangedAttackTimer = rangedAttackInterval;
                    }
                    else
                    {
                        rangedAttackTimer -= Time.deltaTime;
                    }
                    break;
              
                //while state is defensive the enemy will try to maintain a certain distance from the player and fire ranged attacks
                case State.defensive:
                    if (distanceToPlayer < safeDistance)
                    {
                        Retreat();
                    }
                    else
                    {
                        if (rangedAttackTimer <= 0)
                        {
                            direction = Vector2.zero;
                            RangedAttack();
                            rangedAttackTimer = rangedAttackInterval;
                        }
                        else
                        {
                            rangedAttackTimer -= Time.deltaTime;
                        }
                    }
                    break;
              
                //while agressive the enemy will charge at the player with a small interval between charges
                case State.aggressive:
                    if (!isCharging)
                    {
                        StartCharge();
                    }
                    else
                    {
                        if (chargeTimer <= 0)
                        {
                            //speed -= 3;
                            chargeTimer = chargeTime;
                            //currentState = State.waiting;
                            isCharging = false;
                        }
                        else
                        {
                            chargeTimer -= Time.deltaTime;
                        }
                    }
                    break;
              
                //waiting state is used for interval between charges while the enemy is agressive
                case State.waiting:
                    //stops movement while waiting
                    direction = Vector3.zero;
                    if(waitTimer <= 0)
                    {
                        //resets the enemy's state to agressive to start the next charge
                        currentState = State.aggressive;
                        waitTimer = waitTime;
                    }
                    else
                    {
                        waitTimer -= Time.deltaTime;
                    }
                    break;
            }
        }
        else
        {
            distanceToPlayer = Vector3.Magnitude(transform.position - player.Position);
            if (distanceToPlayer <= agroRange)
            {
                isAgro = true;
            }
        }
    }

//Player controller code
void Update()
    {
        Movement();

        if (fireTimer >= 0)
        {
            fireTimer -= Time.deltaTime;
        }
        if(immunityTimer >= 0)
        {
            immunityTimer -= Time.deltaTime;
        }
    }

//updates player position
    private void Movement()
    {
        direction = playerControls.ReadValue<Vector2>();

        velocity = new Vector3(direction.x * moveSpeed, direction.y * moveSpeed, 0);
        position += velocity * Time.deltaTime;
     
    
        StayInBounds();
     
        transform.position = position;
        bounds.center = position;
    }

//room manager class update loop
    void Update()
    {
        if(player.Room != this)
        {
            if (roomBounds.Contains(player.Position))
            {
               player.Room = this;
            }
        }
     
        if(enemies.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.Health <= 0)
                {
                    enemies.Remove(enemy);
                    enemy.gameObject.SetActive(false);
                }
            }
        }
     
        //when no enemies remain sets the exit to destructable and updates the color to signal the player they can move on
        if(enemies.Count <= 0 && exit != null)
        {
            exit.Destructable = true;
            exit.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

 //causes projectile to deal damage to every enemy within a certain radius upon hitting an enemy, meant to create an effect of a ball of acid exploading against a surface
 //only triggered on hitting an enemy to reward good play with additional damage
    private void explosion()
    {
        foreach(Enemy enemy in currentRoom.enemies)
        {
            float distance = Vector3.Magnitude(enemy.Position - position);
            if(distance < explosionRadius)
            {
                enemy.TakeDamage(1);
                enemy.PoisonCounter++;
            }
        }
    }

//calculated rotation to aim enemy spears towards player
 public void AdjustRotation()
    {
        //draws a vector between the spear and player
        Vector3 vecToPlayer = player.Position - transform.position;

        //gets the angle to rotate based on the vector between the player and current position
        float angleToRotate = Mathf.Atan2(vecToPlayer.y, vecToPlayer.x) * Mathf.Rad2Deg;
       
        //updates the rotation using a quaternion
        transform.rotation = Quaternion.Euler(0, 0, angleToRotate);
        bounds.center = transform.position;
    }
