using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public PlayerController player;
    public Vector3 direction;
    public float speed;
    public Vector3 velocity;
    public Vector3 position;
    public SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        gameObject.transform.position = position;
        transform.rotation = Quaternion.LookRotation(Vector3.right, direction);
    }

    
}
