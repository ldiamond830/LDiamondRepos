using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    aggressive,
    defensive,
   
}

//holds behaviors and data needed by all enemies
public abstract class Enemy : MonoBehaviour
{
    //stats
    public int health;
    public int speed;
    public PlayerController player;
    public float agroRange;
    protected float distanceToPlayer;
    

    protected int poisonCounter;

    public Bounds enemyBounds;


    protected Vector2 direction = Vector2.zero;
    protected Vector3 velocity = Vector2.zero;
    protected Vector3 position;

    protected State currentState;

    protected float timeToStateChange;
    protected float stateTimer;
    protected float poisonInterval;
    protected float poisonTimer;


    public int PoisonCounter
    {
        get { return poisonCounter; }
        set { poisonCounter = value; }
    }
  
    public Vector3 Position
    {
        get { return position; }
    }
    public int Health
    {
        set { health = value; }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        poisonInterval = 1.0f;
        enemyBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
    }

    // Update is called once per frame
    protected void Update()
    {
        //update position and hitbox;
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        transform.position = position;
        enemyBounds.center = position;

        if(stateTimer >= timeToStateChange)
        {
            SetState();
            //SetBehavior();
           stateTimer = 0;
        }
        else
        {
            stateTimer += Time.deltaTime;
        }

        

        if (poisonCounter > 0)
        {
            if(poisonTimer <= 0)
            {
                health -= poisonCounter;
                poisonTimer = poisonInterval;
            }
            else
            {
                poisonTimer -= Time.deltaTime;
            }
        }
    }

    protected abstract void SetBehavior();

    protected abstract void SetState();

    //move toward the player
    protected void Approach()
    {
        direction = player.Position - position;
        direction = direction.normalized;


    }

    //move away from the player
    protected void Retreat()
    {
        direction = player.Position - position;
        direction = direction.normalized * -1;
    }

    protected abstract void RangedAttack();
    
}
