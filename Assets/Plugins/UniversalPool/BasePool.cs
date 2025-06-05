using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UniversalPool
{
  public class BasePool<T> : IClearAble where T : Component
  {
    private int _maxCount;

    private Queue<GameObject> _pool;
    private List<GameObject> _inGame;
    private Dictionary<GameObject, ComponentBehaviour<T>> _components = new();

    private Transform _transform;

    public BasePool(T prefab, int initialCount, int maxCount = 0, Transform transform = null)
    {
      _transform = transform;

      _maxCount = maxCount;

      _pool = new();
      _inGame = new();
      CreatePool(initialCount, prefab, transform);
    }

    public void Clear()
    {
      foreach (var gameObject in _inGame)
      {
        if (_components.TryGetValue(gameObject, out var tBehaviour))
          tBehaviour.Behaviour.EventDisabled -= OnInstanceDisabled;
      }

      _pool.Clear();
      _inGame.Clear();
      _components.Clear();

      _transform = null;
    }

    public bool TryGetInstance(out T instance, T key, Vector3 position, Vector3? forward = null,
      Transform parent = null)
    {
      if (!key)
      {
        Debug.LogError("Cannot create object with null key/prefab");
        instance = null;
        return false;
      }

      GameObject gameObject = null;

      CheckHideFirstOne();

      if (_pool.Count == 0)
      {
        gameObject = CreateObject(key, position, forward, parent);
      }
      else
      {
        gameObject = _pool.Dequeue();

        if (!gameObject)
        {
          gameObject = CreateObject(key, position, forward, parent);
        }
        else
        {
          gameObject.transform.position = position;

          if (forward != null)
            gameObject.transform.forward = forward.Value;

          gameObject.transform.SetParent(parent);
        }
      }

      if (!gameObject)
      {
        instance = null;
        return false;
      }

      _inGame.Add(gameObject);

      var tbehaviour = _components[gameObject];
      instance = tbehaviour.Component;
      var behaviour = tbehaviour.Behaviour;

      behaviour.EventDisabled += OnInstanceDisabled;

      gameObject.SetActive(true);
      return true;
    }

    private void CreatePool(int count, T prefab, Transform parent)
    {
      for (int i = 0; i < count; i++)
      {
        var instance = CreateObject(prefab, Vector3.zero, null, parent);
        instance.SetActive(false);
        _pool.Enqueue(instance);
      }
    }

    private GameObject CreateObject(T key, Vector3 position, Vector3? forward = null, Transform parent = null)
    {
      if (!key)
      {
        Debug.LogError("Cannot create object with null key/prefab");
        return null;
      }

      var pooledObject = Object.Instantiate(key, position, key.transform.rotation, parent);

      if (forward != null)
        pooledObject.transform.forward = forward.Value;

      _components.Add(pooledObject.gameObject, new ComponentBehaviour<T>()
      {
        Component = pooledObject,
        Behaviour = CreateOrGetComponent<PooledBehaviour>(pooledObject.gameObject)
      });

      return pooledObject.gameObject;
    }

    private void OnInstanceDisabled(PooledBehaviour behaviour)
    {
      if (!behaviour)
        return;

      behaviour.EventDisabled -= OnInstanceDisabled;

      ReturnToPool(behaviour.gameObject).Forget();
    }

    private async UniTaskVoid ReturnToPool(GameObject instance)
    {
      if (!instance)
        return;

      _pool.Enqueue(instance);
      _inGame.Remove(instance);

      await UniTask.Yield();

      if (!instance)
        return;

      instance.transform.SetParent(_transform);
      instance.SetActive(false);
    }

    private void CheckHideFirstOne()
    {
      if (_maxCount <= 0 || _inGame == null || _inGame.Count < _maxCount || _inGame.Count == 0)
        return;

      var lastInstance = _inGame[0];
      if (lastInstance)
        lastInstance.gameObject.SetActive(false);
    }

    private TM CreateOrGetComponent<TM>(GameObject instance) where TM : Component
    {
      if (!instance)
      {
        Debug.LogError("Cannot add component to null GameObject");
        return null;
      }

      var component = instance.GetComponent<TM>();
      if (!component)
      {
        component = instance.AddComponent<TM>();
      }

      return component;
    }
  }
}