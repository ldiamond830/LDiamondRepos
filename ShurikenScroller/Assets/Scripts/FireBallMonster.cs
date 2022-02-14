using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMonster : Monster
{
    public FireBall fireBall;
    public float projectileSpeed = 5f;
    private float fireRate = 2f;
    private float fireTimer = 0;
    public SceneController sceneManager;
    public Camera cameraObject;
    // Start is called before the first frame update
    void Start()
    {
        //sets fields
        agroRange = cameraObject.orthographicSize * 2f * cameraObject.aspect; 
        isAlive = true;
        damage = 2;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void UpdateHolder()
    {
        //calls update in parent class to check if player is within range
        isAgro = agroCheck();
        //if the player is in range and the monster is ready to fire creates a new fireball and resets the fire timer
        if (isAgro && fireTimer <= 0)
        {
            Attack();
            fireTimer = fireRate;
        }

        //if the monster isn't ready to fire
        if(fireTimer > 0)
        {
            //reduces fireTimer
            fireTimer -= Time.deltaTime;
        }
    }

    void Attack()
    {
        //creates a new fireball
        FireBall newFireBall = Instantiate(fireBall);
        //adds it to the sceneManagers list so it can have collision detection run on it
        sceneManager.fireBallList.Add(newFireBall);
        newFireBall.speed = projectileSpeed;
        //creates a new fireball above the head of the enemy
        newFireBall.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.0f, 0);
        newFireBall.direction = player.Position - newFireBall.position;
        newFireBall.direction = newFireBall.direction.normalized;
        newFireBall.damage = this.damage;
        newFireBall.spriteRenderer = newFireBall.GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.0f, 0), 1);
    }
}
