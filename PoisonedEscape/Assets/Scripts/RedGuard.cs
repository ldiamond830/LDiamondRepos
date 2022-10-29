using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGuard : Enemy
{
    [SerializeField]
    private SpearController spear;
    [SerializeField]
    private float meleeRange;

    private float rangedAttackInterval;
    private float rangedAttackTimer;
    private bool isMeleeing;

    public EnemyProjectile projectileTemplate;

    // Start is called before the first frame update
    void Start()
    {
        
        base.Start();
        timeToStateChange = 2;
        currentState = State.aggressive;
        speed = 1;
        spear.Player = player;
        spear.PlayerBounds = player.gameObject.GetComponent<SpriteRenderer>().bounds;
        distanceToPlayer = Vector3.Magnitude(player.Position - transform.position);
        rangedAttackInterval = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(distanceToPlayer <= agroRange)
        {
            base.Update();

            switch (currentState)
            {
                case State.aggressive:
                    Approach();


                    break;


                case State.defensive:
                   
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

                default:
                    Debug.Log("error in enemy state setting");
                    break;

            }

        }


    }


    private void MeleeAttack()
    {
        isMeleeing = true;
        spear.Position = transform.position;

    }

   
    protected override void RangedAttack()
    {
        EnemyProjectile projectile = Instantiate(projectileTemplate);
        //already have the logic to make the spear rotate to the player, so no need to copy it for aiming the projectile
        projectile.transform.rotation = spear.transform.rotation;

        projectile.Direction = Vector3.Normalize(player.Position - position);




    }


    protected override void SetState()
    {
        int selector = Random.Range(0, 3);

        if(health <= 3)
        {
            currentState = State.defensive;

        }
        else if(health > 3 && health <= 5)
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

    //unused currently
    protected override void SetBehavior()
    {
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
    }
}
