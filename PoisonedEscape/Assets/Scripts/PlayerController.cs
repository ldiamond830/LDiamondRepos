using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private EnemyManager currentRoom;
    [SerializeField]
    private EnemyManager startRoom;


    //movement controls
    public InputAction playerControls;
    private Vector2 direction = Vector2.zero;
    private Vector3 velocity = Vector2.zero;
    private Vector3 position;

    [SerializeField]
    private Slider healthSlider;
    //private Punch fist;

    

    //stats
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float fireRate;
    private float fireTimer;
    [SerializeField]
    private float health;
    private float maxHealth;


    //sounds
    [SerializeField]
    private AudioSource acidHiss;
    [SerializeField]
    private AudioSource attackSound;
    [SerializeField]
    private AudioSource hitSound;



    public Spit spitBase;

    private float immunityTimer;

    private Bounds bounds;

    public float ImmunityTimer
    {
        set { immunityTimer = value; }
        get { return immunityTimer; }
    }

    public Vector3 Position
    {
        get { return position; }
    }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public Bounds PlayerBounds
    {
        get { return bounds; }
    }

    public EnemyManager Room
    {
        get { return currentRoom; }
        set { currentRoom = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //intializing values
        maxHealth = health;
        position = transform.position;
        currentRoom = startRoom;
        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;

        
    }

    // Update is called once per frame
    void Update()
    {
       
        //updates player position
        Movement();

        if (fireTimer >= 0)
        {
            fireTimer -= Time.deltaTime;
        }
        
        //updates the player's invicibility frames
        if(immunityTimer >= 0)
        {
            immunityTimer -= Time.deltaTime;
        }
    }

    private void Movement()
    {
        //reads in the direction from the controlsas
        direction = playerControls.ReadValue<Vector2>();

        //moves the player based on speed value, read in direction and scales by delta time
        velocity = new Vector3(direction.x * moveSpeed, direction.y * moveSpeed, 0);
        position += velocity * Time.deltaTime;
        //check to avoid the player moving out of bounds
        StayInBounds();
        //updates the object in game
        transform.position = position;
        bounds.center = position;
    }
   
    
    

    private void StayInBounds()
    {
        //since the gates are always positioned on the right of the room they function as the border while active
        if (Room.exit.IsActive)
        {
            if (position.x + bounds.extents.x > Room.exit.GateBounds.min.x )
            {
                position.x = Room.exit.GateBounds.min.x - bounds.extents.x;
            }
        }
        
        else
        {
            //prevents the player from walking out of bounds if they aren't going through the area where the gate was before being destoryed
            if (position.x + bounds.extents.x > Room.RoomBounds.max.x && 
                (position.y + bounds.extents.y > Room.exit.GateBounds.max.y || position.y - bounds.extents.y < Room.exit.GateBounds.min.y))
            {
                position.x = Room.RoomBounds.max.x - bounds.extents.x;
            }
        }

        //prevents the player from walking out of bounds if they aren't going through the area where the gate was before being destoryed
        if (position.x - bounds.extents.x < Room.RoomBounds.min.x 
            && (position.y + bounds.extents.y > Room.exit.GateBounds.max.y || position.y - bounds.extents.y < Room.exit.GateBounds.min.y))
       {
            position.x = Room.RoomBounds.min.x + bounds.extents.x;
       }

        //player can backtract as far as the starter room
        if (position.x - bounds.extents.x < startRoom.RoomBounds.center.x - startRoom.RoomBounds.extents.x )
        {
          position.x = Room.RoomBounds.min.x + bounds.extents.x;

        }
        //border checks for top and bottom
        if (position.y + bounds.extents.y > Room.RoomBounds.max.y)
        {
            position.y = Room.RoomBounds.extents.y - bounds.extents.y;
        }
        else if (position.y - bounds.extents.y < Room.RoomBounds.min.y)
        {
            position.y = Room.RoomBounds.min.y + bounds.extents.y;
        }
    }

    //behavior for firing a new projectile
    private void OnProjectile(InputValue value)
    {
        if(fireTimer<= 0)
        {
            //resets the interval between shots
            fireTimer = fireRate;

            //plays the firing sound
            if (!attackSound.isPlaying)
            {
                attackSound.Play();
            }

            //creates a new spit projectile and intializes it's values
            Spit SpitToInstantiate = Instantiate(spitBase);
            SpitToInstantiate.transform.position = transform.position;
            SpitToInstantiate.currentRoom = currentRoom;

            SpitToInstantiate.acidHiss = acidHiss;

            //gets mouse location
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.transform.position.y - 0.5f;
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePos);

            //fires the projectile to either the right or left depending on the mouse position
            if (clickPos.x < position.x)
            {
                SpitToInstantiate.Direction = new Vector3(-1.0f, 0.0f, 0.0f);
            }
            else
            {
                SpitToInstantiate.Direction = new Vector3(1.0f, 0.0f, 0.0f);
            }
        }
      

    }

    //behavior that happens every time the player takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        //updates the health 
        healthSlider.value = health/maxHealth;

        if (!hitSound.isPlaying)
        {
            hitSound.Play();
        }

        //loss condition
        if (health <= 0)
        {
            SceneManager.LoadScene("LossScene");
        }
    }

    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
