using Core.Enemies;
using InputManager;
using UnityEngine;

namespace Core.Character
{
  public class AttackController : MonoBehaviour
  {
    private const int MaxAttackTargets = 10;
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    [SerializeField] private InputController _inputController;
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackForce = 300f;

    private readonly Collider[] _results = new Collider[MaxAttackTargets];
    private void OnEnable()
    {
      _inputController.EventCast += Attack;
    }

    private void OnDisable()
    {
      _inputController.EventCast -= Attack;
    }

    private void Attack()
    {
      _animator.SetTrigger(AttackHash);
    }

    private void OnAttack() //Called from Animation
    {
      var attackPosition = transform.position + transform.forward * 0.5f * _attackRange;
      var size = Physics.OverlapSphereNonAlloc(attackPosition, _attackRange, _results, _targetLayerMask);
      if (size == 0)
        return;

      for (int i = 0; i < size; i++)
      {
        var hitCollider = _results[i];

        if (hitCollider.TryGetComponent(out IAttackable enemy))
        {
          var toTarget = hitCollider.transform.position - transform.position;
          var distance = toTarget.magnitude;
          var forceMultiplier = 1f - Mathf.Clamp01(distance / (_attackRange * 2f));
          var direction = toTarget.normalized;
          
          enemy.Attack(direction * _attackForce * forceMultiplier);
        }
      }
    }
  }
}