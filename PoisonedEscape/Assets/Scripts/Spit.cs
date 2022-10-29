using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;
    public float moveSpeed;

    public EnemyManager enemyManager;
    
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
        
        
        spitBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLanded)
        {
            if(currentGroundTime >= totalGroundTime)
            {
                gameObject.SetActive(false);
                this.enabled = false;
            }

            if (hitTimer <= 0)
            {
                if(enemyManager.enemies.Count > 0)
                {
                    foreach (Enemy enemy in enemyManager.enemies)
                    {
                        if (CollisionCheck(enemy.enemyBounds))
                        {
                            enemy.PoisonCounter++;
                            //Debug.Log("hit");
                            hitTimer = hitInterval;
                        }

                    }
                }
               
            }
            else
            {
                hitTimer -= Time.deltaTime;
            }
            

            currentGroundTime += Time.deltaTime;
        }
        else
        {
            
            direction.y = calculateArc(sumTime);

            
            velocity = new Vector3(direction.x * moveSpeed, direction.y , 0);
            position += velocity * Time.deltaTime;
            transform.position = position;
            spitBounds.center = position;

            //stops moving when it reaches the same height as it started at in the arc or hits an enemy
            if(position.y <= startingY - 0.1 )
            {
                hasLanded = true;
                transform.localScale *= 3;
                
            }

            foreach(Enemy enemy in enemyManager.enemies)
            {
                if (CollisionCheck(enemy.enemyBounds))
                {
                    //if the projectile hits an enemy in mid air it does damage to every enemy in a radius and lands
                    explosion();
                    hasLanded = true;
                    transform.localScale *= 3;
                }
            }
            

            sumTime += Time.deltaTime * 2;

            if(CollisionCheck(enemyManager.exit.GateBounds) && enemyManager.exit.Destructable)
            {
                enemyManager.exit.IsActive = false;
                gameObject.SetActive(false);
                this.enabled = false;
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



    
    private bool CollisionCheck(Bounds other)
    {
        return spitBounds.Intersects(other);
    }

    

   
    private void explosion()
    {
        foreach(Enemy enemy in enemyManager.enemies)
        {
            float distance = Vector3.Magnitude(enemy.Position - position);
            if(distance < explosionRadius)
            {
                enemy.TakeDamage(1);
                enemy.PoisonCounter++;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(position, spitBounds.size);
        
    }
}



