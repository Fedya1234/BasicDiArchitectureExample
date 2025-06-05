using InputManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.GameplayUI
{
  public class MenuManager : MonoBehaviour
  {
    [FormerlySerializedAs("_gamePlayUI")] [SerializeField] private MenuUI _menuUI;
    [SerializeField] private InputSetter _inputSetter;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;

    private void Start()
    {
      _menuUI.gameObject.SetActive(false);
      _inputSetter.gameObject.SetActive(false);
    }
    

    private void OnGameFinished(bool isPlayerWin)
    {
      if (isPlayerWin)
      {
        _winMenu.SetActive(true);
      }
      else
      {
        _loseMenu.SetActive(true);
      }
    }
    

  }
}