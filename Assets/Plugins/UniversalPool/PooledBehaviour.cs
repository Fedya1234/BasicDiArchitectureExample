using System;
using UnityEngine;

namespace UniversalPool
{
  public class PooledBehaviour : MonoBehaviour
  {
    public event Action<PooledBehaviour> EventDisabled;
    private bool _isDestroyed;

    private void OnDestroy()
    {
      _isDestroyed = true;
    }

    private void OnDisable()
    {
      if (_isDestroyed)
        return;

      EventDisabled?.Invoke(this);
    }
  }
}