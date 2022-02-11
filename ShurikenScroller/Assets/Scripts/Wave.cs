using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
   public Vector3 direction;
    public Vector3 velocity;
    public Vector3 position;
    public float speed = 10;
    public int damage;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //controls movement
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        //sets position
        gameObject.transform.position = position;
    }
}
