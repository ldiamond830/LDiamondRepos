using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //movement controls
    public InputAction playerControls;
    private Vector2 direction = Vector2.zero;
    private Vector3 velocity = Vector2.zero;
    private Vector3 position;

    [SerializeField]
    private Punch fist;

    

    //stats
    [SerializeField]
    private float moveSpeed;

    public Spit spitBase;

    private Vector3 prevMousePos;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

       
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
        fist.PlayerPos = position;
    }


    private void OnProjectile(InputAction Action)
    {
        //gets mouse location
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.transform.position.y - 0.5f;
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePos);
        //sets mouse.z to 0 to avoid shuriken going behind the camera
        clickPos.z = 0;
    }
    
    private void RotateHand()
    {
        
        //gets the position of the mouse on the screen
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());
        

        if(mousePosition != prevMousePos)
        {
            Vector3 temp = transform.position - mousePosition;

            float angleToRotate = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
           
            
            //adjusts the angle of the hand

            fist.transform.RotateAround(this.transform.position, Vector3.forward, angleToRotate);
                //uaternion.Euler(0, 0, angleToRotate);
        }


        prevMousePos = mousePosition;
        /*

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());
        //I'm not sure if castPoint has a position in it or not, but if when you type castPoint.transform.position
        direction = (mousePosition - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        //rotate us over time according to speed until we are in the required rotation
        fist.transform.RotateAround(this.transform.position, Vector3.forward, lookRotation.eulerAngles)
        */
    }

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

    private void OnProjectile(InputValue value)
    {
        Spit SpitToInstantiate = Instantiate(spitBase);
        SpitToInstantiate.transform.position = transform.position;


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
