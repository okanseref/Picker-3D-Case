using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    public GameObject cursor;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator=cursor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        cursor.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("click");
        }
    }
}
