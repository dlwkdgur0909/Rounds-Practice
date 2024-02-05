using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class Pool
{
    public Queue<GameObject> pool = new Queue<GameObject>(); //배열에 들어온 순서대로 나감 / 넣는 건 EnQueue /빼는 건 DeQueue
    public string poolName;
    public GameObject poolPrefab;
    public int objectAmount;
    [HideInInspector] public GameObject objectParent;

    public GameObject SpawnFromPool(Vector3 position) => _SpawnFromPool(position);
    public GameObject SpawnFromPool(Vector3 position, Transform parent) => _SpawnFromPool(position, parent);
    public GameObject SpawnFromPool(Vector3 position, Transform parent, Vector3 rotation) => _SpawnFromPool(position, parent, rotation);

    public void ReturnToPool(GameObject obj) => _ReturnToPool(obj);


    private GameObject _SpawnFromPool(Vector3 position)
    {
        GameObject obj = null;
        if (pool.Count == 0) ObjectPoolManager.NewObject(poolName);
        obj = pool.Dequeue();
        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    private GameObject _SpawnFromPool(Vector3 position, Transform parent)
    {
        GameObject obj = null;
        if (pool.Count == 0) ObjectPoolManager.NewObject(poolName);
        obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.parent = parent;
        obj.SetActive(true);
        return obj;
    }

    private GameObject _SpawnFromPool(Vector3 position, Transform parent, Vector3 rotation)
    {
        GameObject obj = null;
        if (pool.Count == 0) ObjectPoolManager.NewObject(poolName);
        obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.parent = parent;
        obj.transform.rotation = Quaternion.Euler(rotation);
        obj.SetActive(true);
        return obj;
    }

    private void _ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = objectParent.transform;
        pool.Enqueue(obj);
    }
}

public class ObjectPoolManager : MonoBehaviour
{

    public static ObjectPoolManager instance = null;

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Pool> poolDictionary = new Dictionary<string, Pool>();


    public void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void Start()
    {
        InitDictionary();
        InitObject();
    }

    private void InitObject()
    {
        foreach (var pools in pools)
        {
            pools.objectParent = new GameObject();
            pools.objectParent.name = pools.poolName;
            for (int i = 0; i < pools.objectAmount; i++)
            {
                var obj = Instantiate(pools.poolPrefab, pools.objectParent.transform);
                obj.SetActive(false);
                pools.pool.Enqueue(obj);
            }
        }
    }

    private void InitDictionary()
    {
        foreach (var pool in pools)
        {
            poolDictionary.Add(pool.poolName, pool);
        }
    }

    public static GameObject SpawnFromPool(string poolName, Vector3 position)
    {
        return instance.poolDictionary[poolName].SpawnFromPool(position);
    }
    public static GameObject SpawnFromPool(string poolName, Vector3 position, Transform parent)
    {
        return instance.poolDictionary[poolName].SpawnFromPool(position, parent);
    }
    public static GameObject SpawnFromPool(string poolName, Vector3 position, Transform parent, Vector3 rotation)
    {
        return instance.poolDictionary[poolName].SpawnFromPool(position, parent, rotation);
    }

    public static void ReturnToPool(string poolName, GameObject obj) => instance._ReturnToPool(poolName, obj);
    private void _ReturnToPool(string poolName, GameObject obj)
    {
        poolDictionary[poolName].ReturnToPool(obj);
    }

    public static void NewObject(string poolName) => instance._NewObject(poolName); //Monobehavior 없이 instance생성
    private void _NewObject(string poolName)
    {
        var obj = Instantiate(poolDictionary[poolName].poolPrefab, poolDictionary[poolName].objectParent.transform);
        poolDictionary[poolName].pool.Enqueue(obj);
    }
}
