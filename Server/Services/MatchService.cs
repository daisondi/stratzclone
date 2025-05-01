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
            var toSaveAll = new List<Match>();
            var skip = 0;
            const int take = 100; // batch size

            while (true)
            {
                // Fetch a page of matches
                var batch = await _api.GetRecentMatchesAsync(steamId, skip, take);

                // Filter out already-saved matches
                var newMatches = batch
                    .Where(m => !_db.Matches
                        .AsNoTracking()
                        .Any(x => x.MatchId == m.MatchId))
                    .ToList();

                // If no new matches found, exit the loop
                if (!newMatches.Any())
                    break;

                // Hook up navigations and add each new match
                foreach (var match in newMatches)
                {
                    foreach (var pm in match.PlayerMatches)
                    {
                        pm.Match = match;
                        foreach (var item in pm.Items)
                            item.PlayerMatch = pm;
                    }
                    _db.Matches.Add(match);
                }

                // Persist this batch of new matches
                await _db.SaveChangesAsync();
                toSaveAll.AddRange(newMatches);

                // Advance to the next page
                skip += take;
            }

            return toSaveAll;
        }


    }
}