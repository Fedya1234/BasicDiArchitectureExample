using Core.Scores;
using UI.Core;
using UnityEngine;

namespace GameSetup
{
  public class GameSceneInitializer : BaseSceneInstaller
  {
    [SerializeField] private CoreUIView _coreUIView;
    [SerializeField] private ScoreManager _scoreManager;
    protected override void InstallBindings()
    {
      GameManager.Add(_scoreManager);
      GameManager.Add(_coreUIView);
    }
  }
}