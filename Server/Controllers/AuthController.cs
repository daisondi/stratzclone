using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;
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

    // existing login kickoff
    [HttpGet("steam/login")]
    public IActionResult SteamLogin([FromQuery] string returnUrl = "/")
    {
        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        return Challenge(props, SteamAuthenticationDefaults.AuthenticationScheme);
    }

    // new “who am I” endpoint
    [Authorize]
    [HttpGet("steam/me")]
    public async Task<IActionResult> Me()
    {
        // 1) pull the OpenID claim
        var openId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (openId == null) 
            return Unauthorized();

        // 2) extract Steam64 ID
        var steam64 = openId.TrimEnd('/').Split('/').Last();

        // 3) fetch from Steam Web API
        var apiKey = _config["Steam:ApiKey"];
        var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={apiKey}&steamids={steam64}";
        using var http = new HttpClient();
        var resp = await http.GetFromJsonAsync<SteamProfileResponse>(url);
        var info = resp?.response.players?.FirstOrDefault();
        if (info == null) 
            return NotFound();

        // 4) (optional) upsert into the DB
        var player = await _db.Players.FindAsync(steam64);
        if (player == null)
        {
            player = new Player {
                SteamId     = info.steamid,
                DisplayName = info.personaname,
                Username    = info.personaname
            };
            _db.Players.Add(player);
        }
        else
        {
            player.DisplayName = info.personaname;
            player.Username    = info.personaname;
            _db.Players.Update(player);
        }
        await _db.SaveChangesAsync();

        // 5) return it
        return Ok(player);
    }
}
