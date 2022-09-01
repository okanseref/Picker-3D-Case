using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //enum 
    public int levelLength = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReleaseRigidbodies()
    {
        Rigidbody[] Rigidbodies;
        Rigidbodies = transform.GetChild(1).GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody item in Rigidbodies)
        {
            item.constraints = RigidbodyConstraints.None;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
