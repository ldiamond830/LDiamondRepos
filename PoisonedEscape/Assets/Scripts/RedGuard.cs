using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGuard : Enemy
{
    //private bool isMeleeing;


    public override void PublicStart()
    {
        OnStart();
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        
        base.OnStart();
        timeToStateChange = 2;
        currentState = State.aggressive;
        speed = 1;
        
        distanceToPlayer = Vector3.Magnitude(transform.position - player.Position);
        rangedAttackInterval = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {


        distanceToPlayer = Vector3.Magnitude(transform.position - player.Position);
        if(distanceToPlayer <= agroRange)
        {
            isAgro = true;
        }

        if (isAgro)
        {

            spear.AdjustRotation();

            base.Update();

            switch (currentState)
            {
                case State.aggressive:
                    Approach();


                    break;


                case State.fearful:
                   
                        if (rangedAttackTimer <= 0)
                        {
                            RangedAttack();
                            rangedAttackTimer = rangedAttackInterval;

                        }
                        else
                        {
                            Retreat();
                            rangedAttackTimer -= Time.deltaTime;
                        }
                    

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

                default:
                    Debug.Log("error in enemy state setting");
                    break;

            }

        }


    }


    

   
    protected override void RangedAttack()
    {
        EnemyProjectile projectile = Instantiate(projectileTemplate);
        //already have the logic to make the spear rotate to the player, so no need to copy it for aiming the projectile
        projectile.transform.rotation = spear.transform.rotation;

        projectile.transform.position = transform.position;

        projectile.Direction = Vector3.Normalize(player.Position - position);
        projectile.Player = player;
        projectile.RoomBounds = boundaries;


    }


    protected override void SetState()
    {
        int selector = Random.Range(0, 3);

        if(Health <= 3)
        {
            if(selector!= 0)
            {
                currentState = State.defensive;
            }
            else
            {
                currentState = State.fearful;
            }

        }
        else if(Health > 3 && Health <= 5)
        {
            //at half health or less enemies have a reduce chance to be agressive
            if(selector == 1)
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
            
                currentState = State.aggressive;
            
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanceToPlayer);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agroRange);

    }

    //unused currently
    protected override void SetBehavior()
    {
        /*
        int selector = Random.Range(0, 2);
        float distanceToPlayer = Vector3.Magnitude(position - player.Position);

        switch (currentState)
        {
            case State.aggressive:
                //if the enemy is within melee range has a chance to do a melee attack
                if(distanceToPlayer < meleeRange)
                {
                    MeleeAttack();
                }
                else
                {
                    if (selector == 0)
                    {
                        RangedAttack();
                    }
                    else
                    {
                        Approach();
                    }
                }
            break;


            case State.defensive:
               if(distanceToPlayer < meleeRange)
               {
                    Retreat();
               }
                else
                {
                    if(selector == 0)
                    {
                        RangedAttack();
                    }
                    else
                    {
                        Retreat();
                    }
                }

                break;

            default:
                Debug.Log("error in enemy state setting");
                break;
       
        }
         */
    }
}
