using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //movement controls
    public InputAction playerControls;
    private Vector2 direction = Vector2.zero;
    private Vector3 velocity = Vector2.zero;
    private Vector3 position;

    [SerializeField]
    private Text healthText;
    //private Punch fist;

    [SerializeField]
    private EnemyManager enemyManager;

    //stats
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float fireRate;
    private float fireTimer;
    [SerializeField]
    private int health;


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

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;

        Movement();
        bounds.center = position;
        fireTimer -= Time.deltaTime;

        if(immunityTimer >= 0)
        {
            immunityTimer -= Time.deltaTime;
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

    private void OnProjectile(InputValue value)
    {
        if(fireTimer<= 0)
        {
            fireTimer = fireRate;

            

            Spit SpitToInstantiate = Instantiate(spitBase);
            SpitToInstantiate.transform.position = transform.position;
            SpitToInstantiate.enemyManager = enemyManager;

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
