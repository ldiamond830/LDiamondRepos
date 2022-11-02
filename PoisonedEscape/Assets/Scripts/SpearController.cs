using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : MonoBehaviour
{
    private PlayerController player;

    
    private Vector3 velocity = Vector2.zero;
    private Vector3 position;
    private Vector3 forward = new Vector3(0.0f, 0.0f, 1.0f);

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;

    private Bounds bounds;
    private Bounds playerBounds;

    public Vector3 Forward
    {
        get { return forward; }
    }
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    public float Speed
    {
        get { return speed; }
    }

    public PlayerController Player
    {
        set { player = value; }
    }

    public Bounds PlayerBounds
    {
        set { playerBounds = value; }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
       


        if (CollisionCheck())
        {
            
            if(player.ImmunityTimer <= 0)
            {
                player.TakeDamage(damage);
                //gives the player 0.5 seconds of immunity to avoid dealing damage every frame
                player.ImmunityTimer = 0.5f;
            }
            
        }
       
    }
     
    public void AdjustRotation()
    {
        //draws a vector between the spear and player
        Vector3 vecToPlayer = player.Position - transform.position;

        //gets the angle to rotate based on the vector between the player and current position
        float angleToRotate = Mathf.Atan2(vecToPlayer.y, vecToPlayer.x) * Mathf.Rad2Deg;
       
        //updates the rotation using a quaternion
        transform.rotation = Quaternion.Euler(0, 0, angleToRotate);

       

        Vector4 temp = Quaternion.Euler(0, 0, angleToRotate) * new Vector4(forward.x, forward.y, forward.z, 0.0f);
        forward = temp.normalized;
        bounds.center = transform.position;
    }

    private bool CollisionCheck()
    {
        if (bounds.Intersects(player.PlayerBounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
