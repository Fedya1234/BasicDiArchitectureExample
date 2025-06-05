using UI.GameOver;
using UnityEngine;

namespace UI.Core
{
  public class CoreUIView : MonoBehaviour
  {
    [SerializeField] private WinMenu _winMenu;
    [SerializeField] private LoseMenu _loseMenu;

    private void Awake()
    {
      if (_winMenu == null || _loseMenu == null)
      {
        Debug.LogError("CoreUIView: WinMenu or LoseMenu is not assigned.");
      }
      
      _winMenu.gameObject.SetActive(false);
      _loseMenu.gameObject.SetActive(false);
    }

    public void ShowWinMenu()
    {
      _winMenu.gameObject.SetActive(true);
    }

    public void ShowLoseMenu()
    {
      _loseMenu.gameObject.SetActive(true);
    }
  }
}