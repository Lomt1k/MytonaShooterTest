using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTonaShooterTest.VFX;

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
            var newObject = GameObject.Instantiate(poolObject, Vector3.zero, Quaternion.identity).GetComponent<T>();
            objectPool.Enqueue(newObject);        
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
