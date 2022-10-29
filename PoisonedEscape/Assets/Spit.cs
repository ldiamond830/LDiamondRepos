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
    private BoxCollider2D collider;

    private bool hasLanded;

    private float startingY;

    float sumTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(1.0f, 0.0f, 0.0f);
        hasLanded = false;
        position = transform.position;
        startingY = position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLanded)
        {

        }
        else
        {
            
            direction.y = calculateArc(sumTime);
            velocity = new Vector3(direction.x * moveSpeed, direction.y * 2, 0);
            position += velocity * Time.deltaTime;
            transform.position = position;

            //stops moving when it reaches the same height as it started at in the arc
            if(position.y <= startingY - 0.1)
            {
                hasLanded = true;
            }

            sumTime += Time.deltaTime;

        }
    }

    //moves along a parabola
    private float calculateArc(float x)
    {
        x -= 2;
        Mathf.Pow(x, 2);
        x *= -1;
        x += 1;

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
}
