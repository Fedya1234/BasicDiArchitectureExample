using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestSystem
{
  public class QuestsManager : MonoBehaviour
  {
    public Action<Quest> OnDataUpdate;

    [SerializeField] private QuestsSaves _saves;
    [SerializeField] private QuestsStaticData _questsStaticData;
    
    private List<QuestId> _currentQuests => _saves.CurrentQuests;
    private List<QuestId> _finishedQuests => _saves.FinishedQuests;
    public IEnumerable<Quest> CurrentQuests => AllQuests.Where(quest => _currentQuests.Contains(quest.Id));
    public IEnumerable<Quest> AllQuests => _questsStaticData.Quests.Values;

    public List<QuestId> NewCompletedQuests =>
      AllQuests.Where(quest => quest.IsCompleted && quest.IsPrizeTaken == false)
        .Select(quest => quest.Id)
        .ToList();

    public List<Quest> CompletedCurrentQuests =>
      CurrentQuests.Where(quest => quest.IsCompleted)
        .ToList();

    private void Awake()
    {
      InitStaticData();
        
      AddCurrentQuestsListeners();
      RemoveFinishedQuests();
    }

    private void OnDestroy()
    {
      var currentQuests = CurrentQuests.ToList();
      foreach (var quest in currentQuests) 
        RemoveQuestListeners(quest);
    }

    public void RemoveFinishedQuests()
    {
      var questsToRemove = AllQuests
        .Where(quest => quest.IsNeedToReset)
        .ToList();

      foreach (var quest in questsToRemove) 
        RemoveQuestListeners(quest);
      
      //ResetQuests(questsToRemove);

      var lastQuests = questsToRemove.Select(quest => quest.Id)
        .ToList();
      
      foreach (var quest in lastQuests)
      {
        if (_saves.FinishedQuests.Contains(quest) == false)
          _saves.FinishedQuests.Add(quest);
      }
      
      _currentQuests.RemoveAll(lastQuests.Contains);

      UpdateQuests();
    }
    
    private void AddCurrentQuestsListeners()
    {
      var currentQuests = CurrentQuests.ToList();
      foreach (var quest in currentQuests) 
        AddQuestListeners(quest);
    }

    private void UpdateQuests()
    {
      var currentQuests = CurrentQuests.ToList();
      var notCompletedQuestsCount = CurrentQuests.Count(quest => quest.IsCompleted == false);
      
      var addCount = _questsStaticData.MaxQuestsCount - notCompletedQuestsCount;
      if (addCount <= 0)
        return;
      
      var questsToAdd = AllQuests
        .Where(quest => _finishedQuests.Contains(quest.Id) == false && quest.IsNeedToReset == false && currentQuests.Contains(quest) == false)
        .ToList();

      if (questsToAdd.Count < addCount)
      {
        var toReset = AllQuests.Where(quest => quest.IsNeedToReset).ToList();
        ResetQuests(toReset);
        _finishedQuests.Clear();
        
        questsToAdd = AllQuests
          .Where(quest => _finishedQuests.Contains(quest.Id) == false && quest.IsNeedToReset == false && currentQuests.Contains(quest) == false)
          .ToList();
      }
      
      questsToAdd = questsToAdd
        .OrderBy(quest =>
        {
          var index = _questsStaticData.QuestsOrder.IndexOf(quest.Id);
          return index == -1 ? _questsStaticData.QuestsOrder.Count + Random.Range(1,1000000) : index;
        })
        .Take(addCount)
        .ToList();
      
      foreach (var quest in questsToAdd) 
        AddQuestListeners(quest);

      _currentQuests.AddRange(questsToAdd.Select(quest => quest.Id));
    }

    private void AddQuestListeners(Quest quest)
    {
      quest.EventQuestUpdated += OnQuestUpdated;
      quest.AddListener();
    }

    private void RemoveQuestListeners(Quest quest)
    {
      quest.EventQuestUpdated -= OnQuestUpdated;
      quest.RemoveListener();
    }

    private void InitStaticData()
    {
      foreach (var (questId, quest) in _questsStaticData.Quests) 
        quest.Init(GetSave(questId), questId, _questsStaticData);
    }
    
    private void ResetQuests(List<Quest> quests)
    {
      foreach (var quest in quests) 
        quest.Reset();
    }

    private void OnQuestUpdated(Quest quest)
    {
      var data = quest.SaveData;
      
      data.Count = quest.Count;

      if (quest.IsCompleted || quest.IsPrizeTaken || quest.IsNeedToReset)
      {
        RemoveFinishedQuests();
        Save();
      }

      OnDataUpdate?.Invoke(quest);
    }
    
    private void Save()
    {
      _saves.Save();
    }
    
    private QuestSaveData GetSave(QuestId id) =>
      _saves.GetSave(id);
    
    private Quest GetQuest(QuestId id) =>
      _questsStaticData.GetQuest(id);

  }
}