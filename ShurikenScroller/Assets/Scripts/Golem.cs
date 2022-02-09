using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Monster
{
    public float projectileSpeed = 5f;
    private float fireRate = 1.5f;
    private float fireTimer = 0;
    public SceneManager sceneManager;
    public int damage = 3;
    public Wave leftWave;
    public Wave rightWave;
    // Start is called before the first frame update
    void Start()
    {
        //sets fields
        agroRange = 1000;
        isAlive = true;
        damage = 2;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
    }

    

    public override void UpdateHolder()
    {
        Update();

        if(isAgro && fireTimer <= 0)
        {
            Attack();

            fireTimer = fireRate;
        }

        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

    }

    private void Attack()
    {
        //player is to the left of the golumn
        if(player.Position.x < transform.position.x)
        {
            //new wave is okay as a musically genre
            Wave newWave = Instantiate(leftWave);

        }
        //player is to the right
        else
        {

        }
    }
}
