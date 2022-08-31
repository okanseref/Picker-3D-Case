using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -2)
        {
            MainService.instance.poolService.PushObject(PoolService.PoolType.ball, gameObject);
        }
    }
}
