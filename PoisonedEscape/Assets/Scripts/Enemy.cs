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

    

    protected BoxCollider2D collider;


    protected Vector2 direction = Vector2.zero;
    protected Vector3 velocity = Vector2.zero;
    protected Vector3 position;

    protected State currentState;

    protected float timeToStateChange;
    protected float stateTimer;

    
    public BoxCollider2D Collider
    {
        get { return collider; }
    }
    public Vector3 Position
    {
        get { return position; }
    }
    // Start is called before the first frame update
    protected void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        velocity = direction * speed;
        position += velocity;
        transform.position = position;

        if(stateTimer >= timeToStateChange)
        {
            SetState();
        }
        else
        {
            stateTimer += Time.deltaTime;
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

    protected void RangedAttack()
    {

    }
}
