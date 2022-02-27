using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Monster
{
    public float projectileSpeed = 5f;
    private float fireRate = 3.5f;
    private float fireTimer = 0;
    public SceneController sceneManager;
    public Wave leftWave;
    public Wave rightWave;
    public Camera cameraObject;
  
    // Start is called before the first frame update
    void Start()
    {
        //sets fields
        //agroRange = cameraObject.orthographicSize * 2f * cameraObject.aspect;
        isAlive = true;
        damage = 2;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        pointValue = 5;
        
    }

    

    public override void UpdateHolder()
    {
        isAgro = agroCheck();

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

    //creates a new wave
    private void Attack()
    {
        //player is to the left of the golumn
        if(player.Position.x < transform.position.x)
        {
            //new wave is okay as a musically genre
            Wave newWave = Instantiate(leftWave);
            newWave.damage = this.damage;
            newWave.position = new Vector3(this.transform.position.x, -1, 0); 
            newWave.direction = new Vector3(-1, 0, 0);
            sceneManager.waveList.Add(newWave);

        }
        //player is to the right
        else
        {
            Wave newWave = Instantiate(rightWave);
            newWave.damage = this.damage;
            newWave.position = new Vector3(this.transform.position.x, -1, 0);
            newWave.direction = new Vector3(1, 0, 0);
            sceneManager.waveList.Add(newWave);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, agroRange);
    }
}
