using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// projectile fired by player
/// </summary>
public class Shuriken : MonoBehaviour
{
    //fields
    public Vector3 direction;
    public float speed = 50f;
    private Vector3 velocity;
    private Vector3 position;
    public SpriteRenderer spriteRenderer;
    private float timeInAir = 1.5f;
    public bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        //sets fields
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        velocity = direction * speed; //moves in a straight line with direction set when the player fires the shot
        position = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        timeInAir -= Time.deltaTime;

        //handles movement
        position += velocity * Time.deltaTime;
        gameObject.transform.position = position;

        if(timeInAir <= 0)
        {
            isActive = false;
        }
    }
}
