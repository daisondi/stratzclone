using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stratzclone.Server.Data;
using stratzclone.Server.External;
using stratzclone.Server.Interfaces;
using stratzclone.Server.Services;
using System.Net.Http.Headers;
using StratzClone.Server.Constants;
using StratzClone.Server.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// 1) CORS
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://localhost:3000")
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()
));

// 2) Steam OpenID
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme          = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = SteamAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(o =>
{
    o.Cookie.SameSite     = SameSiteMode.None;
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddSteam(opt =>
{
    opt.ApplicationKey = builder.Configuration["Steam:ApiKey"]!;
    opt.CallbackPath  = new PathString("/api/auth/steam/callback");
    opt.SignInScheme  = CookieAuthenticationDefaults.AuthenticationScheme;
});

// 3) EF Core
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 4) Stratz client & match service
var stratzBase    = builder.Configuration["Stratz:BaseUrl"] 
                    ?? throw new InvalidOperationException("Missing Stratz:BaseUrl");
var graphqlPath   = builder.Configuration["Stratz:GraphqlPath"] ?? "graphql";

builder.Services.AddHttpClient<IStratzApiClient, StratzApiClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["Stratz:BaseUrl"]);
    c.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", builder.Configuration["Stratz:ApiKey"]);
    c.DefaultRequestHeaders.UserAgent.ParseAdd("STRATZ_API");
    c.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddScoped<IMatchService, MatchService>();
// 5) MVC
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSingleton<IConstantsCache, ConstantsCache>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
