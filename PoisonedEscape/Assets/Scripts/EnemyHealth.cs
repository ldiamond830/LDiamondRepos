using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject backBar;
    public GameObject healthBar;
    public Enemy parent;
    

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDamage(float damageTaken)
    {
        Vector3 temp = new Vector3(healthBar.transform.localScale.x, 1.0f, 1.0f);
        temp.x -= 1.5f / damageTaken;
        healthBar.transform.localScale = temp;
    }
}
