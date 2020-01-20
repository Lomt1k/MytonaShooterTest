using System.Collections.Generic;
using UnityEngine;

public class PoolManager<T> where T: IPoolObject
{
    Queue<T> objectPool;
    GameObject poolObject;

    public PoolManager(GameObject go, int count)
    {
        objectPool = new Queue<T>();
        poolObject = go; 
        CreateStartObjects(count);
    }
         
    private void CreateStartObjects(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = GameObject.Instantiate(poolObject, Vector3.zero, Quaternion.identity);
            go.SetActive(false);
            objectPool.Enqueue(go.GetComponent<T>());
        }       
    }

    public T GetPoolObject()
    {
        if (objectPool.Count < 1)
        {
            CreateStartObjects();
        }
        T obj = objectPool.Dequeue();
        obj.OnPop();
        return obj;
    }

    public void Push(T poolObject)
    {
        objectPool.Enqueue(poolObject);
    }

}
