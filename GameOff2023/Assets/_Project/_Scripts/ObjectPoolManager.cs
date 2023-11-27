using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // public static ObjectPoolManager instance;

    // [SerializeField] private GameObject pooledObjectPrefab;
    // [SerializeField] private int pooledAmount = 20;
    // [SerializeField] private bool willGrow;

    // private List<GameObject> pooledObjects = new List<GameObject>();

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    // }

    // private void Start()
    // {

    //     for (int i = 0; i < pooledAmount; i++)
    //     {
    //         GameObject obj = Instantiate(pooledObjectPrefab);
    //         obj.SetActive(false);
    //         pooledObjects.Add(obj);
    //     }
    // }
    // public GameObject GetPooledObject()
    // {
    //     for (int i = 0; i < pooledObjects.Count; i++)
    //     {
    //         if (!pooledObjects[i].activeInHierarchy)
    //         {
    //             return pooledObjects[i];
    //         }
    //     }

    //     if (willGrow)
    //     {
    //         GameObject obj = Instantiate(pooledObjectPrefab);
    //         pooledObjects.Add(obj);
    //         return obj;
    //     }

    //     return null;
    // }


    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    private GameObject objPolEmptyHolder;
    private static GameObject particleSystemEmty;
    private static GameObject gameObjEmpty;
    private static GameObject entityEmpty;

    public enum PoolType
    {
        ParticleSystem,
        GameObject,
        Entity,
        None
    }
    public static PoolType poolingType;

    private void Awake()
    {
        SetupEmpties();
    }
    private void SetupEmpties()
    {
        objPolEmptyHolder = new GameObject("Pooled Objects");

        particleSystemEmty = new GameObject("Particle System Effects");
        particleSystemEmty.transform.SetParent(objPolEmptyHolder.transform);

        gameObjEmpty = new GameObject("GameObjects");
        gameObjEmpty.transform.SetParent(objPolEmptyHolder.transform);

        entityEmpty = new GameObject("Entity");
        entityEmpty.transform.SetParent(objPolEmptyHolder.transform);


    }

    public static GameObject SpawnObject(GameObject objToSpawn, Vector3 spawnPos, Quaternion spawnRot, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objToSpawn.name);

        // PooledObjectInfo pool = null;
        // foreach (PooledObjectInfo p in objectPools){
        //     if (p.lookupString == obj.name){
        //         pool = p;
        //         break;
        //     }
        // }

        if (pool == null)
        {
            // if pool doesn't exist, create it
            pool = new PooledObjectInfo() { lookupString = objToSpawn.name };
            objectPools.Add(pool);
        }

        GameObject spawnableObj = null;
        foreach (GameObject o in pool.inactiveObjects)
        {
            // if there is an inactive object in pool
            if (o != null)
            {
                spawnableObj = o;
                break;
            }
        }

        if (spawnableObj == null)
        {
            GameObject parentObj = SetParentObject(poolType);

            // if there are no inactive objects, create a new one
            spawnableObj = Instantiate(objToSpawn, spawnPos, spawnRot);

            if (parentObj != null)
            {
                spawnableObj.transform.SetParent(parentObj.transform);
            }
        }
        else
        {
            // if there is an inactive object, reactivate it
            spawnableObj.transform.position = spawnPos;
            spawnableObj.transform.rotation = spawnRot;
            pool.inactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public static GameObject SpawnObject(GameObject objToSpawn, Transform parentTransform)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objToSpawn.name);

        // PooledObjectInfo pool = null;
        // foreach (PooledObjectInfo p in objectPools){
        //     if (p.lookupString == obj.name){
        //         pool = p;
        //         break;
        //     }
        // }

        if (pool == null)
        {
            // if pool doesn't exist, create it
            pool = new PooledObjectInfo() { lookupString = objToSpawn.name };
            objectPools.Add(pool);
        }

        GameObject spawnableObj = null;
        foreach (GameObject o in pool.inactiveObjects)
        {
            if (o != null)
            {
                spawnableObj = o;
                break;
            }
        }

        if (spawnableObj == null)
        {
            // if there are no inactive objects, create a new one
            spawnableObj = Instantiate(objToSpawn, parentTransform);
        }
        else
        {
            // if there is an inactive object, reactivate it
            pool.inactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }


    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7); // Removes "Clone"
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == goName);

        if (pool == null)
        {
            Debug.LogWarning("trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.inactiveObjects.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return particleSystemEmty;
            case PoolType.GameObject:
                return gameObjEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }

}

public class PooledObjectInfo
{
    public string lookupString;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}