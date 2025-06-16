using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameSetup;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.View
{
  public class MenuQuestsShower : MonoBehaviour
  {
    [SerializeField] private Transform _questsContainer;
    [SerializeField] private MenuQuestView _questViewPrefab;
    [SerializeField] private RectTransform _layoutGroup;

    private List<MenuQuestView> _questsViews = new();

    private QuestsManager _questsManager;

    private void Awake()
    {
      _questsManager = FContainer.Get<QuestsManager>();
    }

    private void Start()
    {
      ShowQuests();
    }

    private void ShowQuests()
    {
      HideQuests();

      var quests = _questsManager.CurrentQuests
        .OrderBy(quest => quest.IsCompleted == false)
        .ToList();

      foreach (var quest in quests)
      {
        var viewInstance = Instantiate(_questViewPrefab, _questsContainer);
        viewInstance.Init(quest, OnQuestFinished);
        _questsViews.Add(viewInstance);
      }

      //update vertical layout group size
      LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup);
    }

    private void HideQuests()
    {
      foreach (var questView in _questsViews)
        Destroy(questView.gameObject);

      _questsViews.Clear();

      foreach (Transform child in _questsContainer)
        Destroy(child.gameObject);
    }

    private void OnQuestFinished(MenuQuestView menuQuestView)
    {
      _questsManager.RemoveFinishedQuests();
      StartCoroutine(ShowQuestsWithDelay());
    }
    
    private IEnumerator ShowQuestsWithDelay()
    {
      yield return new WaitForSeconds(0.1f);
      ShowQuests();
    }
  }
}