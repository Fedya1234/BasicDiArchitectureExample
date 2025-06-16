using Core.Scores;
using Effects;
using UI.Core;
using UnityEngine;

namespace GameSetup
{
  public class GameSceneInitializer : BaseSceneInstaller
  {
    [SerializeField] private CoreUIView _coreUIView;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private EffectsManager _effectsManager;
    protected override void InstallBindings()
    {
      FContainer.Add(_scoreManager);
      FContainer.Add(_coreUIView);
      FContainer.Add(_effectsManager);
    }
  }
}