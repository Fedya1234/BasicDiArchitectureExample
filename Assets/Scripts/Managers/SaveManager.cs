using Data.Saves;
using UnityEngine;

namespace Managers
{
  public class SaveManager
  {
    private SaveData _saveData;

    public SaveManager(SaveData saveData)
    {
      _saveData = saveData;
      Load();
    }
    
    private void Load()
    {
      var defaultText = JsonUtility.ToJson(this);
      var loadedString = PlayerPrefs.GetString(_saveData.name, defaultText);
      JsonUtility.FromJsonOverwrite(loadedString, this);
    }

    public void Save()
    {
      var text = JsonUtility.ToJson(this);
      PlayerPrefs.SetString(_saveData.name, text);
      PlayerPrefs.Save();
    }


    public void Delete() => 
      PlayerPrefs.DeleteKey(_saveData.name);
  }
}