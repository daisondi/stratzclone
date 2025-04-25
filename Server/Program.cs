using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stratzclone.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) CORS to trust React dev server
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              // If you need to call the backend directly (e.g. for /me), include its URL too:
              //.WithOrigins("http://localhost:3000", "https://localhost:7065")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 2) Auth: cookies + Steam OpenID
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = SteamAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
  {
    options.Cookie.SameSite = SameSiteMode.None;
    // Only require Secure in production
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
    ? CookieSecurePolicy.None
    : CookieSecurePolicy.Always;
  })
.AddSteam(options =>
{
    options.ApplicationKey = builder.Configuration["Steam:ApiKey"];
    options.CallbackPath  = new PathString("/api/auth/steam/callback");
    options.SignInScheme  = CookieAuthenticationDefaults.AuthenticationScheme;
});
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);
// 3) Authorization & controllers
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// (optional) redirect HTTP → HTTPS
// app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
