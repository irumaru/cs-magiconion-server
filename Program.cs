using MagicOnion;
using MagicOnion.Server;
using Microsoft.AspNetCore.Server.Kestrel.Core;

// Microsoft.AspNetCore.Authentication.JwtBearer
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserContenter;

public class Program
{

    // JWTのtokenを作成
    // トークン操作用のクラスを用意
    public static JwtSecurityTokenHandler JwtTokenHandler = new JwtSecurityTokenHandler();
    // 共通鍵なのでSymmetricSecurityKeyクラスを用意
    // 引数: 鍵のバイト配列
    public static SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("83b9add66092fdf9c36620764c63839e"));

    // トークンを生成
    public static string GenerateJwtToken(string name)
    {
        // nameが空の場合は例外をスロー
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidOperationException("Name is not specified.");
        }

        // トークンのクレームを作成
        var claims = new[] { new Claim(ClaimTypes.Name, name) };

        // 共有鍵の署名を作成
        var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        // トークンを作成
        var token = new JwtSecurityToken(
            issuer: "ExampleServer",
            audience: "ExampleClients",
            claims: claims,
            expires: DateTime.Now.AddSeconds(300),
            signingCredentials: credentials
        );

        // JwtSecurityToken => Compact Serialization Format => シリアライズ
        return JwtTokenHandler.WriteToken(token);
    }

    public static void Main(string[] args)
    {
        Console.WriteLine($"Security key size: {SecurityKey.KeySize}");

        var builder = WebApplication.CreateBuilder(args);

        // Kestrel サーバーの設定を追加して、h2c を有効にする
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
        });

        builder.Services.AddGrpc();       // Add this line(Grpc.AspNetCore)
        builder.Services.AddHttpContextAccessor(); // Add this line(Microsoft.AspNetCore.Http)
        builder.Services.AddScoped<IUserContent, UserContent>(); // Add this line(UserContenter)
        builder.Services.AddMagicOnion(); // Add this line(MagicOnion.Server)

        // 認証を追加

        // 認証サービスに必要なサービスを追加
        // デフォルトの認可スキームに，Nameを要求するポリシーを追加
        builder.Services.AddAuthorization(options =>
        {
            // 認可ポリシーを追加
            // これをデフォルトで使用
            options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
            {
                // Jwtのデフォルトの認証スキーム
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                // Nameクレームを要求
                policy.RequireClaim(ClaimTypes.Name);
            });
        });

        // 認証サービスに必要なサービスを追加
        // JwtBearerDefaults.AuthenticationScheme(上で作成した)を使用してJWT認証を追加
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // Jwtベアラー認証のオプションを設定
            .AddJwtBearer(options =>
            {
                // トークンの検証パラメータを設定します。
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        // トークンの受信者（audience）を検証しません。
                        ValidateAudience = false,
                        // トークンの発行者（issuer）を検証しません。
                        ValidateIssuer = false,
                        // トークンのアクター（actor）を検証しません。
                        ValidateActor = false,
                        // トークンの有効期限を検証します。
                        ValidateLifetime = true,
                        // トークンの署名を検証するためのキーを指定します。
                        IssuerSigningKey = SecurityKey
                    };
            }
        );

        var app = builder.Build();

        IWebHostEnvironment env = app.Environment;

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // ルーティングを追加
        app.UseRouting();

        // 認証と認可を追加
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapMagicOnionService();

        app.Run();
    }

}