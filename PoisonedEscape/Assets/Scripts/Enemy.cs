using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    aggressive,
    defensive,
    fearful,
    enraged,
    waiting
}

//holds behaviors and data needed by all enemies
public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float safeDistance;
    protected float rangedAttackInterval;
    protected float rangedAttackTimer;
    [SerializeField]
    protected EnemyProjectile projectileTemplate;

    [SerializeField]
    protected SpearController spear;

    [SerializeField]
    private SpriteRenderer poisonIndicator;

    
    private AudioSource hitSound;

    public Slider healthBar;

    public GameObject room;
    protected Bounds boundaries;

    //stats
    public float maxHealth;
    private float health;
    public int speed;
    public PlayerController player;
    public float agroRange;

    protected float distanceToPlayer;

    protected bool isAgro;

    protected int poisonCounter;

    public Bounds enemyBounds;

    //movement 
    protected Vector2 direction = Vector2.zero;
    protected Vector3 velocity = Vector2.zero;
    protected Vector3 position;

    protected State currentState;

    protected float timeToStateChange;
    protected float stateTimer;
    protected float poisonInterval;
    protected float poisonTimer;

    protected bool damagable;

    //properties
    public AudioSource HitSound
    {
        set { hitSound = value; }
    }

    public int PoisonCounter
    {
        get { return poisonCounter; }
        set { poisonCounter = value; }
    }
  
    public Vector3 Position
    {
        get { return position; }
    }
    public float Health
    {
        set { health = value; }
        get { return health; }
    }

    

    
    protected virtual void OnStart()
    {

        //setting base values
        position = transform.position;
        health = maxHealth;

        UpdatedHealthBar();
        poisonInterval = 2.75f;
        enemyBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        boundaries = room.GetComponent<SpriteRenderer>().bounds;
        /*
        cameraHeight = cameraObject.orthographicSize * 2f;
        cameraWidth = cameraHeight * cameraObject.aspect;
        */
        isAgro = false;
       
        spear.Player = player;
        spear.PlayerBounds = player.gameObject.GetComponent<SpriteRenderer>().bounds;
    }

    // Update is called once per frame
    protected void Update()
    {
        //update position and hitbox;
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        transform.position = position;
        enemyBounds.center = new Vector3(position.x, position.y, 0.0f);

        

        

        if (poisonCounter > 0)
        {
            //makes an icon visible to show that the enemy has been poisoned
            if (!poisonIndicator.enabled)
            {
                poisonIndicator.enabled = true;
            }

            if(poisonTimer <= 0)
            {
                //deals 1 damage for every poison applyed to this enemy 
                TakeDamage(poisonCounter);
                
                UpdatedHealthBar();
                poisonTimer = poisonInterval;
            }
            else
            {
                poisonTimer -= Time.deltaTime;
            }
        }
        //prevents the enemy from exiting their room 
        StayInBounds();
    }

    

    //called when enemy takes damage
    protected abstract void SetState();

    public void TakeDamage(int damage)
    {
        //updates health
        health -= damage;
        //recalculates the state whenever the enemy takes damage
        SetState();

        //plays a hit sound if the sound is currenly playing 
        if (!hitSound.isPlaying)
        {
            hitSound.Play();
        }

        //updates the UI health bar
        UpdatedHealthBar();
    }

    //move toward the player
    protected void Approach()
    {
        direction = player.Position - position;
        direction = direction.normalized;


    }

    //move away from the player
    protected void Retreat()
    {
        direction = player.Position - position;
        direction = direction.normalized * -1;
    }

    //implimented in all child classes
    protected abstract void RangedAttack();

    //prevents the enemy from exceeding the bounds of its room
    protected void StayInBounds()
    {
        if(position.x + enemyBounds.extents.x > boundaries.max.x)
        {
            position.x = boundaries.max.x - enemyBounds.extents.x;
        }
        else if(position.x - enemyBounds.extents.x < boundaries.min.x)
        {
            position.x = boundaries.min.x + enemyBounds.extents.x;

        }

        if(position.y + enemyBounds.extents.y > boundaries.max.y)
        {
            position.y = boundaries.extents.y - enemyBounds.extents.y;
        }
        else if(position.y - enemyBounds.extents.y < boundaries.min.y)
        {
            position.y = boundaries.min.y + enemyBounds.extents.y;
        }
    }

    private void UpdatedHealthBar()
    {
        //sets the health bar based on the percentage of maxHealth the enemy has remaining
        healthBar.value = (health / maxHealth);
    }

    //implimented by all child classes
    public abstract void PublicStart();
    
}
