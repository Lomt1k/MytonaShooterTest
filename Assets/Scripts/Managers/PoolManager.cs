using System.Collections.Generic;
using UnityEngine;

public class PoolManager<T> where T: IPoolObject
{
    private Queue<T> _objectPool;
    private GameObject _poolObject;

    public PoolManager(GameObject go, int count)
    {
        _objectPool = new Queue<T>();
        _poolObject = go; 
        CreateStartObjects(count);
    }

    public T GetPoolObject()
    {
        if (_objectPool.Count < 1)
        {
            CreateStartObjects();
        }
        T obj = _objectPool.Dequeue();
        obj.OnPop();
        return obj;
    }

    public void Push(T poolObject)
    {
        _objectPool.Enqueue(poolObject);
    }

    private void CreateStartObjects(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = GameObject.Instantiate(_poolObject, Vector3.zero, _poolObject.transform.rotation);
            go.SetActive(false);
            _objectPool.Enqueue(go.GetComponent<T>());
        }       
    }

}
