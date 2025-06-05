using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QuestSystem.View
{
  public class InGameQuestView : MonoBehaviour
  {
    [SerializeField] private TMP_Text _count;
    [SerializeField] private TMP_Text _shortDescription;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Image _icon;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _movedContainer;
    [SerializeField] private Button _button;
    [SerializeField] private UnityEvent _onOneDone;
    [SerializeField] private UnityEvent _onCompleted;
    [SerializeField] private float _moveDistance = 500;
    private Quest _quest;
    public Quest Quest => _quest;
    private Vector3 _startPosition;

    private bool _isOpened;

    public void Init(Quest quest)
    {
      _quest = quest;

      if (quest.Icon != default)
        _icon.sprite = quest.Icon;

      _count.text = $"{quest.MaxCount - quest.Count}";
      _shortDescription.text = quest.ShortDescription;
      _description.text = quest.DescriptionWithMaxCount;
      ShowDescription(false);

      if (_progressBar != default)
        _progressBar.fillAmount = (float) quest.Count / quest.MaxCount;

      quest.EventQuestUpdated += OnQuestUpdated;

      ShowHidePanel();
    }

    private void Awake()
    {
      _startPosition = _movedContainer.localPosition;
    }

    private void OnEnable()
    {
      _button.onClick.AddListener(ShowHidePanel);
    }

    private void OnDisable()
    {
      _quest.EventQuestUpdated -= OnQuestUpdated;
      _button.onClick.RemoveListener(ShowHidePanel);
    }

    private void ShowHidePanel()
    {
      DOTween.Kill(_movedContainer);
      _isOpened = !_isOpened;
      _movedContainer.DOLocalMoveX(_isOpened ? _startPosition.x - _moveDistance : _startPosition.x, 0.3f);
      ShowDescription(_isOpened);
      if (_isOpened)
      {
        _movedContainer
          .DOLocalMoveX(_startPosition.x, 0.3f)
          .SetDelay(3f)
          .OnComplete(() =>
          {
            _isOpened = false;
            ShowDescription(_isOpened);
          });
      }
    }

    private void ShowDescription(bool value)
    {
      _description.gameObject.SetActive(value);
      _count.gameObject.SetActive(!value);
      _shortDescription.gameObject.SetActive(!value);
    }

    private void OnQuestUpdated(Quest quest)
    {
      _count.text = $"{quest.MaxCount - quest.Count}";
      if (_progressBar != default)
        _progressBar.fillAmount = (float) quest.Count / quest.MaxCount;

      var tween = DOTween.Sequence()
        .Append(_container.DOScale(1.2f, 0.3f))
        .Append(_container.DOScale(1f, 0.3f));

      if (quest.IsCompleted)
      {
        tween.Append(_container.DOScale(0.1f, 0.3f).SetEase(Ease.InBack))
          .OnComplete(() => gameObject.SetActive(false));

        _onCompleted.Invoke();
      }
      else
      {
        _onOneDone.Invoke();
      }

      tween.Play();
    }
  }
}