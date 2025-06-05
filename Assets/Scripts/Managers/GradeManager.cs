using System;
using Data.Grades;
using UnityEngine;

namespace Managers
{
  public class GradeManager
  {
    public event Action EventGrade;

    private readonly GradeSaveData _gradeSaveData;
    private readonly GradeStaticData _gradeStaticData;
    private readonly CurrencyManager _currencyManager;
    private readonly SaveManager _saveManager;

    private GradeData CurrentGradeData => _gradeStaticData.GetLevelData(GradeIndex);
    private int GradeIndex => _gradeSaveData.GradeIndex;

    public int CurrentGradePrice => CurrentGradeData.Price;
    public bool IsMaxLevel => GradeIndex >= _gradeStaticData.MaxGradeLevel;

    public GradeManager(GradeSaveData gradeSaveData, GradeStaticData gradeStaticData, CurrencyManager currencyManager,
      SaveManager saveManager)
    {
      _gradeSaveData = gradeSaveData;
      _gradeStaticData = gradeStaticData;
      _currencyManager = currencyManager;
      _saveManager = saveManager;
    }

    public bool IsCanGrade()
    {
      var currencyAmount = _currencyManager.GetCurrencyAmount(_gradeStaticData.GradeCurrencyId);
      var isEnoughCurrency = currencyAmount >= CurrentGradeData.Price;
      
      return !IsMaxLevel && isEnoughCurrency;
    }

    public void Grade()
    {
      if (!IsCanGrade())
      {
        Debug.LogWarning($"GradeManager: Cannot grade. Max level reached or not enough currency.");
      }

      _currencyManager.AddCurrency(_gradeStaticData.GradeCurrencyId, -CurrentGradeData.Price);
      _gradeSaveData.GradeIndex++;

      _saveManager.Save();
      EventGrade?.Invoke();
    }
  }
}