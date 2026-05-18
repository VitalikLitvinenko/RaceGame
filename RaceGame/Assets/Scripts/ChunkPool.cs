using System.Collections.Generic;
using UnityEngine;

public class ChunkPool
{
    private List<GameObject> _prefabs;
    private Queue<GameObject> _pool = new Queue<GameObject>();
    private Transform _parent;
    private IChunkFactory _factory;
    private int _currentIndex = 0;

    public ChunkPool(List<GameObject> prefabs, Transform parent, int initialSize, IChunkFactory factory)
    {
        _prefabs = prefabs;
        _parent = parent;
        _factory = factory;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNew();
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject Get(Vector3 position)
    {
        GameObject obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = CreateNew();
        }

        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }

    private GameObject CreateNew()
    {
        GameObject obj = _factory.Create(_prefabs[_currentIndex], _parent);
        _currentIndex = (_currentIndex + 1) % _prefabs.Count;
        return obj;
    }
}