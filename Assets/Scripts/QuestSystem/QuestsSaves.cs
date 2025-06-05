using System.Collections.Generic;
using Saves;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "QuestsSaves", menuName = "QuestsSystem/QuestsSaves")]
    public class QuestsSaves : SaveableScriptableObject
    {
        [SerializeField] private Dictionary<QuestId, QuestSaveData> _quests = new ();
        [SerializeField] private List<QuestId> _currentQuests = new ();
        [SerializeField] private List<QuestId> _finishedQuests = new ();
        
        public List<QuestId> FinishedQuests => _finishedQuests;
        public List<QuestId> CurrentQuests => _currentQuests;

        [Button]
        public void ResetData()
        {
            _quests.Clear();
            _currentQuests.Clear();
            _finishedQuests.Clear();
        }
        
        public QuestSaveData GetSave(QuestId id)
        {
            if (_quests.TryGetValue(id, out var saveData)) 
                return saveData;
            
            saveData = new QuestSaveData();
            _quests.Add(id, saveData);
            return saveData;
        }
    }
}