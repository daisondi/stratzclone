using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stratzclone.Server.Data;
using stratzclone.Server.Interfaces;
using stratzclone.Server.Models;

namespace stratzclone.Server.Services
{

    public class MatchService : IMatchService
    {
        private readonly IStratzApiClient _api;
        private readonly ApplicationDbContext _db;

        public MatchService(IStratzApiClient api, ApplicationDbContext db)
        {
            _api = api;
            _db = db;
        }

        public async Task<IEnumerable<Match>> FetchAndSaveMatchesAsync(string steamId)
        {
            var fetched = await _api.GetRecentMatchesAsync(steamId);
            var toSave = new List<Match>();

            foreach (var match in fetched)
            {
                var exists = await _db.Matches
                                      .AsNoTracking()
                                      .AnyAsync(m => m.MatchId == match.MatchId);
                if (exists) continue;

                // hook up navigations
                foreach (var pm in match.PlayerMatches)
                {
                    pm.Match = match;
                    foreach (var item in pm.Items)
                        item.PlayerMatch = pm;
                }

                _db.Matches.Add(match);
                toSave.Add(match);
            }

            if (toSave.Any())
                await _db.SaveChangesAsync();

            return toSave;
        }
        public async Task<IEnumerable<PlayerMatch>> FetchAndSavePlayerMatchesAsync(string steamId)
        {
            var playerMatches = await _api.GetPlayerMatchesAsync(steamId);
            var toSave = new List<PlayerMatch>();

            foreach (var pm in playerMatches)
            {
                var exists = await _db.PlayerMatches
                    .AsNoTracking()
                    .AnyAsync(x => x.MatchId == pm.MatchId && x.SteamId == pm.SteamId);

                if (exists)
                    continue;

                _db.PlayerMatches.Add(pm);
                toSave.Add(pm);
            }

            if (toSave.Any())
                await _db.SaveChangesAsync();

            return toSave;
        }
    }
}
