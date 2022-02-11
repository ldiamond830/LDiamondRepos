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

        gameObject.transform.position = position;

        //reduces the time till the player can next throw a shuriken
        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
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
            newShuriken.transform.position = position;

            //adds the new shuriken to the list so collision tests can be run
            sceneManager.shurikenList.Add(newShuriken);

            //sets the cooldown before the next shot
            fireTimer = fireRate;
            
        }
        
    }
}
