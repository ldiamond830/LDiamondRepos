using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMonster : Monster
{
    public FireBall fireBall;
    public float projectileSpeed = 5f;
    private float fireRate = 1.5f;
    private float fireTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        agroRange = 300;
        isAlive = true;
        damage = 2;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void UpdateHolder()
    {
        Update();
        if (isAgro && fireTimer <= 0)
        {
            Attack();
            fireTimer = fireRate;
        }

        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    void Attack()
    {
        FireBall newFireBall = Instantiate(fireBall);
        newFireBall.speed = projectileSpeed;
        //creates a new fireball above the head of the enemy
        newFireBall.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.5f, 0);
        newFireBall.direction = player.Position - newFireBall.position;
        newFireBall.direction = newFireBall.direction.normalized;
    }
}
