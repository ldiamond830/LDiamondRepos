using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;
    public float moveSpeed;
    private Bounds bounds;
    private Bounds roomBounds;
    private bool stop;
    private PlayerController player;

   
    private float deSpawnTimer;

    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }
    public PlayerController Player
    {
        set { player = value; }
    }
    public Bounds RoomBounds
    {
        set { roomBounds = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        //projectiles will disappear after 3 seconds
        deSpawnTimer = 3.0f;

        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        position = transform.position;
        stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            
            velocity = direction * moveSpeed;
            position += velocity * Time.deltaTime;
            transform.position = position;
            bounds.center = position;


            if (CollisionCheck())
            {
                player.Health--;
                gameObject.SetActive(false);
                this.enabled = false;
            }

            StayInBounds();
        }
        else
        {
            if(deSpawnTimer <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                deSpawnTimer -= Time.deltaTime;
            }
        }
        

        
    }


    private bool CollisionCheck()
    {
        if (bounds.Intersects(player.PlayerBounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StayInBounds()
    {
        if((position.x + bounds.extents.x > roomBounds.max.x
            || position.y + bounds.extents.y > roomBounds.max.y
            || position.y - bounds.extents.y < roomBounds.min.y
            || position.x - bounds.extents.x < roomBounds.min.x)){
            //stops the projectile moving when it hits a wall
            stop = true;
        }
    }

}
