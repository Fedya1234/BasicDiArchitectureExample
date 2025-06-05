namespace Data.Currencies
{
    public class CurrencyEvent
    {
        public CurrencyId CurrencyId;
        public int Value;
        public int PrevValue;
        public int Diff => Value - PrevValue;
        
        public CurrencyEvent(CurrencyId currencyId, int value, int prevValue)
        {
            CurrencyId = currencyId;
            Value = value;
            PrevValue = prevValue;
        }
    }
}