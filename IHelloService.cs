using MagicOnion;
using MagicOnion.Server;
using Hello.Shared;

namespace Hello.Service;

public class HelloService : ServiceBase<IHelloService>, IHelloService
{
  public async UnaryResult<string> SayAsync(string mes)
  {
    string ss = $"Hello {mes}";

    Console.WriteLine(ss);

    return ss;
  }
}