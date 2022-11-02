using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //movement fields
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;

    //stat fields
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int damage;

    //collision fields
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

        //initializing values
        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        position = transform.position;
        stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            //updates the position and bounds based on the movement calculation
            velocity = direction * moveSpeed;
            position += velocity * Time.deltaTime;
            transform.position = position;
            bounds.center = position;


            if (CollisionCheck())
            {
                //because each projectile can only hit once doesn't give the player invicibilty frames like the enemies spear
                player.TakeDamage(damage);
                //disables projectile after it hits the player and does damage
                gameObject.SetActive(false);
                this.enabled = false;
            }

            StayInBounds();
        }
        else
        {
            //projectile will wait on the floor for a short period before disapearing
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

    //checks if the bounds of the sprite is intersecting that of the player
    private bool CollisionCheck()
    {
        return bounds.Intersects(player.PlayerBounds);
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
