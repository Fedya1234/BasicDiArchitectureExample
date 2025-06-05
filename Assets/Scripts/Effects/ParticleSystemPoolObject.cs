using System.Collections;
using UnityEngine;

namespace Effects
{
  [RequireComponent(typeof(ParticleSystem))]
  public class ParticleSystemPoolObject : MonoBehaviour
  {
    private ParticleSystem _particleSystem;

    private void Awake()
    {
      TryGetComponent(out _particleSystem);

      var main = _particleSystem.main;
      main.stopAction = ParticleSystemStopAction.Disable;
    }

    private void OnEnable() =>
      _particleSystem.Play(true);

    public void Stop() =>
      _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

    public void Stop(float delay)
    {
      StartCoroutine(StopWithDelay(delay));
    }

    private IEnumerator StopWithDelay(float delay)
    {
      yield return new WaitForSeconds(delay);
      Stop();
    }
  }
}