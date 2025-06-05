using Core.Scores;
using GameSetup;
using TMPro;
using UnityEngine;

namespace UI.Core
{
  public class ScoreIndicator : MonoBehaviour
  {
    [SerializeField] private TMP_Text _scoreText;
    
    private ScoreManager _scoreManager;
    private void Awake()
    {
      _scoreManager = GameManager.Get<ScoreManager>();
    }
    
    private void OnEnable()
    {
      _scoreManager.EventScoreChanged += OnScoreChanged;
    }
    
    private void OnDisable()
    {
      _scoreManager.EventScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged()
    {
      if (_scoreText != null)
        _scoreText.text = _scoreManager.Score.ToString();
    }

    private void Start()
    {
      OnScoreChanged();
    }
  }
}