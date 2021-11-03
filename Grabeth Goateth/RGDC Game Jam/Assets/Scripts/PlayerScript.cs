using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerScript : MonoBehaviour
{
    public Light flashLight;
    private float lightTimer = 1;
    public int charges = 3;
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        flashLight.enabled = false;
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(flashLight.enabled == true)
        {
            lightTimer -= Time.deltaTime;

            if(lightTimer <= 0)
            {
                flashLight.enabled = false;
                lightTimer = 1;
            }
        }

        transform.position = position;
    }

    private void OnFlashLight(InputValue value)
    {
        if (charges > 0 && flashLight.enabled == false)
        {
            flashLight.enabled = true;
            charges -= 1;
        }
    }

    private void OnForward(InputValue value)
    {
        position.x += 0.1f;
    }

    private void OnBack(InputValue value)
    {
        position.x -= 0.1f;
    }

    private void OnLeft(InputValue value)
    {
        
        position.z += 0.1f;
    }

    private void OnRight(InputValue value)
    {
        position.z -= 0.1f;
    }
}
