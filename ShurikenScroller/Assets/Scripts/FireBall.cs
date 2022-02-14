using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    
    public Vector3 direction;
    public float speed;
    public Vector3 velocity;
    public Vector3 position;
    public SpriteRenderer spriteRenderer;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //controls movement
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        //sets position
        gameObject.transform.position = position;
        //rotates the fireball to face towards the player
        transform.rotation = Quaternion.LookRotation(Vector3.right, direction); //WIP
    }

    
}
