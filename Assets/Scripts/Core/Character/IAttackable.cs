using UnityEngine;

namespace Core.Character
{
  public interface IAttackable
  {
    public void Attack(Vector3 force);
  }
}