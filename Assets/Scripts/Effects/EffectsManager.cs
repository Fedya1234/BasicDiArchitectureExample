using System.Collections;
using Sirenix.OdinInspector;
using Tools;
using UnityEngine;
using UniversalPool;

namespace Effects
{
  public class EffectsManager : StaticBehaviour<EffectsManager>
  {
    [SerializeField] private EffectsLibrary _effectsLibrary;
    [SerializeField] private bool _initializeOnStart = true;
    [SerializeField] private bool _isUseTimeBudget;
    [SerializeField][ShowIf("_isUseTimeBudget")] private float _maxMillisecondsPerFrame = 30f;
    private void Start()
    {
      if (!_effectsLibrary)
      {
        Debug.LogError("NO EffectsLibrary is EffectsManager");
        return;
      }
      if (_initializeOnStart)
      {
        if (_isUseTimeBudget)
          StartCoroutine(InitializePoolsWithTimeBudget(_maxMillisecondsPerFrame));
        else
          InitializePools();
      }
    }
    public static ParticleSystemPoolObject Create(EffectTypeId typeId, Vector3 position, Vector3? direction = null,
      Transform parent = null, float scale = 1f)
    {
      return Instance.CreateInstance(typeId, position, direction, parent, scale);
    }

    public ParticleSystemPoolObject CreateInstance(EffectTypeId typeId, Vector3 position, Vector3? direction = null,
      Transform parent = null, float scale = 1f)
    {
      var effectData = PoolObjectByTypeId(typeId);
      if (!effectData)
      {
        Debug.LogError("NO EffectsLibrary is EffectsManager");
        return null;
      }

      if (PoolFactory.TryGetInstance(out var instance, effectData, position, direction, parent))
      {
        instance.transform.localScale = effectData.transform.localScale * scale;
        return instance;
      }

      return null;
    }
    
    public IEnumerator InitializePoolsWithTimeBudget(float maxMillisecondsPerFrame)
    {
      var startTime = Time.realtimeSinceStartup;

      var toPool = _effectsLibrary.EffectInitialCounts;

      foreach (var effect in toPool)
      {
        if (effect.Value == 0)
          continue;

        var effectData = PoolObjectByTypeId(effect.Key);
        if (effectData)
          PoolFactory.FillPool(effectData, effect.Value, effect.Value);

        var elapsed = (Time.realtimeSinceStartup - startTime) * 1000f;
        if (elapsed >= maxMillisecondsPerFrame)
        {
          yield return null;
          startTime = Time.realtimeSinceStartup;
        }
      }
    }
    
    public void InitializePools()
    {
      if (!_effectsLibrary)
      {
        Debug.LogError("NO EffectsLibrary is EffectsManager");
        return;
      }

      foreach (var effect in _effectsLibrary.EffectInitialCounts)
      {
        if (effect.Value == 0)
          continue;

        var effectData = PoolObjectByTypeId(effect.Key);
        if (effectData)
          PoolFactory.FillPool(effectData, effect.Value, effect.Value);
      }
    }

    private ParticleSystemPoolObject PoolObjectByTypeId(EffectTypeId typeId) =>
      _effectsLibrary ? _effectsLibrary.Effect(typeId) : null;
  }
}