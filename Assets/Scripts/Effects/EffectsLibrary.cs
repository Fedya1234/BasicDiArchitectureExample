using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Effects
{
  [CreateAssetMenu(menuName = "ScriptableObjects/EffectsLibrary")]
  public class EffectsLibrary : SerializedScriptableObject
  {
    [SerializeField] private Dictionary<EffectTypeId, ParticleSystemPoolObject> _effects = new();
    [SerializeField] private ParticleSystemPoolObject _defaultEffect;
    [SerializeField] private Dictionary<EffectTypeId, int> _effectInitialCounts = new();

    public Dictionary<EffectTypeId, int> EffectInitialCounts => _effectInitialCounts;

    public ParticleSystemPoolObject Effect(EffectTypeId effectTypeId)
    {
      if (_effects.ContainsKey(effectTypeId) == false)
      {
        Debug.LogError($"No Effect {effectTypeId} in EffectsLibrary");
        return _defaultEffect;
      }

      return _effects[effectTypeId];
    }
  }
}