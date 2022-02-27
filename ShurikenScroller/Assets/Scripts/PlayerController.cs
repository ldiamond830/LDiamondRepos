using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //variables
    public float speed = 0.01f;
    private Vector3 velocity;
    private Vector3 position;
    public float hp = 10;
    public float fireRate = 1.0f;
    public Shuriken shuriken;
    private float fireTimer = 0;
    public SceneController sceneManager;
    public SpriteRenderer spriteRenderer;
    private bool isJumping = false;
    private float jumpConstant;
    private float jumpStart;
    private float speedTimer;
    private bool firstSpeedUp = false;
    private bool secondSpeedUp = false;
    public int score;
    //properties
    public Vector3 Position
    {
        get { return position; }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //starts the auto moving, subject to change
        velocity = new Vector3(speed, 0, 0);
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position += velocity * Time.deltaTime;

        

        //reduces the time till the player can next throw a shuriken
        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if (isJumping)
        {
            position.y += jumpConstant;

            if (position.y > 4)
            {
                jumpConstant = -3f * Time.deltaTime;
            }

            //when the player is just as low as the point they started at stops updating their height for this jump
            if (position.y <= jumpStart)
            {
                isJumping = false;
            }
        }


        speedTimer += Time.deltaTime;
        //increases the player's speed during the shift between the first and second part of the song
        if(speedTimer >= 18 && !firstSpeedUp)
        {
            firstSpeedUp = true;
            speed += 1;
            velocity = new Vector3(speed, 0, 0);
        }

        //increase's the players speed more but slower this time so that it creates the effect of their movement ramping up over time
        if(speedTimer > 47.5 && !secondSpeedUp)
        {
            speed += 0.01f;
            velocity = new Vector3(speed, 0, 0);

            //stops increasing the player's speed when the final second of the song begins
            if (speedTimer > 49.5)
            {
                secondSpeedUp = true;
            }
        }

        gameObject.transform.position = position;
    }

    void OnShoot(InputValue value)
    {
        if(fireTimer <= 0)
        {
            //gets mouse location
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.transform.position.y - 0.5f;
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePos);
            //sets mouse.z to 0 to avoid shuriken going behind the camera
            clickPos.z = 0;



            Shuriken newShuriken = Instantiate(shuriken);
            //new shuriken will move towards the position of the mouse at the time of firing
            newShuriken.direction = clickPos - this.position;
            newShuriken.direction.Normalize();
            newShuriken.transform.position = position;
            newShuriken.speed = 15f;
            //adds the new shuriken to the list so collision tests can be run
            sceneManager.shurikenList.Add(newShuriken);

            //sets the cooldown before the next shot
            fireTimer = fireRate;
            
        }

      
    }

    void OnJump(InputValue value)
    {
        if (!isJumping)
        {
            isJumping = true;
            jumpConstant = 3f * Time.deltaTime;
            jumpStart = position.y;
        }
        
    }
}
