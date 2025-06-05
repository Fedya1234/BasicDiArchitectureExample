using System;
using UnityEngine;

namespace QuestSystem
{
  public class Quest
  {
    public event Action<Quest> EventQuestUpdated;

    [SerializeField] private int _maxCount = 1;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private string _shortDescription;
    [SerializeField] private Sprite _icon;
    [SerializeField] private QuestDifficulty _difficulty;
    private QuestId _id;
    private QuestSaveData _saveData;
    private QuestsStaticData _staticData;
    
    public QuestSaveData SaveData => _saveData;
    public QuestId Id => _id;
    public int MaxCount => _maxCount;
    public int Count => _saveData.Count;
    public bool IsCompleted => Count >= MaxCount;
    public bool IsPrizeTaken => _saveData.IsPrizeTaken;
    public bool IsNextQuestReady => IsPrizeTaken && _saveData.NextQuestDateTime <= DateTime.Now; // No More Wait Pause
    public bool IsNeedToReset => IsCompleted && IsPrizeTaken && IsNextQuestReady;
    public TimeSpan TimeToNextQuest => _saveData.NextQuestDateTime - DateTime.Now;
    public Sprite Icon => _icon;
    
    public QuestDifficulty Difficulty => _difficulty;
    
    public int Prize => _staticData.GetPrize(_difficulty);
    
    public string Name => _name;
    public string Description => _description;
    public string ShortDescription => string.IsNullOrEmpty(_shortDescription) ? Description.Substring(0, Description.IndexOf(" ", StringComparison.Ordinal)) : _shortDescription;
    
    //Replace {0} to MaxCount and make Rich text Green color
    public string DescriptionWithMaxCount =>
      _description.Replace("{0}", $"<color=green>{MaxCount}</color>");
    
    public virtual bool IsCanBeSelected =>
      true;
    
    public void Init(QuestSaveData saveData, QuestId id, QuestsStaticData staticData)
    {
      _id = id;
      _saveData = saveData;
      _staticData = staticData;
      
      OnInitialized();
    }

    public virtual void RemoveListener()
    {
      
    }

    public virtual void AddListener()
    {
      
    }
    
    public void Reset() => 
      _saveData.Reset();
    
    public void TakePrize()
    {
      //Smells like it should be in other place
      //DataManager.CurrencyManager.AddCurrency(CurrencyId.SoftCurrency, Prize, DataManager.CharactersDataManager.SelectedCharacter);
      
      _saveData.TakePrize(false);
      EventQuestUpdated?.Invoke(this);
    }
    
    public void SkipQuest()
    {
      _saveData.Count = MaxCount;
      _saveData.TakePrize(true);
      EventQuestUpdated?.Invoke(this);
    }
    
    protected virtual void OnInitialized()
    {
      
    }

    protected void AddProgress(int count)
    {
      _saveData.Count += count;
      
      if (Count >= MaxCount)
      {
        _saveData.Count = MaxCount;
        RemoveListener();
        //AnalyticsManager.LogQuestCompleted(Id.ToString(), DataManager.AnalyticsDataManager.Session);
      }
      
      EventQuestUpdated?.Invoke(this);
    }
  }
}