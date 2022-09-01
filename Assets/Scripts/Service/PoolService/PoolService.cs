using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolService : MonoBehaviour
{
    public enum PoolType
    {
        ball,
        breakFX
    }
    public int maxLimit = 20;
    public GameObject ball_prefab;
    public GameObject breakFX_prefab;
    Queue<GameObject> ball_queue = new Queue<GameObject>();
    Queue<GameObject> breakFX_queue = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            CreateNewStock(PoolType.ball);
        }
        for (int i = 0; i < 10; i++)
        {
            CreateNewStock(PoolType.breakFX);
        }
    }

    public void PushObject(PoolType type,GameObject poolObject)
    {
        poolObject.transform.SetParent(null);
        poolObject.SetActive(false);
        if(GetQueue(type).Count> maxLimit)
        {
            Destroy(poolObject);
        }
        else
        {
            GetQueue(type).Enqueue(poolObject);
        }
    }
    private void CreateNewStock(PoolType type)
    {
        GameObject newObject = Instantiate(GetPrefab(type), null);
        newObject.SetActive(false);
        GetQueue(type).Enqueue(newObject);
    }
    public GameObject PopObject(PoolType type)
    {
        if (GetQueue(type).Count < 3)
        {
            CreateNewStock(type);
        }
        return GetQueue(type).Dequeue();
    }
    private GameObject GetPrefab(PoolType type)
    {
        switch (type)
        {
            case PoolType.ball:
                return ball_prefab;
            case PoolType.breakFX:
                return breakFX_prefab;
        }
        return null;
    }
    private Queue<GameObject> GetQueue(PoolType type)
    {
        switch (type)
        {
            case PoolType.ball:
                return ball_queue;
            case PoolType.breakFX:
                return breakFX_queue;
        }
        return null;
    }
}
