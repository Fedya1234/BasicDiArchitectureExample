using System;
using Data.Currencies;

namespace Managers
{
    public class CurrencyManager
    {
        private CurrencySaveData _saveData;
        public event Action<CurrencyEvent> EventDataChanged;
        
        public CurrencyManager(CurrencySaveData saveData)
        {
            _saveData = saveData;
        }
        
        public void AddCurrency(CurrencyId currencyId, int amount)
        {
            var currency = GetCurrency(currencyId);
            currency.Amount += amount;
            EventDataChanged?.Invoke(new CurrencyEvent(currencyId, currency.Amount, currency.Amount - amount));
        }
        
        public void AddCurrency(Currency currency)
        {
            var currencyInSaveData = GetCurrency(currency.CurrencyId);
            currencyInSaveData.Amount += currency.Amount;
            EventDataChanged?.Invoke(new CurrencyEvent(currency.CurrencyId, currencyInSaveData.Amount, currencyInSaveData.Amount - currency.Amount));
        }

        public Currency GetCurrency(CurrencyId currencyId)
        {
            var currency = _saveData.Currencies.Find(c => c.CurrencyId == currencyId);
            
            if (currency != null) 
                return currency;
            
            
            currency = new Currency { CurrencyId = currencyId, Amount = 0 };
            _saveData.Currencies.Add(currency);
            return currency;
        }
        
        
        public int GetCurrencyAmount(CurrencyId currencyId) => 
            GetCurrency(currencyId).Amount;
    }
}