using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlueGuard : Enemy
{

    private bool isCharging;
    private float chargeTime;
    private float chargeTimer;

    private float waitTime;
    private float waitTimer;

    private float enragedChargeTime;
    private float engragedChargeTimer;
    private int enragedSpeed;
    private int standardSpeed;

    [SerializeField]
    private SpearController spear2;

    public override void PublicStart()
    {
        OnStart();
    }

    // Start is called before the first frame update
    protected override void OnStart()
    {
        base.OnStart();

        //initializes the 2nd spear, spear1 is handled by base.Start();
        spear2.Player = player;
        spear2.PlayerBounds = player.gameObject.GetComponent<SpriteRenderer>().bounds;

        //setting default values
        rangedAttackInterval = 0.75f;
        currentState = State.aggressive;

        chargeTime = 1.5f;
        waitTime = 0.75f;
        chargeTimer = chargeTime;
        waitTimer = waitTime;

        enragedChargeTime = 1.0f;
        engragedChargeTimer = enragedChargeTime;
        standardSpeed = speed;
        enragedSpeed = speed * 2;
        
    }

    // Update is called once per frame
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

                  
                    //Debug.Log(velocity);
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
            //checks if the player is within agro range, if so combat behaviors are triggered
            distanceToPlayer = Vector3.Magnitude(transform.position - player.Position);
            if (distanceToPlayer <= agroRange)
            {
                isAgro = true;
            }
        }
    }

    //fires three projectiles stacked on top of each other
    protected override void RangedAttack()
    {
        float separation = -0.5f;
        //creates three projectiles separated by and interval of 0.5 on the y axis
        for(uint i = 0; i < 3; i++)
        {
            EnemyProjectile projectile = Instantiate(projectileTemplate);
            //already have the logic to make the spear rotate to the player, so no need to copy it for aiming the projectile
            projectile.transform.rotation = spear.transform.rotation;

            projectile.transform.position = new Vector3(transform.position.x, transform.position.y + separation, 0.0f);

            projectile.Direction = Vector3.Normalize(player.Position - position);
            projectile.Player = player;
            projectile.RoomBounds = boundaries;
            separation += 0.5f;

        }
    }

    private void StartCharge()
    {
        isCharging = true;
        //gets the direction vector to the player at the start of the charge
        direction = player.Position - transform.position;
        direction.Normalize();
        

    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Position, agroRange);
        Gizmos.DrawWireSphere(Position, safeDistance);
    }

    

}
