using Data.Currencies;
using GameSetup;
using Managers;
using UnityEngine;

namespace QuestSystem.QuestTypes
{
    public class GetCurrencyQuest : Quest // Its Just an Example - its no Quest like this in gamePlay
    {
        [SerializeField] private CurrencyId _id;

        private CurrencyManager _currencyManager;
        protected override void OnInitialized()
        {
            _currencyManager = GameManager.Get<CurrencyManager>();
        }

        public override void AddListener()
        {
            base.AddListener();
            _currencyManager.EventDataChanged += OnDataChanged;
        }
    
        public override void RemoveListener()
        {
            base.RemoveListener();
            _currencyManager.EventDataChanged += OnDataChanged;
        }

        private void OnDataChanged(CurrencyEvent currencyEvent)
        {
            if (currencyEvent.CurrencyId != _id)
                return;

            var diff = currencyEvent.Diff;
            if (diff <= 0)
                return;

            AddProgress(diff);
        }
    }
}