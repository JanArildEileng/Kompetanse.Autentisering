using Authorization.FakeIdentityAndAccess.AppServices.Contracts;
using Authorization.FakeIdentityAndAccess.Infrastructure;
using Authorization.FakeIdentityAndAccess.Services.AuthorizationCode;
using Authorization.FakeIdentityAndAccess.Services.TokenGenerators;
using Common.TokenUtils;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<AuthorizationCodeCache>();

builder.Services.AddSingleton<IdTokenGenerator>();
builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();

builder.Services.AddRazorPages();

builder.Services.AddSingleton<TokenValidetorService>(x => {

    var _config = x.GetRequiredService<IConfiguration>();
    TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["RefreshJwtToken:SecretKey"])),
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidIssuer = _config["RefreshJwtToken:Issuer"],
        ValidAudience = _config["RefreshJwtToken:Audience"]
    };
    return ActivatorUtilities.CreateInstance<TokenValidetorService>(x, tokenValidationParameters);
});

builder.Services.AddSingleton<IUserRepoitory,UserRepoitory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
