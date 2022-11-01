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

        spear2.Player = player;
        spear2.PlayerBounds = player.gameObject.GetComponent<SpriteRenderer>().bounds;

        rangedAttackInterval = 0.75f;
        currentState = State.aggressive;

        chargeTime = 1.5f;
        waitTime = 0.75f;
        chargeTimer = chargeTime;
        waitTimer = waitTime;

        enragedChargeTime = 1.0f;
        engragedChargeTimer = enragedChargeTime;
        standardSpeed = speed;
        enragedSpeed = speed * 1.5;
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Magnitude(transform.position - player.Position);
        if (distanceToPlayer <= agroRange)
        {
            isAgro = true;
        }

        //Debug.Log(currentState);

        if (isAgro)
        {
            Debug.Log(currentState);
            
            base.Update();
            spear.AdjustRotation();
            spear2.AdjustRotation();

            switch (currentState)
            {
                case State.enraged:

                    if (!isCharging)
                    {
                        StartCharge();


                    }
                    else
                    {
                        //speed = 10;

                        //velocity = new Vector3(direction.x * speed * 3, direction.y * speed * 3, 0.0f);

                        if (engragedChargeTimer <= 0)
                        {
                            //speed -= 3;
                            engragedChargeTimer = enragedChargeTime;
                            //currentState = State.waiting;
                            isCharging = false;
                        }
                        else
                        {
                            engragedChargeTimer -= Time.deltaTime;
                        }

                        

                    }

                    if (rangedAttackTimer <= 0)
                    {
                            //direction = Vector2.zero;
                            RangedAttack();
                            rangedAttackTimer = rangedAttackInterval;

                    }
                    else
                    {
                        rangedAttackTimer -= Time.deltaTime;
                    }

                  
                    //Debug.Log(velocity);
                    break;
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
                case State.aggressive:

                    if (!isCharging)
                    {
                        StartCharge();
                        

                    }
                    else
                    {
                        //speed = 10;

                        //velocity = new Vector3(direction.x * speed , direction.y * speed, 0.0f);

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

                        if (currentState == State.aggressive)
                        {

                        }

                    }
                    /*
                    else
                    {
                        //speed = 8;
                    }
                    */

                    break;
                case State.waiting:
                    direction = Vector3.zero;
                    if(waitTimer <= 0)
                    {
                        currentState = State.enraged;
                        waitTimer = waitTime;
                    }
                    else
                    {
                        waitTimer -= Time.deltaTime;
                    }
                    break;
            }
        }
    }

    //fires three projectiles stacked on top of each other
    protected override void RangedAttack()
    {
        float separation = -0.5f;

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
        direction = player.Position - transform.position;
        direction.Normalize();
        //speed += 3;

    }

    protected override void SetState()
    {
        int selector;
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

    protected override void SetBehavior()
    {
        //currently unused
    }

}
