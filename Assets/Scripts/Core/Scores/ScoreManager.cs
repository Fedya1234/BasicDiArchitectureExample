using System;
using Data.Currencies;
using GameSetup;
using Managers;
using UI.Core;
using UnityEngine;

namespace Core.Scores
{
  public class ScoreManager : MonoBehaviour
  {
    [SerializeField] private int _winScore = 10;
    
    public event Action EventScoreChanged;
    
    private int _score;

    public int Score => _score;
    
    private CoreUIView _coreUIView;
    private CurrencyManager _currencyManager;
    private SaveManager _saveManager;
    private void Awake()
    {
      _coreUIView = GameManager.Get<CoreUIView>();
      _currencyManager = GameManager.Get<CurrencyManager>();
      _saveManager = GameManager.Get<SaveManager>();
      _score = 0;
    }

    public void AddScore(int value)
    {
      _score += value;
      if (_score < 0) 
        _score = 0;

      if (_score >= _winScore)
      {
        _currencyManager.AddCurrency(CurrencyId.SoftCurrency, _score);
        _coreUIView.ShowWinMenu();
        //TODO: Implement level ++ logic
        _saveManager.Save();
      }
      
      EventScoreChanged?.Invoke();
    }
  }
}