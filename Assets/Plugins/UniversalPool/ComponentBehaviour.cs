using UnityEngine;

namespace UniversalPool
{
  public class ComponentBehaviour<T> where T : Component
  {
    public T Component;
    public PooledBehaviour Behaviour;
  }
}