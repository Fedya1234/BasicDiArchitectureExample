using UI.Gates;
using UnityEngine.UI;

namespace UI.Tools
{
  public class MenuButton : Button
  {
    protected override void Awake()
    {
      base.Awake();
      onClick.AddListener(OnTap);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      onClick.RemoveAllListeners();
    }
    
    private void OnTap()
    {
      GatesManager.LoadSceneAsync(SceneId.MainMenu); 
    }
  }
}