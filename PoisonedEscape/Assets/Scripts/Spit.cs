using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    [SerializeField]
    private Sprite groundSprite;
    private SpriteRenderer spriteRenderer;

    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;
    public float moveSpeed;

    public EnemyManager currentRoom;
    
    private Bounds spitBounds;
    //broken
    //private CircleCollider2D collider;

    private bool hasLanded;

    private float startingY;
    [SerializeField]
    private float explosionRadius;

    private float sumTime = 0.0f;

   [SerializeField]
    private float totalGroundTime;
    private float currentGroundTime;

    private float hitTimer;
    private float hitInterval;

    public AudioSource acidHiss;
   

    public Vector3 Direction
    {
        set { direction = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hitInterval = 0.5f;
        hitTimer = 0.0f;
        hasLanded = false;
        position = transform.position;
        startingY = position.y;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        

        spitBounds = spriteRenderer.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLanded)
        {
            //despawns after a certain period
            if(currentGroundTime >= totalGroundTime)
            {
                gameObject.SetActive(false);
                this.enabled = false;
            }

            //checks enemy collisions
            if (currentRoom.enemies.Count > 0)
            {
                if (hitTimer <= 0)
                {

                    foreach (Enemy enemy in currentRoom.enemies)
                    {
                        if (CollisionCheck(enemy.enemyBounds))
                        {
                            //adds a stack of poison to the hit enemy
                            enemy.PoisonCounter++;
                            hitTimer = hitInterval;
                        }

                    }


                }
                else
                {
                    hitTimer -= Time.deltaTime;
                }
            }
            

            currentGroundTime += Time.deltaTime;
        }
        else
        {
            //prevents the projectile going out of boudns
            BoundsCheck();
            
            //updates the direction based on how long the projectile has been in the air
            direction.y = calculateArc(sumTime);

            //updates position
            velocity = new Vector3(direction.x * moveSpeed, direction.y , 0);
            position += velocity * Time.deltaTime;
            transform.position = position;
            spitBounds.center = position;

            //stops moving when it reaches a similar height as it started at in the arc or hits an enemy
            if(position.y <= startingY - 0.1 )
            {
                Land();

            }
            //enemy collision check
            if(currentRoom.enemies.Count > 0)
            {
                foreach (Enemy enemy in currentRoom.enemies)
                {
                    if (CollisionCheck(enemy.enemyBounds))
                    {
                        //if the projectile hits an enemy in mid air it does damage to every enemy in a radius and lands
                        explosion();
                        //lands upon hitting an enemy
                        Land();

                    }
                }
            }
            
            
            
            //added the *2 while experiementing to make the arc move more smoothly
            sumTime += Time.deltaTime * 2;

            //check collision with the exit to each room to allow the player to shoot them down
            if (currentRoom.exit.IsActive)
            {
                if (CollisionCheck(currentRoom.exit.GateBounds) && currentRoom.exit.Destructable)
                {
                    
                    if (acidHiss.clip != null)
                    {
                        acidHiss.Play();
                    }

                    currentRoom.exit.IsActive = false;
                    gameObject.SetActive(false);
                    this.enabled = false;

                }


            }
        }
    }

    //moves along a parabola
    private float calculateArc(float x)
    {
        x -= 2;
        Mathf.Pow(x, 2);
        x *= -1;
        //x += 1;

        return x;
    }

    //updates the sprite and sets bool when projectile "hits the ground" or hits an enemy
    private void Land()
    {
        hasLanded = true;
        spriteRenderer.sprite = groundSprite;
    }

    
    private bool CollisionCheck(Bounds other)
    {
        return spitBounds.Intersects(other);
    }

    

   //deals damage to every enemy within a certain radius upon hitting an enemy, meant to create an effect of a ball of acid exploading against a surface
   //only triggered on hitting an enemy to reward good play with additional damage
    private void explosion()
    {
        foreach(Enemy enemy in currentRoom.enemies)
        {
            float distance = Vector3.Magnitude(enemy.Position - position);
            if(distance < explosionRadius)
            {
                enemy.TakeDamage(1);
                enemy.PoisonCounter++;
            }
        }
    }

    //if the projectile hits a wall it just lands there against the wall
    private void BoundsCheck()
    {
        if (position.x + spitBounds.extents.x > currentRoom.RoomBounds.max.x 
            || position.y + spitBounds.extents.y > currentRoom.RoomBounds.max.y
            || position.y - spitBounds.extents.y < currentRoom.RoomBounds.min.y
            || position.x - spitBounds.extents.x < currentRoom.RoomBounds.min.x)
        {
            Land();
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(position, spitBounds.size);
        
    }
}



