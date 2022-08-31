using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool moveEnabled, moveActive;
    float ScreenWidth;
    float xStartPoint = 0;
    public float playerXSpeed, playerZSpeed, currentZspeed = 5f;
    public float xMaxLeft, xMaxRight;
    Rigidbody rb;
    float xPositionChange;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        //moveEnabled = true;
        ScreenWidth = Screen.width;
        currentZspeed = playerZSpeed;
        moveActive = false;
    }
    public void ResetSpeed()
    {
        currentZspeed = playerZSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (moveEnabled == false)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && moveEnabled)
        {
            moveActive = true;
            xStartPoint = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0) && moveEnabled)
        {
            moveActive = false;
            rb.velocity = new Vector3(0, rb.velocity.y, currentZspeed);
            xPositionChange = 0;
        }
        if (Input.GetMouseButton(0) && moveEnabled)
        {
            float difference = Input.mousePosition.x - xStartPoint;
            xPositionChange = ((difference) / ScreenWidth) * playerXSpeed;
            xStartPoint = Input.mousePosition.x;
        }
    }
    private void FixedUpdate()
    {
        if (moveEnabled)
        {
            rb.velocity = new Vector3(xPositionChange, rb.velocity.y, currentZspeed) * Time.deltaTime;
        }
    }
}
