using System.Collections.Generic;
using UnityEngine;

namespace UniversalPool
{
  public class PoolFactory : MonoBehaviour
  {
    private static PoolFactory _instance;

    private static PoolFactory Instance
    {
      get
      {
        if (_instance)
          return _instance;

        _instance = FindFirstObjectByType<PoolFactory>();
        if (_instance)
          return _instance;

        var obj = new GameObject("PoolFactory");
        _instance = obj.AddComponent<PoolFactory>();
        return _instance;
      }
    }

    private readonly Dictionary<int, IClearAble> _typedPools = new();

    private void Awake()
    {
      if (!_instance)
        _instance = this;
      else
      {
        if (_instance != this)
        {
          Destroy(gameObject);
          return;
        }
      }
    }

    private void OnDestroy()
    {
      if (_instance == this)
        _instance = null;
    }

    private void OnDisable()
    {
      foreach (var pool in _typedPools.Values)
      {
        if (pool == null)
          continue;
        
        pool.Clear();
      }

      _typedPools.Clear();
    }

    public static bool TryGetInstance<T>(out T instance, T key, Vector3 position, Vector3? forward = null,
      Transform parent = null) where T : Component
    {
      var instanceID = key.gameObject.GetInstanceID();

      if (!Instance._typedPools.TryGetValue(instanceID, out var poolObj))
      {
        poolObj = Instance.CreatePool(key, 1);
        Debug.LogWarning($"Pool created on Fly for {key.name} Please Make it onScene load stage");
      }

      var typedPool = (BasePool<T>) poolObj;

      return typedPool.TryGetInstance(out instance, key, position, forward, parent);
    }

    public static void FillPool<T>(T prefab, int initialCount, int maxCount = 0) where T : Component
    {
      if (Instance._typedPools.ContainsKey(prefab.gameObject.GetInstanceID()))
      {
        Debug.LogWarning($"Pool for {prefab.name} already exists");
        return;
      }

      Instance.CreatePool(prefab, initialCount, maxCount);
    }
    
    public static void RemovePool<T>(T prefab) where T : Component
    {
      var instanceId = prefab.gameObject.GetInstanceID();
      if (Instance._typedPools.TryGetValue(instanceId, out var pool))
      {
        pool.Clear();
        Instance._typedPools.Remove(instanceId);

        var poolGoName = $"{prefab.name} Pool";
        var poolTransform = Instance.transform.Find(poolGoName);
        if (poolTransform != null)
        {
          Destroy(poolTransform.gameObject);
        }
      }
      else
      {
        Debug.LogWarning($"No pool found for prefab {prefab.name} to remove.");
      }
    }

    private BasePool<T> CreatePool<T>(T prefab, int initialCount, int maxCount = 0) where T : Component
    {
      var instanceId = prefab.gameObject.GetInstanceID();
      var newGo = new GameObject($"{prefab.name} Pool");
      newGo.transform.SetParent(transform);

      var pool = new BasePool<T>(prefab, initialCount, maxCount, newGo.transform);
      _typedPools.Add(instanceId, pool);

      return pool;
    }
  }
}
