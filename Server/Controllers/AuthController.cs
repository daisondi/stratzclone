using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using stratzclone.Server.Data;
using stratzclone.Server.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    // ───────────────────────────────────────────────────────────────
    // 1) Kick-off: /api/auth/steam/login
    // ───────────────────────────────────────────────────────────────
    [HttpGet("steam/login")]
    public IActionResult SteamLogin([FromQuery] string returnUrl = "/")
    {
        Console.WriteLine($"[SteamLogin] redirect to Steam, returnUrl = {returnUrl}");
        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        return Challenge(props, SteamAuthenticationDefaults.AuthenticationScheme);
    }

    // ───────────────────────────────────────────────────────────────
    // 2) “Who am I”:  /api/auth/steam/me   (requires cookie)
    // ───────────────────────────────────────────────────────────────
    [Authorize]
    [HttpGet("steam/me")]
    public async Task<IActionResult> Me()
    {
        Console.WriteLine("[Me] endpoint hit");

        // 2-a) OpenID claim
        var openId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine($"[Me] openId   : {openId}");
        if (openId == null)
        {
            Console.WriteLine("[Me] openId missing → 401");
            return Unauthorized();
        }

        // 2-b) Steam64 ID
        var steam64 = openId.TrimEnd('/').Split('/').Last();
        Console.WriteLine($"[Me] steam64  : {steam64}");

        // 2-c) Steam Web API call
        var apiKey = _config["Steam:ApiKey"];
        var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={apiKey}&steamids={steam64}";
        Console.WriteLine($"[Me] Steam API: {url}");

        using var http = new HttpClient();
        var resp = await http.GetFromJsonAsync<SteamProfileResponse>(url);
        var info = resp?.response.players?.FirstOrDefault();
        if (info == null)
        {
            Console.WriteLine("[Me] Steam returned 0 players → 404");
            return NotFound();
        }
        Console.WriteLine($"[Me] persona  : {info.personaname}");
        // ── compute SteamID32 ────────────────────────────────────
        // Steam32 = steam64 - 76561197960265728
        var sid64 = ulong.Parse(info.steamid);
        var sid32 = (sid64 - 76561197960265728UL).ToString();
        // 2-d) Upsert DB row
        var player = await _db.Players.FindAsync(steam64);
        if (player == null)
        {
            Console.WriteLine("[Me] inserting new player");
            player = new Player
            {
                SteamId32 = sid32,
                SteamId = info.steamid,
                DisplayName = info.personaname,
                Username = info.personaname,
                ProfilePictureUrl = info.avatarfull
            };
            _db.Players.Add(player);
        }
        else
        {
            Console.WriteLine("[Me] updating existing player");
            player.DisplayName = info.personaname;
            player.Username = info.personaname;
            player.SteamId32 = sid32;
            player.ProfilePictureUrl = info.avatarfull;
            _db.Players.Update(player);
        }
        await _db.SaveChangesAsync();
        Console.WriteLine("[Me] DB save completed");

        // 2-e) Return JSON
        return Ok(player);
    }
}
