using System;
using System.Globalization;

namespace QuestSystem
{
  [Serializable]
  public class QuestSaveData
  {
    public int Count;
    public bool IsPrizeTaken;
    public string NextQuestTime;
    
    public void Reset()
    {
      Count = 0;
      IsPrizeTaken = false;
      NextQuestTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }

    public void TakePrize(bool isAddPause = true)
    {
      IsPrizeTaken = true;
      
      if (isAddPause == false)
        return;
      
      NextQuestTime = DateTime.Now
        .AddSeconds(QuestsStaticData.NextQuestSecondsDelay)
        .ToString(CultureInfo.InvariantCulture);
    }
    
    public DateTime NextQuestDateTime => DateTime.TryParse(NextQuestTime, out var result) ? result : DateTime.Now;
  }
}