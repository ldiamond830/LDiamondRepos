using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //fields
    public Light flashLight;
    private float lightTimer = 2;
    public int charges = 3;
    private Vector3 position;
    private Vector2 playerInput;
    public float speed = 2;
    public Animator animator;
    public GameObject goat;
    public bool hasGoat = false;
    private Bounds goatBounds;

    public Text flashLightText;
    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
        flashLight.enabled = false;
        goatBounds = goat.GetComponent<SpriteRenderer>().bounds;

        flashLightText.text = string.Format("Flashlight Charges: {0}", charges);
    }

    // Update is called once per frame
    void Update()
    {
        //WASD movement controls
        if(playerInput.y > 0)
        {
            position.y += speed * Time.deltaTime;
            //controls the orientation of the player sprite
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
        }
        else if(playerInput.y< 0)
        {
            position.y -= speed * Time.deltaTime;
            animator.SetBool("Up", false);
            animator.SetBool("Down", true);
        }
        else
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
        }

        if(playerInput.x > 0)
        {
            position.x += speed * Time.deltaTime;
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
        }
        else if(playerInput.x < 0)
        {
            position.x -= speed * Time.deltaTime;
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
        }
        else
        {
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
        }

        //flashlight behavior
        if (flashLight.enabled == true)
        {
            lightTimer -= Time.deltaTime;

            if (lightTimer <= 0)
            {
                flashLight.enabled = false;
                lightTimer = 2;
                flashLightText.text = "Flashlight Charges: " + charges;
            }

        }

        //when the player colides with the goat
        if (goatBounds.Contains(transform.position))
        {
            hasGoat = true;
            goat.SetActive(false);


        }

        if (hasGoat)
        {
            //win condition is when the player reaches nearby the fire with the goat
            if(position.y > 11 && position.y < 35 && position.x > 7 && position.x < 30)
            {
                SceneManager.LoadScene("WinScene");
            }
            
        }

        gameObject.transform.position = position;
    }

    //input method for flash light
    private void OnFlashLight(InputValue value)
    {
        //stops the player from using their stun more than 3 times or spamming it over and over
        if (charges > 0 && flashLight.enabled == false)
        {
            flashLight.enabled = true;
            charges -= 1;
        }
    }

    //gets WASD movement input vector
    public void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }
}
