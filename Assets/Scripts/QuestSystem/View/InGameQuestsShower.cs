using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameSetup;
using UnityEngine;

namespace QuestSystem.View
{
  public class InGameQuestsShower : MonoBehaviour
  {
    [SerializeField] private Transform _questsContainer;
    [SerializeField] private InGameQuestView _questViewPrefab;
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private float _showDelay = 0.5f;

    private Dictionary<Quest, InGameQuestView> _questsViews = new();

    private QuestsManager _questsManager;

    private void Awake()
    {
      _questsManager = FContainer.Get<QuestsManager>();
    }

    private void OnEnable()
    {
      StartCoroutine(AddNewQuestCoroutine());
    }

    private void OnDisable()
    {
      StopAllCoroutines();
      ClearQuests();
    }

    private void AddQuestView(Quest quest)
    {
      var viewInstance = Instantiate(_questViewPrefab, _questsContainer);
      viewInstance.Init(quest);
      quest.EventQuestUpdated += OnQuestUpdated;
      _questsViews.Add(quest, viewInstance);
    }

    private void OnQuestUpdated(Quest quest)
    {
      if (quest.IsCompleted)
      {
        RemoveQuestView(quest);
        StopAllCoroutines();
        StartCoroutine(AddNewQuestCoroutine());
      }
    }

    private IEnumerator AddNewQuestCoroutine()
    {
      yield return new WaitForSeconds(_startDelay);
      var newQuests = _questsManager.CurrentQuests
        .Where(quest => quest.IsCompleted == false && _questsViews.ContainsKey(quest) == false)
        .ToList();

      foreach (var quest in newQuests)
      {
        AddQuestView(quest);
        yield return new WaitForSeconds(_showDelay);
      }
    }

    private void ClearQuests()
    {
      foreach (var quest in _questsViews.Keys.ToList())
      {
        RemoveQuestView(quest);
      }
    }

    private void RemoveQuestView(Quest quest)
    {
      quest.EventQuestUpdated -= OnQuestUpdated;
      var view = _questsViews[quest];
      view.gameObject.SetActive(false);
      _questsViews.Remove(quest);
    }
  }
}