// using Grpc.Core;
// using Microsoft.AspNetCore.Authorization;
// using MagicOnion;
// using MagicOnion.Server;

namespace UserContenter;

public interface IUserContent
{
  string uid { get; }
  string did { get; }
}

public class UserContent : IUserContent
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public string uid { get; }
  public string did { get; }

  public UserContent(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;

    var hUser = _httpContextAccessor.HttpContext.User;
    uid = hUser.FindFirst("uid")?.Value;
    did = hUser.FindFirst("did")?.Value;
  }
}