using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (transform.position.y < -2)
        {
            MainService.instance.poolService.PushObject(PoolService.PoolType.ball, gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (rb.velocity.magnitude > 1f)
            {
                MainService.instance.soundService.PlaySound(0, Random.Range(0.3f, 0.7f));
            }
        }
        else
        {
            if (rb.velocity.magnitude > 3f)
            {
                MainService.instance.soundService.PlaySound(0, Random.Range(0.5f, 0.8f));
            }
        }

    }
}
