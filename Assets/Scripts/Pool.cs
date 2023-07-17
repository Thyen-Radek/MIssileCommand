using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private GameObject _entityPrefab;

    private Queue<GameObject> _pool;

    private int _poolSize = 0;

    void Start()
    {
        _pool = new Queue<GameObject>();
    }

    public GameObject GetEntity()
    {
        GameObject entity = null;

        // Check if a entity is available in the _pool
        if (_pool.Count > 0)
        {
            entity = _pool.Dequeue();
        }
        else
        {
            entity = Instantiate(_entityPrefab, this.transform);
            _poolSize++;
        }

        // Activate the entity and return it
        entity.SetActive(true);
        return entity;
    }

    public void ReturnEntity(GameObject entity)
    {
        // Deactivate the entity and return it to the _pool
        entity.SetActive(false);
        _pool.Enqueue(entity);
    }

    public void ClearPool()
    {
        // Clear the _pool
        foreach (GameObject entity in _pool)
        {
            Destroy(entity);
        }
        _pool.Clear();
    }

    public int GetPoolSize()
    {
        return _poolSize;
    }

    public bool AllObjectsInPool()
    {
        return _pool.Count == _poolSize;
    }
}
