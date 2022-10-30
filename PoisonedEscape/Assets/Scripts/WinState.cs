using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : MonoBehaviour
{
    [SerializeField]
    private EnemyManager finalRoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(finalRoom.enemies.Count <= 0)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
