using System;
using Data.Currencies;
using DG.Tweening;
using GameSetup;
using Managers;
using TMPro;
using Tools.Tweening;
using UnityEngine;

namespace UI.GameplayUI
{
  public class CurrencyIndicator : MonoBehaviour
  {
    [SerializeField] private CurrencyId _currencyId;
    [SerializeField] private TMP_Text _currencyText;

    private CurrencyManager _currencyManager;

    private void Awake()
    {
      _currencyManager = GameManager.Get<CurrencyManager>();
    }

    private void OnEnable()
    {
      _currencyManager.EventDataChanged += OnCurrencyChanged;
      _currencyText.text = _currencyManager.GetCurrency(_currencyId).Amount.ToString();
    }

    private void OnDisable()
    {
      _currencyManager.EventDataChanged -= OnCurrencyChanged;
    }

    private void OnCurrencyChanged(CurrencyEvent currencyEvent)
    {
      if (currencyEvent.CurrencyId != _currencyId)
        return;

      var isUp = currencyEvent.Value > currencyEvent.PrevValue;

      _currencyText.text = currencyEvent.PrevValue.ToString();
      _currencyText.DOCounter(currencyEvent.PrevValue, currencyEvent.Value, 0.5f)
        .OnComplete(() =>
        {
          if (isUp == false)
            return;

          _currencyText.transform.DOScale(Vector3.one * 1.2f, 0.3f)
            .SetEase(Ease.InOutBounce)
            .OnComplete(() => _currencyText.transform.DOScale(Vector3.one, 0.2f));
        });
    }
  }
}