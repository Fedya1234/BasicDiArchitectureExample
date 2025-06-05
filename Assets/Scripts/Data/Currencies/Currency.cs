using System;

namespace Data.Currencies
{
    [Serializable]
    public class Currency
    {
        public CurrencyId CurrencyId;
        public int Amount;
    }
}