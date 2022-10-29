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
    private CircleCollider2D collider;

    private bool hasLanded;

    private float startingY;

    private float explosionRadius;

    private float sumTime = 0.0f;

   [SerializeField]
    private float totalGroundTime;
    private float currentGroundTime;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(1.0f, 0.0f, 0.0f);
        hasLanded = false;
        position = transform.position;
        startingY = position.y;
        collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLanded)
        {
            if(currentGroundTime >= totalGroundTime)
            {
                gameObject.SetActive(false);
            }

            currentGroundTime += Time.deltaTime;
        }
        else
        {
            
            direction.y = calculateArc(sumTime);

            
            velocity = new Vector3(direction.x * moveSpeed, direction.y , 0);
            position += velocity * Time.deltaTime;
            transform.position = position;

            //stops moving when it reaches the same height as it started at in the arc or hits an enemy
            if(position.y <= startingY - 0.1 || CollisionCheck())
            {
                hasLanded = true;
                explosion();
            }

            sumTime += Time.deltaTime * 2;

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

    private bool CollisionCheck()
    {
        foreach (Enemy enemy in enemyManager.enemies)
        {
            if (collider.IsTouching(enemy.Collider))
            {
                return true;
            }
        }
        return false;
    }

    private void explosion()
    {
        foreach(Enemy enemy in enemyManager.enemies)
        {
            float distance = Vector3.Magnitude(enemy.Position - position);
            if(distance < explosionRadius)
            {
                Debug.Log("hit");
            }
        }
    }
}
