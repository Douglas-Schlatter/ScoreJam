using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class gfxControl : MonoBehaviour
{
    public AIPath pf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pf.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            //transform.localRotation = new Quaternion(0, 0, -90, 0);
        }
        else if (pf.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            //transform.localRotation = new Quaternion(0, 0, 90, 0);
        }
    }
}
