using GameSetup;
using Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class AutoInitializerLoader
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void LoadMain()
  {
    Scene currentScene = SceneManager.GetActiveScene();
    if (currentScene.IsValid() && (currentScene.buildIndex != 0 || SceneManager.sceneCountInBuildSettings == 1))
    {
      FContainer dataManager = Object.FindFirstObjectByType<FContainer>();
      if (dataManager == null)
      {
        var dataManagerPrefab = GetAsset<GameObject>("GameManager");
        if (dataManagerPrefab != null)
        {
          Object.Instantiate(dataManagerPrefab);
        }
        else
        {
          Debug.LogError("[Game]: Initializer prefab is missing!");
        }
      }
    }
  }

  private static T GetAsset<T>(string name) where T : Object
  {
    var guids = AssetDatabase.FindAssets(name);
    if (guids.Length == 0)
    {
      Debug.LogError($"Asset with name {name} not found.");
      return null;
    }

    string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
    T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
    if (asset == null)
    {
      Debug.LogError($"Failed to load asset at path {assetPath}.");
    }

    return asset;
  }
}