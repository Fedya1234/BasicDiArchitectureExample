using System;
using Data.Currencies;
using UnityEngine;

namespace Data.Grades
{
  [CreateAssetMenu(fileName = "GradeStaticData", menuName = "Data/GradeStaticData")]
  public class GradeStaticData : ScriptableObject
  {
    [SerializeField] private GradeData[] _gradeData;
    [SerializeField] private CurrencyId _gradeCurrencyId;
    public CurrencyId GradeCurrencyId => _gradeCurrencyId;
    public int MaxGradeLevel => _gradeData.Length - 1;
    private void OnValidate()
    {
      if (_gradeData.Length == 0)
      {
        Debug.LogError($"GradeStaticData: {name} must contain at least one grade data entry.");
      }
    }

    public GradeData GetLevelData(int level)
    {
      if (level < 0)
        throw new ArgumentOutOfRangeException(nameof(level), "Level is out of range.");
      
      if (level >= _gradeData.Length)
        level = _gradeData.Length - 1;
      
      return _gradeData[level];
    }
  }
}