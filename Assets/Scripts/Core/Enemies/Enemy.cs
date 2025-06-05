using System.Collections;
using Core.Scores;
using Effects;
using GameSetup;
using UnityEngine;

namespace Core.Enemies
{
  public class Enemy : MonoBehaviour
  {
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private EffectTypeId _deathEffectTypeId;
    [SerializeField] private int _scoreValue = 1;
    [SerializeField] private float _deathDelay = 0.3f;

    private ScoreManager _scoreManager;
    
    private void Awake()
    {
      _scoreManager = GameManager.Get<ScoreManager>();
      
      if (_rigidbody == null)
      {
        if (TryGetComponent(out _rigidbody) == false)
        {
          Debug.LogError("Rigidbody is not assigned and could not be found on the Enemy object.");
        }
      }
    }

    private void OnEnable()
    {
      if (_rigidbody != null)
      {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
      }
    }
    
    public void Attack(Vector3 force)
    {
      if (_rigidbody != null) 
        _rigidbody.AddForce(force, ForceMode.Impulse);
      
      StartCoroutine(KillWithDelay(_deathDelay));
    }
    
    private IEnumerator KillWithDelay(float delay)
    {
      yield return new WaitForSeconds(delay);
      
      if (_deathEffectTypeId != EffectTypeId.None)
        EffectsManager.Create(_deathEffectTypeId, transform.position, transform.forward);
      
      _scoreManager.AddScore(_scoreValue);
      
      gameObject.SetActive(false);
    }
  }
}