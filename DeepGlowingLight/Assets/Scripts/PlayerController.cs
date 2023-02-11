using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //fields
    public InputAction playerControls;
    private Vector2 direction = Vector2.zero;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashLength;
    private bool isDashing;
    private bool canDash;
   
    [SerializeField]
    private float dashCooldown;
    public new Rigidbody2D rigidbody;
    private bool canJump = true;

    private Vector2 respawnPoint;

    //Animation move
    private bool isRunning = false;
    [SerializeField]
    private Animator animator;
    private float playerPosPre, horizontal;
    public GameObject obj;

    public bool CanJump
    {
        get { return canJump; }
        set { canJump = value; }
    }

    public Vector2 RespawnPoint
    {
        get { return respawnPoint; }
        set { respawnPoint = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Animation move
        playerPosPre = 0f;

        isDashing = false;
        canDash = true;

        respawnPoint = new Vector2(rigidbody.position.x, rigidbody.position.y);

    }

    private void FixedUpdate()
    {
        //updates player's horizontal velocity
        rigidbody.velocity = new Vector2((direction.x * speed * Time.deltaTime), rigidbody.velocity.y);
       

        //Debug.Log(rigidbody.velocity.x);
    }
    // Update is called once per frame
    void Update()
    {
        direction = playerControls.ReadValue<Vector2>();


        //Animation move
        horizontal = direction.x;
        isRunning = horizontal != 0f;
        if (isRunning)
        {
            Vector3 scale = gameObject.transform.localScale;
            if (horizontal < 0 && scale.x > 0 || horizontal > 0 && scale.x < 0)
            {
                scale.x *= -1;
            }
            gameObject.transform.localScale = scale;
        }
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsJumpping", !canJump);
        playerPosPre = transform.position.x;
    }

    private void OnExitGame(InputValue value)
    {
        if (obj.activeSelf) { obj.SetActive(false); }
        if (!obj.activeSelf) { obj.SetActive(true); }
    }

    private void OnJump(InputValue value)
    {

        if (canJump)
        {
            
            rigidbody.AddForce(new Vector2(0, jumpHeight));
            //canJump = false;
        }

    }
    private void OnDash(InputValue value)
    {
        direction = playerControls.ReadValue<Vector2>();
       
        /*
        if(direction.x == 0)
        {
            rigidbody.AddForce(Vector2.right * dashSpeed);
        }
        else
        {
            rigidbody.AddForce(direction * dashSpeed);
        }
        */

        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        
            isDashing = true;
            canDash = false;

            float originalGravity = rigidbody.gravityScale;
            //keeps the player's vertical height constant while dashing
            rigidbody.gravityScale = 0.0f;

        //if the player has no horizontal direction dashes them to the right
            if (direction.x == 0)
            {
                rigidbody.AddForce(new Vector2(dashSpeed, 0));
               
            }
            else
            { 
               rigidbody.AddForce(new Vector2((direction.x * dashSpeed), 0.0f));
            }
            

            //waits until the end of the player's dash
            yield return new WaitForSeconds(dashLength);
            //resets gravity so player can fall after dashing
            rigidbody.gravityScale = originalGravity;
            isDashing = false;

            //cooldown
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
    }

    

    //Jump adjustment
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
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
