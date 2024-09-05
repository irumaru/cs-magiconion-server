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

  public HelloService(IUserContent userContent)
  {
    _userContent = userContent;
  }

  [Authorize]
  public async UnaryResult<string> SayAsync(string mes)
  {
    // var user = Context.CallContext.GetHttpContext().User.Identity.Name;
    string user = _userContent.user;

    string reMes = $"Hello {mes}";

    Console.WriteLine(reMes);
    Console.WriteLine($"User: {user}");

    return reMes;
  }
}