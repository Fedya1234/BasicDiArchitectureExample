using System;

namespace Data.Grades
{
  [Serializable]
  public class GradeData
  {
    public int Price;
    public int Value;
    
    public GradeData(int price, int value)
    {
      Price = price;
      Value = value;
    }
  }
}