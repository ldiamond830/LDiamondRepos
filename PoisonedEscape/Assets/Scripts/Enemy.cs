using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds behaviors and data needed by all enemies
public abstract class Enemy : MonoBehaviour
{
    //stats
    public int health;
    public int speed;
    public PlayerController player;


    private BoxCollider2D collider;


    private Vector2 direction = Vector2.zero;
    private Vector3 velocity = Vector2.zero;
    private Vector3 position;

    public BoxCollider2D Collider
    {
        get { return collider; }
    }
    public Vector3 Position
    {
        get { return position; }
    }
    // Start is called before the first frame update
    protected void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
}
