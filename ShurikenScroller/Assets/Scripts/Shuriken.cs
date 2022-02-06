using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 10f;
    private Vector3 velocity;
    private Vector3 position;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        velocity = direction * speed;
        position = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        position += velocity * Time.deltaTime;
        gameObject.transform.position = position;
    }
}
