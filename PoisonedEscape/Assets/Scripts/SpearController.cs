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
        float angleToRotate = Mathf.Atan2(player.Position.y, player.Position.x) * Mathf.Rad2Deg;
        //angleToRotate *= -1;
        transform.rotation = Quaternion.Euler(0, 0, angleToRotate);
        Vector4 temp = Quaternion.Euler(0, 0, angleToRotate) * new Vector4(forward.x, forward.y, forward.z, 0.0f);
        forward = temp.normalized;
        bounds.center = transform.position;


        if (CollisionCheck())
        {
            if(player.ImmunityTimer <= 0)
            {
                player.TakeDamage(damage);
                player.ImmunityTimer = 0.5f;
            }
            
        }
       
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
