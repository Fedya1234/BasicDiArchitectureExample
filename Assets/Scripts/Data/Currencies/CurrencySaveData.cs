using System;
using System.Collections.Generic;

namespace Data.Currencies
{
  [Serializable]
  public class CurrencySaveData
  {
    public List<Currency> Currencies = new ();
  }
}