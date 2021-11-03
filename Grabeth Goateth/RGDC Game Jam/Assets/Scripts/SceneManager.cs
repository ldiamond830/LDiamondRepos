using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] guardList;
    void Start()
    {
        //stores every guard object
        guardList = GameObject.FindGameObjectsWithTag("Guard");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
