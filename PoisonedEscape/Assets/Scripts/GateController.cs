using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    private bool destructable;
    private bool isActive;

    private Bounds gateBounds;

    public bool Destructable
    {
        set { destructable = value; }
        get { return destructable; }
    }
    public Bounds GateBounds
    {
        get { return gateBounds; }
    }

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        gateBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        gateBounds.center = new Vector3(transform.position.x, transform.position.y, 0.0f);

        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            Debug.Log("Gate inactive");
            gameObject.SetActive(false);
        }
    }

    public void Deactivate()
    {
        


    }
}
