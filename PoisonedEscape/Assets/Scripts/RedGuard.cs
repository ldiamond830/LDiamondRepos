using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGuard : Enemy
{
    [SerializeField]
    private float meleeRange;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        currentState = State.aggressive;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void MeleeAttack()
    {

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
                currentState = State.aggressive;
            }
        }
        else
        {
            if(selector <= 1)
            {
                currentState = State.aggressive;
            }
        }

    }


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
                    if (selector == 0)
                    {
                        MeleeAttack();
                    }
                    else
                    {
                        Approach();
                    }
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
