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

    /* currently unused
    //camera bounds
    private float cameraHeight;
    private float cameraWidth;
    public Camera cameraObject;
    */

    public Slider healthBar;

    public GameObject room;
    private Bounds boundaries;

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

    // Start is called before the first frame update
    protected void Start()
    {
        position = transform.position;
        health = maxHealth;
        UpdatedHealthBar();
        poisonInterval = 1.0f;
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
        enemyBounds.center = position;

        

        

        if (poisonCounter > 0)
        {
            if (!poisonIndicator.enabled)
            {
                poisonIndicator.enabled = true;
            }

            if(poisonTimer <= 0)
            {
                TakeDamage(poisonCounter);
                SetState();
                UpdatedHealthBar();
                poisonTimer = poisonInterval;
            }
            else
            {
                poisonTimer -= Time.deltaTime;
            }
        }

        StayInBounds();
    }

    protected abstract void SetBehavior();

    //called when enemy takes damage
    protected abstract void SetState();

    public void TakeDamage(int damage)
    {
      
        health -= damage;
        //recalculates the state whenever the enemy takes damage
        SetState();

        if (!hitSound.isPlaying)
        {
            hitSound.Play();
        }

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

    protected abstract void RangedAttack();

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
    
}
