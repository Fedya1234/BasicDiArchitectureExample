using UnityEngine;

namespace Tools
{
  public class StaticBehaviour<T> : MonoBehaviour where T : Component
  {
    private static T _instance;

    protected static T Instance
    {
      get
      {
        if (_instance)
          return _instance;

        _instance = FindFirstObjectByType<T>();
        if (_instance)
        {
          return _instance;
        }

        Debug.LogError($"StaticBehaviour: Instance of {typeof(T)} not found in the scene. Ensure it is added to a GameObject.");
        return null;
      }
    }

    private void Awake()
    {
      if (_instance != null && _instance != this)
      {
        Destroy(gameObject);
        return;
      }
      
      if (_instance == null)
      {
        _instance = this as T;
      }

      OnAwake();
    }

    private void OnDestroy()
    {
      if (_instance == this)
      {
        _instance = null;
      }
    }

    protected virtual void OnAwake()
    {
    }
  }
}