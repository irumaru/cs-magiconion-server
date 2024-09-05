using MagicOnion;
using MagicOnion.Server;
using Hello.Shared;

namespace Account.Service;

public class AccountService : ServiceBase<IAccountService>, IAccountService
{
  public async UnaryResult<string> CreateAccountAsync(string user)
  {
    Console.WriteLine($"Create account.");
    string token = Program.GenerateJwtToken(user);

    Console.WriteLine($"User: {user}");
    Console.WriteLine($"Token: {token}");

    return token;
  }
}
