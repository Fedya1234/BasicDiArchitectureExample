using UnityEngine;

namespace Saves
{
  public class SaveableScriptableObject : ScriptableObject
  {
    private bool _isLoaded;
    
    private void OnEnable()
    {
      Load();
    }

    public void Load()
    {
      if (_isLoaded)
        return;
      
      string defaultText = JsonUtility.ToJson(this);
      string loadedString = PlayerPrefs.GetString(name, defaultText);
      JsonUtility.FromJsonOverwrite(loadedString, this);
      _isLoaded = true;
    }

    public void Save()
    {
      var text = JsonUtility.ToJson(this);
      PlayerPrefs.SetString(name, text);
      PlayerPrefs.Save();
    }


    public void Delete() => 
      PlayerPrefs.DeleteKey(name);
  }
}