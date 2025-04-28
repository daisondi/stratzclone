using System.Collections.Generic;
using System.Threading.Tasks;
using stratzclone.Server.Models;

namespace stratzclone.Server.Interfaces
{
    public interface IStratzApiClient
    {
        /// <summary>
        /// Fetches the most recent matches for the given Steam-64 ID.
        /// </summary>
        Task<IEnumerable<Match>> GetRecentMatchesAsync(string steamId);
        Task<IEnumerable<PlayerMatch>> GetPlayerMatchesAsync(
        string steamId,
        int skip = 0,
        int take = 100
    );
    }
}
