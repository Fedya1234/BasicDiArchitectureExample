using Data.Currencies;
using Data.Grades;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Saves
{
  [CreateAssetMenu(fileName = "SaveData", menuName = "Data/SaveData")]
  public class SaveData : ScriptableObject
  {
    public CurrencySaveData CurrencySaveData;
    public GradeSaveData GradeSaveData;

    [Button]
    private void ClearSaveData()
    {
      Clear();
      Debug.Log($"SaveData: {name} cleared.");
    }
    
    private void OnValidate()
    {
      if (CurrencySaveData == null) 
        Debug.LogError($"SaveData: {name} CurrencySaveData is null. Initializing with default values.");

      if (GradeSaveData == null) 
        Debug.LogError($"SaveData: {name} GradeSaveData is null. Initializing with default values.");
    }

    public void Clear()
    {
      CurrencySaveData = new CurrencySaveData();
      GradeSaveData = new GradeSaveData();
    }
  }
}