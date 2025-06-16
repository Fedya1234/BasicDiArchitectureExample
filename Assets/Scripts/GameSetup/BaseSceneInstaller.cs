using System;
using UnityEngine;

namespace GameSetup
{
  [DefaultExecutionOrder(-100)]
  public abstract class BaseSceneInstaller : MonoBehaviour
  {
    protected abstract void InstallBindings();

    private void Awake()
    {
      InstallBindings();
    }

    private void OnDestroy()
    {
      FContainer.ClearSceneContainer();
    }
  }
}