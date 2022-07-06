
using Authorization.RefitApi;
using Authorization.WebBFFApplication.AppServices.Contracts;
using Authorization.WebBFFApplication.AppServices.Features.Backend;
using Authorization.WebBFFApplication.AppServices.Features.IdentityAndAccess;
using Authorization.WebBFFApplication.AppServices.Services;
using Authorization.WebBFFApplication.Infrastructure;
using Common.TokenUtils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddRazorPages();


builder.Services.AddScoped<IIdentityAndAccessApiService, IdentityAndAccessApiService>();
builder.Services.AddScoped<IBackendApiService, BackendApiService>();

//builder.Services.AddSingleton<TokenValidetorService>(new );

builder.Services.AddSingleton<TokenValidetorService>(x => {

      var _config = x.GetRequiredService<IConfiguration>();
      TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
      {
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["IdJwtToken:SecretKey"])),
         RequireExpirationTime = true,
         ValidateLifetime = true,
         ValidateAudience = true,
         ValidateIssuer = true,
         ValidIssuer = _config["IdJwtToken:Issuer"],
         ValidAudience = _config["IdJwtToken:Audience"]
     };
     return ActivatorUtilities.CreateInstance<TokenValidetorService>(x, tokenValidationParameters);
    });



builder.Services.AddSingleton<TokenCacheManager>();
builder.Services.AddScoped<TokenFreshService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<RestrictedDataService>();






builder.Services.AddRefitClient<IBackendApi>()
        .ConfigureHttpClient(c => 
        {
            c.BaseAddress = new Uri("https://localhost:7170/");
        });


builder.Services.AddRefitClient<IIdentityAndAccessApi>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri("https://localhost:7134/");
        }
    );

builder.Services.AddHttpClient();



builder.Services.AddAuthentication(option=> 
   {
       option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
       option.DefaultChallengeScheme= CookieAuthenticationDefaults.AuthenticationScheme;
       option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
   }).     
    AddCookie(option =>
    {
        option.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();



app.Run();
