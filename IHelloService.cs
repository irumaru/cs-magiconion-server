using MagicOnion;
using MagicOnion.Server;
using Hello.Shared;

using Microsoft.AspNetCore.Authorization;
using Grpc.Core;

namespace Hello.Service;

public class HelloService : ServiceBase<IHelloService>, IHelloService
{
  [Authorize]
  public async UnaryResult<string> SayAsync(string mes)
  {
    var user = Context.CallContext.GetHttpContext().User.Identity.Name;

    string reMes = $"Hello {mes}";

    Console.WriteLine(reMes);
    Console.WriteLine($"User: {user}");

    return reMes;
  }
}