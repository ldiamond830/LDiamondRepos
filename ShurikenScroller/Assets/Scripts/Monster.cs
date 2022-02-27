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
    
    public int PointValue
    {
        get { return pointValue; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
       
    }

    protected bool agroCheck()
    {
        //checks if the player is close enough to start attacking
        if (Vector3.Magnitude(player.transform.position - this.transform.position) < agroRange)
        {
            return true;
        }
        else
        {
            //stops the enemy from attacking if the player is far enough away, avoids a situation where an enemy the player passed by is still firing projectiles 
            return false;
        }
    }

    //requires each child class to have it's own update method to be called by the scene manager
    public abstract void UpdateHolder();

    
}
