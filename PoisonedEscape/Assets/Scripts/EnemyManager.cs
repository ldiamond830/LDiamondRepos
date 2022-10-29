using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public List<Enemy> enemies = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Enemy enemy in enemies)
        {
            if(enemy.health<= 0)
            {
                Debug.Log("enemy dead");
            }
        }
    }
}
