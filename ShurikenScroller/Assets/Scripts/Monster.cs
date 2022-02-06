using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// parent class for all enemies
/// </summary>
public abstract class Monster : MonoBehaviour
{
    //variables
    public bool isAlive;
    public float agroRange;
    protected bool isAgro;
    public int damage;
    public PlayerController player;
    public SpriteRenderer spriteRenderer;
    protected int pointValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        //checks if the player is close enough to start attacking
        if(Vector3.Magnitude(player.transform.position - this.transform.position) < agroRange)
        {
            isAgro = true;
        }
    }

    public abstract void UpdateHolder();
}
