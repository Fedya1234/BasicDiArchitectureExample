using Data.Saves;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class SaveReseter : IPreprocessBuildWithReport
{
      public int callbackOrder { get { return 0; } }
      public void OnPreprocessBuild(BuildReport report)
      {
          var scriptableObject = AssetDatabase.LoadAssetAtPath<SaveData>("Assets/SO/SaveData.asset");
          scriptableObject.Clear();
      }
}

