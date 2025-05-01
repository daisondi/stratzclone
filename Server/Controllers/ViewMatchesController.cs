using System;                               // for TimeSpan
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stratzclone.Server.Data;
using StratzClone.Server.Models;
using StratzClone.Server.Constants;
using StratzClone.Server.Interfaces;

namespace StratzClone.Server.Controllers;

[ApiController]
public class ViewMatchesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IConstantsCache _consts;   // tiny helper, see § 1.3

    public ViewMatchesController(ApplicationDbContext db, IConstantsCache consts)
    {
        _db = db;
        _consts = consts;
    }

    /// <summary>
    /// GET /api/viewmacthes?page=1&pageSize=20   ← spelling kept per spec
    /// </summary>
    [HttpGet("/api/viewmatches")]
    [Authorize]   // user must be logged in (JWT cookie / header)
    public async Task<ActionResult<PagedResponse<MatchViewModel>>> ViewMine(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        // Current user → SteamId64 comes from token (filled by /api/auth/steam)
        var steamId64 = User.FindFirstValue("steamId");   // adjust if claim-type differs
        if (steamId64 is null) return Unauthorized();

        // Query PlayerMatches that belong to this steam user
        var q = _db.PlayerMatches
                   .Include(pm => pm.Match)          // brings Duration, StartTime, result
                   .Where(pm => pm.SteamId == steamId64)
                   .OrderByDescending(pm => pm.Match.StartDateUtc);

        var total = await q.CountAsync();

        var slice = await q.Skip((page - 1) * pageSize).Take(pageSize).Select(pm => new MatchViewModel
        {
            MatchId = pm.MatchId,
            StartTimeUtc = pm.Match.StartDateUtc,
            Duration = TimeSpan.FromSeconds(pm.Match.DurationSecs),

            // NEW: full hero object, no HeroId
            Hero = _consts.GetHero(pm.HeroId),

            IsWin = (pm.IsRadiant && pm.Match.DidRadiantWin)
               || (!pm.IsRadiant && !pm.Match.DidRadiantWin),
            Kills = pm.Kills,
            Deaths = pm.Deaths,
            Assists = pm.Assists,

            Item0 = _consts.GetItem(pm.Item0Id),
            Item1 = _consts.GetItem(pm.Item1Id),
            Item2 = _consts.GetItem(pm.Item2Id),
            Item3 = _consts.GetItem(pm.Item3Id),
            Item4 = _consts.GetItem(pm.Item4Id),
            Item5 = _consts.GetItem(pm.Item5Id),
        })
      .ToListAsync();


        return Ok(new PagedResponse<MatchViewModel>(slice, page, pageSize, total));
    }
}
