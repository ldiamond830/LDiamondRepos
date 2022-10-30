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
    private Text healthText;
    //private Punch fist;

    

    //stats
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float fireRate;
    private float fireTimer;
    [SerializeField]
    private int health;


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

    public int Health
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
        position = transform.position;
        currentRoom = startRoom;
        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;

        healthText.text = "Health: " + health;
    }

    // Update is called once per frame
    void Update()
    {
       

        Movement();
        bounds.center = position;
        fireTimer -= Time.deltaTime;

        if(immunityTimer >= 0)
        {
            immunityTimer -= Time.deltaTime;
        }
       
        if(health <= 0)
        {
            SceneManager.LoadScene("LossScene");
        }

        //busted

        //RotateHand();
    }

    private void Movement()
    {
        //reads in the direction from the controlsas
        direction = playerControls.ReadValue<Vector2>();

        //moves the player based on speed value, read in direction and scales by delta time
        velocity = new Vector3(direction.x * moveSpeed, direction.y * moveSpeed, 0);
        position += velocity * Time.deltaTime;

        StayInBounds();

        transform.position = position;
        
    }
   
    
    private void RotateHand()
    {
        /*
       //gets the position of the mouse on the screen
       Vector3 mousePosition = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());


       if(mousePosition != prevMousePos)
       {
           Vector3 temp = transform.position - mousePosition;

           float angleToRotate = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;


           //adjusts the angle of the hand

           // fist.transform.RotateAround(this.transform.position, Vector3.forward, angleToRotate);
               //uaternion.Euler(0, 0, angleToRotate);
       }


       prevMousePos = mousePosition;


       Vector3 mousePosition = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());
       //I'm not sure if castPoint has a position in it or not, but if when you type castPoint.transform.position
       direction = (mousePosition - transform.position).normalized;

       //create the rotation we need to be in to look at the target
       Quaternion lookRotation = Quaternion.LookRotation(direction);

       //rotate us over time according to speed until we are in the required rotation
       fist.transform.RotateAround(this.transform.position, Vector3.forward, lookRotation.eulerAngles)
       */
    }
    /*
    private void OnPunch(InputValue value)
    {
        if(fist.CurrentState != State.isReturning)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());
            fist.Direction = mousePosition - transform.position;
            fist.Direction.Normalize();
            fist.CurrentState = State.isPunching;

        }
       
    }
    */

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
        
        //player can backtract as far as the starter room
        if (position.x - bounds.extents.x < startRoom.RoomBounds.center.x - startRoom.RoomBounds.extents.x)
        {
          position.x = Room.RoomBounds.min.x + bounds.extents.x;

        }

        if (position.y + bounds.extents.y > Room.RoomBounds.max.y)
        {
            position.y = Room.RoomBounds.extents.y - bounds.extents.y;
        }
        else if (position.y - bounds.extents.y < Room.RoomBounds.min.y)
        {
            position.y = Room.RoomBounds.min.y + bounds.extents.y;
        }
    }

    private void OnProjectile(InputValue value)
    {
        if(fireTimer<= 0)
        {
            fireTimer = fireRate;

            if (!attackSound.isPlaying)
            {
                attackSound.Play();
            }

            Spit SpitToInstantiate = Instantiate(spitBase);
            SpitToInstantiate.transform.position = transform.position;
            SpitToInstantiate.currentRoom = currentRoom;

            SpitToInstantiate.acidHiss = acidHiss;

            //gets mouse location
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.transform.position.y - 0.5f;
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePos);

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

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = "Health: " + health;

        if (!hitSound.isPlaying)
        {
            hitSound.Play();
        }
    }

    //needed to for controls to work 
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
