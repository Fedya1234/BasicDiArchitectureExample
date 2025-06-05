using Data.Currencies;
using GameSetup;
using Managers;
using TMPro;
using UI.Gates;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayUI
{
  public class MenuUI : MonoBehaviour
  {
    private const string MaxLevel = "Max Level";
    
    [SerializeField] private Button _addCoinsButton;
    [SerializeField] private Button _gradeButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_Text _gradePriceText;

    private CurrencyManager _currencyManager;
    private GradeManager _gradeManager;

    private void Awake()
    {
      _currencyManager = GameManager.Get<CurrencyManager>();
      _gradeManager = GameManager.Get<GradeManager>();
    }

    private void OnEnable()
    {
      _startButton.onClick.AddListener(OnStartGameClicked);
      _addCoinsButton.onClick.AddListener(OnAddCoinsButtonClicked);
      _gradeButton.onClick.AddListener(OnGradeButtonClicked);
      _currencyManager.EventDataChanged += OnCurrencyChanged;
      _gradeManager.EventGrade += OnGrade;

      UpdateGradeButtonState();
      UpdatePriceText();
    }

    private void OnDisable()
    {
      _startButton.onClick.RemoveListener(OnStartGameClicked);
      _addCoinsButton.onClick.RemoveListener(OnAddCoinsButtonClicked);
      _gradeButton.onClick.RemoveListener(OnGradeButtonClicked);
      _currencyManager.EventDataChanged -= OnCurrencyChanged;
      _gradeManager.EventGrade -= OnGrade;
    }

    private void OnStartGameClicked()
    {
      //Any Checks here - for money or energy
      GatesManager.LoadSceneAsync(SceneId.Level1);
    }

    private void OnGrade()
    {
      UpdatePriceText();
      UpdateGradeButtonState();
    }

    private void OnCurrencyChanged(CurrencyEvent currencyEvent)
    {
      UpdateGradeButtonState();
    }

    private void UpdatePriceText()
    {
      _gradePriceText.text = _gradeManager.IsMaxLevel
        ? MaxLevel
        : _gradeManager.CurrentGradePrice.ToString();
    }

    private void UpdateGradeButtonState()
    {
      _gradeButton.interactable = _gradeManager.IsCanGrade();
    }

    private void OnGradeButtonClicked()
    {
      if (_gradeManager.IsCanGrade())
      {
        _gradeManager.Grade();
      }
      else
      {
        Debug.LogWarning("Cannot grade: either max level reached or not enough currency.");
      }
    }

    private void OnAddCoinsButtonClicked()
    {
      _currencyManager.AddCurrency(CurrencyId.SoftCurrency, 1);
    }
  }
}