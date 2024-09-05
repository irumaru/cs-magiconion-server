using MagicOnion;
using MagicOnion.Server;
using Hello.Shared;

using Microsoft.AspNetCore.Authorization;

namespace Hello.Service;

public class HelloService : ServiceBase<IHelloService>, IHelloService
{
  [Authorize]
  public async UnaryResult<string> SayAsync(string mes)
  {
    string ss = $"Hello {mes}";

    Console.WriteLine(ss);

    return ss;
  }
}