using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.View
{
  public class MenuQuestView : MonoBehaviour
  {
    [SerializeField] private TMP_Text _count;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Button _takeButton;
    [SerializeField] private Button _skipButton;
    [SerializeField] private GameObject _canTakeGameObject;
    [SerializeField] private GameObject _timerGameObject;
    [SerializeField] private TMP_Text _timeLeftText;
    [SerializeField] private GameObject _prizeGameObject;
    [SerializeField] private TMP_Text _prizeText;

    private Action<MenuQuestView> _onQuestFinished;
    private Quest _quest;
    private bool IsCanTake => _quest.IsCompleted && _quest.IsPrizeTaken == false;
    private bool IsTimer => _quest.IsPrizeTaken && _quest.IsNextQuestReady == false;
    private bool IsDone => _quest.IsPrizeTaken && _quest.IsNextQuestReady;

    private void OnEnable()
    {
      _takeButton.onClick.AddListener(OnTakeTap);
      _skipButton.onClick.AddListener(OnSkipTap);
      UpdateView();
    }

    private void OnDisable()
    {
      _takeButton.onClick.RemoveListener(OnTakeTap);
      _skipButton.onClick.RemoveListener(OnSkipTap);
      StopAllCoroutines();
    }

    private void OnDestroy()
    {
      if (_quest == default)
        return;

      _quest.EventQuestUpdated -= OnQuestUpdated;
    }

    public void Init(Quest quest, Action<MenuQuestView> onQuestFinished)
    {
      _onQuestFinished = onQuestFinished;
      _quest = quest;

      _quest.EventQuestUpdated += OnQuestUpdated;
      UpdateView();
    }

    private IEnumerator TimerCoroutine()
    {
      while (IsTimer)
      {
        UpdateTime();
        yield return new WaitForSeconds(1);
      }

      UpdateTime();

      _onQuestFinished?.Invoke(this);
    }

    private void UpdateTime()
    {
      if (IsTimer == false)
        return;

      _timeLeftText.text = _quest.TimeToNextQuest.Hours.ToString("00") + ":" +
                           _quest.TimeToNextQuest.Minutes.ToString("00") + ":" +
                           _quest.TimeToNextQuest.Seconds.ToString("00");
    }

    private void OnQuestUpdated(Quest quest)
    {
      UpdateView();
    }

    private void UpdateView()
    {
      if (_quest == default)
        return;

      if (_quest.Icon != default)
        _icon.sprite = _quest.Icon;

      _count.text = $"{_quest.Count}/{_quest.MaxCount}";
      _name.text = _quest.Name;
      _description.text = _quest.DescriptionWithMaxCount;
      _prizeText.text = _quest.Prize.ToString();

      if (_progressBar != default)
        _progressBar.fillAmount = (float) _quest.Count / _quest.MaxCount;

      _canTakeGameObject.SetActive(IsCanTake);
      _timerGameObject.SetActive(IsTimer);
      _prizeGameObject.SetActive(_quest.IsPrizeTaken == false);

      _skipButton.gameObject.SetActive(_quest.IsCompleted == false);
      UpdateTime();

      StopAllCoroutines();

      if (IsDone)
      {
        _onQuestFinished?.Invoke(this);
        return;
      }

      if (IsTimer)
      {
        StartCoroutine(TimerCoroutine());
      }
    }

    private void OnTakeTap()
    {
      if (IsCanTake == false)
        return;

      _quest.TakePrize();
    }

    private void OnSkipTap()
    {
      _quest.SkipQuest();
    }
  }
}