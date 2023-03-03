//enemy attack hitbox behavior
private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            _playerInHitBox = true;
               
            preAttackState = EnemyState.CHASE;
        }
        else if (other.gameObject.tag == "PlayerWall")
        {
            preAttackState = enemy.enemyState;
            enemy.enemyState = EnemyState.ATTACK;
            _wallInHitBox = true;
            wall = other.gameObject;
            Invoke("DestroyWall", destroyWallTime);

        } 
    }
    
    //enemy attack loop
    void Update()
    {
        if (_playerInHitBox && enemy.enemyState == EnemyState.ATTACK)
        {
            attackSequenceCoolDown -= Time.deltaTime;
            
            if(attackSequenceCoolDown <= 0)
            {
                AttackSequence();
            }
        }
        else if(_playerInHitBox && enemy.enemyState == EnemyState.CHASE)
        {
            enemy.enemyState = EnemyState.ATTACK;
        }
    }
    
     public void ChasePlayer()
    {
        audioManager.enemyChaseSound();

        NavMeshHit hit;
        //if the player model size is changed the value of 2.0f will also need to be changed in relation or the function breaks
        if (NavMesh.SamplePosition(player.transform.position, out hit, 2.0f, 1 << NavMesh.GetAreaFromName("Walkable")) && (distanceToPlayer < enemy.GetComponent<EnemyAIDetection>().dangerDistance))
        {
            agent.SetDestination(player.transform.position);
            agent.stoppingDistance = 2.3f;
            transform.LookAt(new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z));
            demonAnim.SetFloat("Speed", 4);
        }
    }
    
   
 private void RandomScan()
    {
        randomScanTimer += Time.deltaTime;

        if(randomScanTimer >= timeTillRandomScan)
        {
            randomScanTimer = 0.0f;
            timeTillRandomScan = Random.Range(10.0f, 15.0f);
            enemyScan = true;
            enemyProximityDetection = true;
            enemyAIPatrol.enemyState = EnemyState.SCAN;
            audioManager.enemyScanSound();
        }
    }
