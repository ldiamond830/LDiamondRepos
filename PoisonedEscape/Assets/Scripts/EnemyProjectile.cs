using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 position;
    public float moveSpeed;

    private PlayerController player;
    
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * moveSpeed;
        position = velocity * Time.deltaTime;
        transform.position = position;
    }
}
