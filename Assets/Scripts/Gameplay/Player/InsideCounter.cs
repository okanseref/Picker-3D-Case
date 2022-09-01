using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideCounter : MonoBehaviour
{
    public List<GameObject> list = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!list.Contains(other.gameObject)&&other.tag.Equals("Ball"))
        {
            list.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other )
    {
        if (list.Contains(other.gameObject) && other.tag.Equals("Ball"))
        {
            list.Remove(other.gameObject);
        }
    }
    
}
