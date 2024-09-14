using MagicOnion;
using MagicOnion.Server;
using Hello.Shared;

using Microsoft.AspNetCore.Authorization;
using Grpc.Core;

using UserContenter;

namespace Hello.Service;

public class HelloService : ServiceBase<IHelloService>, IHelloService
{
  private readonly IUserContent _userContent;
  private int count;

  public HelloService(IUserContent userContent)
  {
    _userContent = userContent;
    count = 0;
  }

  [Authorize]
  public async UnaryResult<string> SayAsync(string mes)
  {
    // var user = Context.CallContext.GetHttpContext().User.Identity.Name;
    
    string reMes = $"Hello {mes}";

    Console.WriteLine(reMes);
    Console.WriteLine($"uid: {_userContent.uid}");
    Console.WriteLine($"did: {_userContent.did}");

    count++;
    Console.WriteLine($"count: {count}");

    var counter = CounterSingleton.GetInstance();
    counter.Increment();
    Console.WriteLine($"counter: {counter.GetCount()}");

    return reMes;
  }
}