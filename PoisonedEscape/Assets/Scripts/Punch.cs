using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    isPunching,
    isReturning,
    wait
}

public class Punch : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;

    private Vector3 distanceFromPlayer;
    private Vector3 playerPos;
    private float defaultDis;
    private Vector3 returnPos;

    //stats
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float range;

    private State currentState;


    public EnemyManager enemyManager;
    private BoxCollider2D collider;

    public Vector3 Direction
    {
        set { direction = value; }
        get { return direction; }
    }
    public State CurrentState
    {
        set { currentState = value; }
        get { return currentState; }
    }

   public Vector3 PlayerPos
   {
        set { playerPos = value; }
   }

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        currentState = State.wait;
        defaultDis = 0.4f;
       
        collider = gameObject.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.isPunching:
                //moves the player based on speed value, read in direction and scales by delta time
                velocity = new Vector3(direction.x * moveSpeed, direction.y * moveSpeed, 0);

                reposition();

                 distanceFromPlayer = playerPos - position;

                if (CollisionCheck())
                {
                    Debug.Log("usdgahasdf");
                }

                if(Vector3.Magnitude(distanceFromPlayer) >= range)
                {
                    currentState = State.isReturning;
                    
                    
                }

                break;

            case State.isReturning:
                direction = playerPos - position;

                //returns faster than it moves out
                velocity = new Vector3(direction.x * moveSpeed * 2, direction.y * moveSpeed * 2, 0);
                //velocity = Vector3.Lerp( playerPos, position, Time.deltaTime);

                reposition();

                distanceFromPlayer = playerPos - position;
                
                if(Vector3.Magnitude(distanceFromPlayer) <= defaultDis * 2)
                {
                    returnPos = position;
                    currentState = State.wait;
                    
                }
                
                break;

            case State.wait:
                //position = playerPos;
                //transform.position = returnPos;
                break;
                  

        }
           
        
    }

    private void reposition()
    {
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    private bool CollisionCheck()
    {
        foreach(Enemy enemy in enemyManager.enemies)
        {
            if (collider.IsTouching(enemy.Collider))
            {
                return true;
            }
        }
        return false;
    }
}
