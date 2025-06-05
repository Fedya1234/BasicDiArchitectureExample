using UI.Gates;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameOver
{
  public class LoseMenu : MonoBehaviour
  {
    [SerializeField] private Button _mainMenuButton;

    private void OnEnable()
    {
      _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnDisable()
    {
      _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
      GatesManager.LoadScene(SceneId.MainMenu);
    }
  }
}