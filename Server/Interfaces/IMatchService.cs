using System.Collections.Generic;
using System.Threading.Tasks;
using stratzclone.Server.Models;

namespace stratzclone.Server.Interfaces
{
    public interface IMatchService
    {
        /// <summary>
        /// Fetches from Stratz and saves any new matches into the database.
        /// Returns the matches that were saved.
        /// </summary>
        Task<IEnumerable<Match>> FetchAndSaveMatchesAsync(string steamId);
    }
}
