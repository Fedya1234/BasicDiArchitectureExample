using UnityEngine;
using UnityEngine.AI;

namespace Core.Character
{
  public class SetAnimationSpeedByAgentSpeed : MonoBehaviour
  {
    private static readonly int Speed = Animator.StringToHash("Speed");
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;

    private void Update()
    {
      _animator.SetFloat(Speed, _agent.velocity.magnitude);
    }
  }
}