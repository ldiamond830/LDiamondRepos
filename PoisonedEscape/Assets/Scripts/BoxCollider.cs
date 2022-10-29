using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider : MonoBehaviour
{
    private Bounds bounds;

    public BoxCollider other;
    private Bounds otherBounds;
    // Start is called before the first frame update
    void Start()
    {
        bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
