using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;
    public float moveSpeed;
    private Bounds bounds;

    private PlayerController player;
    
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }
    public PlayerController Player
    {
        set { player = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * moveSpeed;
        position += velocity * Time.deltaTime;
        transform.position = position;
        bounds.center = position;


        if (CollisionCheck())
        {
            player.Health--;
            gameObject.SetActive(false);
            this.enabled = false;
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
