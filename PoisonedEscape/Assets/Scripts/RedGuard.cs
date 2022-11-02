using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGuard : Enemy
{
    
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


        

        if (isAgro)
        {

            spear.AdjustRotation();

            base.Update();

            switch (currentState)
            {
                //moves toward the player while agressive
                case State.aggressive:
                    Approach();


                    break;

                    //tries to get as far from the player and fires projectiles while fearful
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
                    //tries to maintain a certain distances and fires projectiles while defensive
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

               

            }

        }
        //checks if the player is within agro range
        else
        {
            distanceToPlayer = Vector3.Magnitude(transform.position - player.Position);
            if (distanceToPlayer <= agroRange)
            {
                isAgro = true;
            }
        }


    }


    

   //creates a new projectile and initializes its values
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

    //sets the enemy behavior based on a combination of chance and remaining health
    protected override void SetState()
    {
        int selector = Random.Range(0, 3);
        //at 3 health enemy has no chance to be agressive
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
            //at 5 health or less enemies have a reduce chance to be agressive
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

   
}
