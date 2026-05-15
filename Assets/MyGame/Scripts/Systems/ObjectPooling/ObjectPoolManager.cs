using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> poolDict = new();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!poolDict.ContainsKey(prefab))
        {
            poolDict[prefab] = new Queue<GameObject>();
        }

        var pool = poolDict[prefab];

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, this.transform);
            obj.AddComponent<PooledObject>().Init(prefab, this);
        }

        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);

        return obj;
    }

    public void Despawn(GameObject obj, GameObject prefab)
    {
        obj.SetActive(false);
        poolDict[prefab].Enqueue(obj);
    }
}