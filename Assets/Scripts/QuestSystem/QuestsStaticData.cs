using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QuestSystem
{
  [CreateAssetMenu(fileName = "QuestsStaticData", menuName = "QuestsSystem/QuestsStaticData")]
  public class QuestsStaticData : SerializedScriptableObject
  {
    public static int NextQuestSecondsDelay = 10;
    [SerializeField] private int _maxQuestsCount = 2;
    [SerializeField] private Dictionary<QuestId, Quest> _quests = new();
    [SerializeField] private Dictionary<QuestDifficulty, int> _prizes = new();
    [SerializeField] private List<QuestId> _questsOrder = new();

    public Dictionary<QuestId, Quest> Quests =>
      _quests;

    public List<QuestId> QuestsOrder => _questsOrder;

    public int MaxQuestsCount => _maxQuestsCount;

    public Quest GetQuest(QuestId id)
    {
      if (_quests.TryGetValue(id, out var achievement))
        return achievement;

      Debug.LogWarning($"Achievement with id {id} not found");
      return null;
    }

    public int GetPrize(QuestDifficulty difficulty)
    {
      if (_prizes.TryGetValue(difficulty, out var prize))
        return prize;

      Debug.LogWarning($"Prize with difficulty {difficulty} not found");
      return 0;
    }

    public QuestId GetQuestId(Quest quest)
    {
      foreach (var (id, ach) in _quests)
        if (ach == quest)
          return id;

      Debug.LogWarning($"Quest {quest} not found");
      return QuestId.None;
    }
  }
}