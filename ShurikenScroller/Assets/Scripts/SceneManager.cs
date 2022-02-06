using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public List<Monster> enemyList = new List<Monster>();
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Monster enemy in enemyList)
        {
            enemy.UpdateHolder();
        }
    }


    void CollisionDetector(SpriteRenderer sprite1, SpriteRenderer sprite2)
    {

    }
}
