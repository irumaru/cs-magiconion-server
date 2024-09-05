// using Grpc.Core;
// using Microsoft.AspNetCore.Authorization;
// using MagicOnion;
// using MagicOnion.Server;

namespace UserContenter;

public interface IUserContent
{
  string user { get; }
}

public class UserContent : IUserContent
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public string user { get; }

  public UserContent(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;

    user = _httpContextAccessor.HttpContext.User.Identity.Name;
  }
}