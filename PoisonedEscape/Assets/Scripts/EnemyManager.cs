using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// as I progressed it made more sense to use this as a full room manager but changing the name would cause issues
/// </summary>
public class EnemyManager : MonoBehaviour
{

    public AudioSource enemyHit;

    //public Camera cameraObject;
    public GateController exit;
    private Bounds roomBounds;

    public List<Enemy> enemies = new List<Enemy>();
    public PlayerController player;

    public Bounds RoomBounds
    {
        get { return roomBounds; }
    }

    private bool empty = false;

    // Start is called before the first frame update
    void Start()
    {
        /* currently unused
        foreach (Enemy enemy in enemies)
        {
            enemy.cameraObject = cameraObject;
        }
        */

        roomBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        roomBounds.center = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0.0f);
        

        if(enemies.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.HitSound = enemyHit;
                enemy.room = this.gameObject;
                enemy.player = player;

                enemy.PublicStart();
            }
            
            //indicates to player that the gate cannot be destroyed
            exit.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            exit.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            empty = true;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.Room != this)
        {
            
            if (roomBounds.Contains(player.Position))
            {
               player.Room = this;
                //Debug.Log("Player room: " + this);
            }
        }
        if(enemies.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.Health <= 0)
                {
                    enemies.Remove(enemy);
                    enemy.gameObject.SetActive(false);
                }
            }
        }
        
        if(enemies.Count <= 0 && exit != null)
        {
            exit.Destructable = true;
            exit.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, roomBounds.size);
        //Gizmos.DrawWireCube(exit.transform.position, exit.GateBounds.size);
    }
}
