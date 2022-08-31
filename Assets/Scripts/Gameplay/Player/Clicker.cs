using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public bool clickDetection = false;
    public float forwardSpeed = 400;
    public float maxSpeed = 800;
    public float speedIncrease = 40f;
    float currentSpeed = 1f;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        currentSpeed = forwardSpeed;
        clickDetection = false;
    }

    // Update is called once per frame
    public void ResetClicker()
    {
        currentSpeed = forwardSpeed;
    }
    void Update()
    {
        if (clickDetection&&Input.GetMouseButtonDown(0))
        {
            currentSpeed += speedIncrease;
            currentSpeed = Mathf.Clamp(currentSpeed,forwardSpeed, maxSpeed);
            //print(currentSpeed - forwardSpeed / maxSpeed - forwardSpeed);
            MainService.instance.uiService.SetFillPanel((currentSpeed - forwardSpeed) / (maxSpeed - forwardSpeed));
        }
    }
    private void FixedUpdate()
    {
        if (clickDetection)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, currentSpeed) * Time.deltaTime;
        }
    }
}
