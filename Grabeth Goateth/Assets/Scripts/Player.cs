using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
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
        if(playerInput.y > 0)
        {
            position.y += speed * Time.deltaTime;
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


        if (goatBounds.Contains(transform.position))
        {
            hasGoat = true;
            goat.SetActive(false);


        }

        if (hasGoat)
        {
            if(position.y > 11 && position.y < 35 && position.x > 7 && position.x < 30)
            {
                SceneManager.LoadScene("WinScene");
            }
            
        }

        gameObject.transform.position = position;
    }

    private void OnFlashLight(InputValue value)
    {
        if (charges > 0 && flashLight.enabled == false)
        {
            flashLight.enabled = true;
            charges -= 1;
        }
    }

    public void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }
}
