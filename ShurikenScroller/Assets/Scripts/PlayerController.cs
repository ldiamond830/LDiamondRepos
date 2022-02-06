using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.01f;
    private Vector3 velocity;
    private Vector3 position;
    public float hp = 10;
    public float fireRate = 1.0f;
    public Shuriken shuriken;
    private float fireTimer = 0;
    
    public Vector3 Position
    {
        get { return position; }
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(speed, 0, 0);
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position += velocity * Time.deltaTime;

        gameObject.transform.position = position;

        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    void OnShoot(InputValue value)
    {
        if(fireTimer <= 0)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.transform.position.y - 0.5f;
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePos);
            //sets mouse.y to 0 to avoid the seek point being positioned above the ground
            clickPos.z = 0;



            Shuriken newShuriken = Instantiate(shuriken);
            newShuriken.direction = clickPos - this.position;
            newShuriken.transform.position = position;

            fireTimer = fireRate;
            
        }
        
    }
}
