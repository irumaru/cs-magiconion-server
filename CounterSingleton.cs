public class CounterSingleton
{
  private static CounterSingleton _instance;
  private int count;

  private CounterSingleton()
  {
    count = 0;
  }

  public static CounterSingleton GetInstance()
  {
    if(_instance == null)
    {
      _instance = new CounterSingleton();
    }

    return _instance;
  }

  public int GetCount()
  {
    return count;
  }

  public void Increment()
  {
    count++;
  }
}
