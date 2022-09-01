using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Big : MonoBehaviour
{
    // Start is called before the first frame update
    bool broken = false;
    List<Vector3> list;
    void Start()
    {
        list = new List<Vector3>();
        list.Add(new Vector3(0.2f, 0f, -0.4f));
        list.Add(new Vector3(0.2f, -0.3f, -0.2f));
        list.Add(new Vector3(-0.2f, 0f, -0.4f));
        list.Add(new Vector3(-0.2f, -0.3f, -0.2f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player")|| collision.gameObject.tag.Equals("Arm"))
        {
            if (broken)
            {
                return;
            }
            gameObject.SetActive(false);
            broken = true;
            for (int i = 0; i < 4; i++)
            {
                GameObject ball= MainService.instance.poolService.PopObject(PoolService.PoolType.ball);
                ball.transform.position = transform.position+list[i];
                ball.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
