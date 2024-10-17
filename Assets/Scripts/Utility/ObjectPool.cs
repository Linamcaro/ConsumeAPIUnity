using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> 
{
    private Transform prefab;
    private Transform container;
    private Queue<Transform> objects = new Queue<Transform>();

    public ObjectPool(Transform prefab, Transform container, int initialSize)
    {
        this.prefab = prefab;
        this.container = container;

        // Pre-instantiate a number of objects
        for (int i = 0; i < initialSize; i++)
        {
            Transform obj = GameObject.Instantiate(prefab, container);
            obj.gameObject.SetActive(false); // Keep them inactive initially
            objects.Enqueue(obj);
        }
    }

    // Get an object from the pool
    public Transform GetObject()
    {
        if (objects.Count > 0)
        {
            Transform obj = objects.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            // If no objects available in the pool, instantiate a new one
            Transform newObj = GameObject.Instantiate(prefab, container);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    // Return an object to the pool
    public void ReturnObject(Transform obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
}